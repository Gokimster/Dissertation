using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace TextRPG
{
    public class CombatManager
    {
        public static readonly CombatManager Instance = new CombatManager();
        private XElement xElem;
        Dictionary<string,string> scripts;

        public CombatManager()
        {
            loadScripts();
        }

        private void loadScripts()
        {
            xElem = PersistenceManager.initXML(Properties.Settings.Default.combatScriptsFile, "combatScripts");
            scripts = new Dictionary<string, string>();
            foreach (var script in xElem.Elements())
            {
                scripts.Add(script.Element("function").Value, script.Element("script").Value);
            }
        }

        public void removeScriptForFunction(string function)
        {
            scripts.Remove(function);
            var ar = from script in xElem.Elements("script")
                     where script.Element("function").Value == function
                     select script;
            foreach (XElement xel in ar)
            {
                xel.Remove();
            }
            xElem.Save(Properties.Settings.Default.combatScriptsFile);
        }

        public void addScriptToFunction(string function, string script)
        {
            removeScriptForFunction(function);
            scripts.Add(function, script);
            xElem.Add(new XElement("script", new XElement("function", function), new XElement("script", script)));
            xElem.Save(Properties.Settings.Default.combatScriptsFile);
        }

        public void doCombat(Enemy e)
        {
            string scriptFile;
            if (scripts.TryGetValue("mainCombat", out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
            else {
                fightTillDeath(e);
            }
        }

        public static void fightTillDeath(Enemy e)
        {
            while (!e.isDead() && !Player.Instance.isDead())
            {
                attackPhaseSimultaneous(e);
            }
        }

        public static void playerAttack(Enemy e)
        {
            e.takeDmg(Player.Instance.getDmgDone());
        }

        public static void playerDefend()
        {

        }

        public static void enemyAttack(Enemy e)
        {
            Player.Instance.takeDmg(e.getDmgDone());
        }

        public static void enemyDefend()
        {

        }


        public static void attackPhaseSimultaneous(Enemy e)
        {
            playerAttack(e);
            enemyAttack(e);
        }
    }
}
