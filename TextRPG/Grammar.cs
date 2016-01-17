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

            //help commands
            NonTerminal helpCommand = new NonTerminal("helpCommand");
            NonTerminal inventoryCommand = new NonTerminal("inventoryCommand");
            NonTerminal inventoryOp = new NonTerminal("inventoryOp");
            NonTerminal inventoryAlias = new NonTerminal("inventoryAlias");

            NonTerminal value = new NonTerminal("value");

            value.Rule = number | STRING;
            command.Rule = moveCommand | helpCommand;

            //move
            moveOp.Rule = ToTerm("go") | "head";
            moveDir.Rule = ToTerm("north") | "south" | "east" | "west" | "N" | "S" | "E" | "W";
            moveCommand.Rule = moveOp + moveDir;

            //help commands
            inventoryOp.Rule = ToTerm("show") | "display";
            inventoryAlias.Rule = ToTerm("inventory") | "items";
            inventoryCommand.Rule = inventoryAlias | inventoryOp + inventoryAlias;
            //TO DO: add rule to help command
            helpCommand.Rule = inventoryCommand;

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
                if (GameManager.changeCurrentArea(n.ChildNodes[1].ChildNodes[0].Term.Name))
                {
                    Debug.WriteLine("Moved");
                }
            }

            if(mainNode.Term.Name == "helpCommand")
            {
                if(mainNode.ChildNodes[0].Term.Name == "inventoryCommand")
                {
                    Debug.WriteLine("ShowInventory");
                    GameManager.showPlayerInventory();
                }
            }
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
                Debug.WriteLine("Input not valid");
                return false;
            }

        }
    }
}
