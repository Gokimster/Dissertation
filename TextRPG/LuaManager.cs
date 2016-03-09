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

        //static Lua lua = new Lua();
        public static bool executeCommand(string luaCommand)
        {
            Lua lua = new Lua();    
            lua.LoadCLRPackage();
            lua.DoString(@" import ('TextRPG', 'TextRPG') 
               import ('System') ");
            try {
                var x = lua.DoString(luaCommand);
                GUI.Instance.appendToOutput(x.ToString());
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public static void startScript(string scriptName)
        {
            creatingScript = true;
            script = new string[scriptSize];
            currScriptSize = 0;
            currScriptName = scriptName+".lua";
            GUI.Instance.appendToOutput("Started Script with name : " + currScriptName + " \nWrite the Script below.");
        }

        public static void endScript()
        {
            creatingScript = true;
            System.IO.File.WriteAllLines(currScriptName, script);
        }

        public static void appendToScript(string s)
        {
            script[currScriptSize++] = s;
        }

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
