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
        public Enemy enemy { get; set; }

        public CombatManager()
        {
            enemy = null;
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

        public void doPhase(int phase)
        {
            string scriptFile;
            if (scripts.TryGetValue("phase" + phase, out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
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
            enemy = e;
            if (scripts.TryGetValue("mainCombat", out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
            else {
                fightTillDeath();
            }
            enemy = null;
        }

        public void fightInPhases(int noOfPhases)
        {
            for(int i = 0; i <noOfPhases; i++)
            {
                doPhase(i+1);
            }
        }

        public void fightTillDeath()
        {
            while (!enemy.isDead() && !Player.Instance.isDead())
            {
                attackPhaseSimultaneous();
            }
        }

        public void playerAttack()
        {
            string scriptFile;
            if (scripts.TryGetValue("playerAttack", out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
            else {
                enemy.takeDmg(Player.Instance.getDmgDone());
            }
        }

        public void playerDefend()
        {

        }

        public void enemyAttack()
        {
            string scriptFile;
            if (scripts.TryGetValue("enemyAttack", out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
            else {
                Player.Instance.takeDmg(enemy.getDmgDone());
            }
        }

        public void enemyDefend()
        {
            string scriptFile;
            if (scripts.TryGetValue("enemyDefend", out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
            else {
            }
        }


        public void attackPhaseSimultaneous()
        {
            playerAttack();
            enemyAttack();
        }
    }
}
