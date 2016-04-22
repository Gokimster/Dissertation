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

        /// <summary>
        /// Do a certain combat phase
        /// </summary>
        /// <param name="phase"></param>
        public void doPhase(int phase)
        {
            string scriptFile;
            if (scripts.TryGetValue("phase" + phase, out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
        }

        /// <summary>
        /// Remove combat script associated with a function
        /// </summary>
        /// <param name="function"></param>
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

        /// <summary>
        /// Associate a combat script to a combat function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="script"></param>
        public void addScriptToFunction(string function, string script)
        {
            removeScriptForFunction(function);
            scripts.Add(function, script);
            xElem.Add(new XElement("script", new XElement("function", function), new XElement("script", script)));
            xElem.Save(Properties.Settings.Default.combatScriptsFile);
        }

        /// <summary>
        /// Do the combat behaviour with an enemy
        /// </summary>
        /// <param name="e"></param>
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

        /// <summary>
        /// Do combat in phases
        /// </summary>
        /// <param name="noOfPhases"></param>
        public void fightInPhases(int noOfPhases)
        {
            for(int i = 0; i <noOfPhases; i++)
            {
                doPhase(i+1);
            }
        }

        /// <summary>
        /// Fight till death behaviour with simultanous attack phases
        /// </summary>
        public void fightTillDeath()
        {
            while (!enemy.isDead() && !Player.Instance.isDead())
            {
                attackPhaseSimultaneous();
            }
        }

        /// <summary>
        /// Do player attack behaviour
        /// </summary>
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

        /// <summary>
        /// Do player defend behaviour
        /// </summary>
        public void playerDefend()
        {
            string scriptFile;
            if (scripts.TryGetValue("playerDefend", out scriptFile))
            {
                LuaManager.executeScript(scriptFile);
            }
            else {
            }
        }

        /// <summary>
        /// do enemy attack behaviour
        /// </summary>
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

        /// <summary>
        /// Do enemy defend behaviour
        /// </summary>
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

        /// <summary>
        /// Simultaneous attack by enemy and player
        /// </summary>
        public void attackPhaseSimultaneous()
        {
            playerAttack();
            enemyAttack();
        }
    }
}
