using System;
using NLua;

namespace TextRPG
{
    public static class LuaManager
    {
        public static bool creatingScript;
        private static string currScriptName;
        private static string[] script;
        private static int scriptSize = 50;
        private static int currScriptSize = 0;

        /// <summary>
        /// Try to execute a Lua command given in a string
        /// </summary>
        /// <param name="luaCommand"></param>
        public static bool executeCommand(string luaCommand)
        {
            Lua lua = new Lua();    
            lua.LoadCLRPackage();
            lua.DoString(@" import ('TextRPG', 'TextRPG') 
               import ('System') ");
            try {
                lua.DoString(luaCommand);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Start recording a lua script with a specific name
        /// </summary>
        /// <param name="scriptName"></param>
        public static void startScript(string scriptName)
        {
            creatingScript = true;
            script = new string[scriptSize];
            currScriptSize = 0;
            currScriptName = scriptName+".lua";
            GUI.Instance.appendToOutput("Started Script with name : " + currScriptName + " \nWrite the Script below.");
        }

        /// <summary>
        /// Finalise a user created script by writing it to file
        /// </summary>
        public static void endScript()
        {
            creatingScript = true;
            System.IO.File.WriteAllLines(currScriptName, script);
            GUI.Instance.appendToOutput("Created script " + currScriptName);
        }

        /// <summary>
        /// Append a string to the existing current lua script
        /// </summary>
        /// <param name="s"></param>
        public static void appendToScript(string s)
        {
            script[currScriptSize++] = s;
        }

        /// <summary>
        /// Execute a script from file by name
        /// </summary>
        /// <param name="scriptName"></param>
        public static bool executeScript(string scriptName)
        {
            Lua lua = new Lua();
            lua.LoadCLRPackage();
            lua.DoString(@" import ('TextRPG', 'TextRPG') 
               import ('System') ");
            try
            {
                var x = lua.DoFile(scriptName+".lua");
                GUI.Instance.appendToOutput(x.ToString());
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
