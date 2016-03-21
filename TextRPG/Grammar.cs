using System;
using Irony.Parsing;
using System.Diagnostics;

namespace TextRPG
{
    public class Grammar : Irony.Parsing.Grammar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Grammar"/> class.
        /// </summary>
        public Grammar() : base(false)
        { // true means case sensitive
            GrammarComments = @"The Grammar for in game text commands";

            // Terminals (Lexing)
            NumberLiteral number = new NumberLiteral("number");
            StringLiteral STRING = new StringLiteral("STRING", "\"");

            //Non-Terminals (Parsing)
            NonTerminal command = new NonTerminal("command");

            //move
            NonTerminal moveCommand = new NonTerminal("moveCommand");
            NonTerminal moveOp = new NonTerminal("moveOp");
            NonTerminal moveDir = new NonTerminal("moveDirection");

            //attack
            NonTerminal attackCommand = new NonTerminal("attackCommand");
            NonTerminal attackOp = new NonTerminal("attackOp");

            //pick up item
            NonTerminal pickUpCommand = new NonTerminal("pickUpCommand");
            NonTerminal pickUpOp = new NonTerminal("pickUpOp");            

            //help commands
            NonTerminal helpCommand = new NonTerminal("helpCommand");

            NonTerminal inventoryCommand = new NonTerminal("inventoryCommand");
            NonTerminal inventoryOp = new NonTerminal("inventoryOp");
            NonTerminal inventoryAlias = new NonTerminal("inventoryAlias");

            NonTerminal inspectCommand = new NonTerminal("inspectCommand");
            NonTerminal inspectOp = new NonTerminal("inspectOp");

            NonTerminal name = new NonTerminal("name");
            NonTerminal nameString = new NonTerminal("nameString");

            //lua commands
            NonTerminal scriptCommand = new NonTerminal("scriptCommand");
            NonTerminal scriptOp = new NonTerminal("scriptOp");
            NonTerminal endScriptOp = new NonTerminal("endScriptOp");
            NonTerminal endScriptCommand = new NonTerminal("endScriptCommand");


            command.Rule = moveCommand | pickUpCommand | helpCommand | attackCommand | scriptCommand | endScriptCommand;
            //identifiers
            name.Rule = new IdentifierTerminal("identifier");
            nameString.Rule = name | name + nameString;

            //move
            moveOp.Rule = ToTerm("go") | "head";
            moveDir.Rule = ToTerm("north") | "south" | "east" | "west" | "N" | "S" | "E" | "W";
            moveCommand.Rule = moveOp + moveDir;

            //pick up item
            pickUpOp.Rule = ToTerm("pick up") | "get" | "take";
            pickUpCommand.Rule = pickUpOp +  nameString;

            //attack
            attackOp.Rule = ToTerm("attack") | "engage" | "fight";
            attackCommand.Rule = attackOp + nameString;

            //help commands
            inventoryOp.Rule = ToTerm("show") | "display";
            inventoryAlias.Rule = ToTerm("inventory") | "items";
            inventoryCommand.Rule = inventoryAlias | inventoryOp + inventoryAlias;

            inspectOp.Rule = ToTerm("inspect") | "view" | "show";
            inspectCommand.Rule = inspectOp + nameString;
            //TO DO: add rule to help command
            helpCommand.Rule = inventoryCommand | inspectCommand;

            //script commands
            scriptOp.Rule = ToTerm("script") | "new script" | "lua script";
            endScriptOp.Rule = ToTerm("end") | "finish" | "stop";
            scriptCommand.Rule = scriptOp + name;
            endScriptCommand.Rule = endScriptOp + scriptOp;

            this.Root = command;
        }


        /// <summary>
        /// displays the tree from a given node and level
        /// </summary>
        private void displayTree(ParseTreeNode n, int level)
        {
            for(int i = 0; i < level; i++)
            {
                Debug.Write(" ");
            }
            Debug.WriteLine(n);
            foreach(ParseTreeNode child in n.ChildNodes)
            {
                displayTree(child, level + 1);
            }
        }

        private void doCommand(ParseTreeNode n)
        {
            var mainNode = n.ChildNodes[0];
            if (mainNode.Term.Name == "moveCommand")
            {
                GameManager.changeCurrentArea(mainNode.ChildNodes[1].ChildNodes[0].Term.Name);
            }

            if(mainNode.Term.Name == "helpCommand")
            {
                if(mainNode.ChildNodes[0].Term.Name == "inventoryCommand")
                {
                    Debug.WriteLine("ShowInventory");
                    GameManager.showPlayerInventory();
                }
                else
                {
                    if (mainNode.ChildNodes[0].Term.Name == "inspectCommand")
                    {
                        string s = getStringFromNameString(mainNode.ChildNodes[0].ChildNodes[1]);
                        GameManager.inspect(s);
                    }
                }
            }

            if(mainNode.Term.Name == "pickUpCommand")
            {
                if (mainNode.ChildNodes[1] != null && mainNode.ChildNodes[1].ChildNodes[0] != null)
                {
                    string s = getStringFromNameString(mainNode.ChildNodes[1]);
                    GameManager.pickUpItem(s);
                }
            }
            if (mainNode.Term.Name == "attackCommand")
            {
                if (mainNode.ChildNodes[1] != null && mainNode.ChildNodes[1].ChildNodes[0] != null)
                {
                    string s = getStringFromNameString(mainNode.ChildNodes[1]);
                    GameManager.doCombat(s);
                }
            }

            if (mainNode.Term.Name == "endScriptCommand")
            {
                LuaManager.endScript();
            }

            if (mainNode.Term.Name == "scriptCommand")
            {
                if (mainNode.ChildNodes[1] != null && mainNode.ChildNodes[1].ChildNodes[0] != null)
                {
                    string s = (string)mainNode.ChildNodes[1].ChildNodes[0].Token.Value;
                    LuaManager.startScript(s);
                }
            }
        }

        private string getStringFromNameString(ParseTreeNode n, string s = "" )
        {
            s += n.ChildNodes[0].ChildNodes[0].Token.Value;
            if(n.ChildNodes.Count > 1)
            {
                s += " ";
                return getStringFromNameString(n.ChildNodes[1], s);
            }
            return s;
        }

        /// <summary>
        /// Parses a string using the initialized grammar
        /// </summary>
        public bool parse(String s)
        {
            LanguageData language = new LanguageData(this);
            Parser parser = new Parser(language);
            ParseTree parseTree = parser.Parse(s);
            ParseTreeNode root = parseTree.Root;
            //check if input is valid
            if(root!=null)
            {
                displayTree(root, 0);
                doCommand(root);
                return true;
            }
            else
            {
                if (LuaManager.creatingScript)
                {
                    LuaManager.appendToScript(s);
                }
                else {
                    if(!LuaManager.executeCommand(s))
                    {
                        GUI.Instance.appendToOutput("No such command.");
                    }

                }
                return true;
            }


        }
    }
}
