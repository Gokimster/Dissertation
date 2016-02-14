using System;
using NLua;

namespace TextRPG
{
    public static class LuaManager
    {
        //static Lua lua = new Lua();
        public static bool executeCommand(string luaCommand)
        {
            Lua lua = new Lua();    
            lua.LoadCLRPackage();
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
    }
}
