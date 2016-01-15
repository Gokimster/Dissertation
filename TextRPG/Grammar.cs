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

            NonTerminal value = new NonTerminal("value");

            value.Rule = number | STRING;
            command = moveCommand;

            //move
            moveOp.Rule = ToTerm("go") | "head";
            moveDir.Rule = ToTerm("north") | "south" | "east" | "west" | "N" | "S" | "E" | "W";
            moveCommand.Rule = moveOp + moveDir;

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

        private void doMove(ParseTreeNode n)
        {
            if (n.Term.Name == "moveCommand")
            {
                if (AreaManager.Instance.changeCurrentArea(n.ChildNodes[1].ChildNodes[0].Term.Name))
                {
                    Debug.WriteLine("Moved");
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
                doMove(root);
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
