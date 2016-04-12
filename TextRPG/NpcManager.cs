using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace TextRPG
{
    public class NpcManager
    {
        private Dictionary<int, Character> npcs;
        public static readonly NpcManager Instance = new NpcManager();
        private XElement xElem;

        public NpcManager()
        {
            loadNpcs();
        }

        /// <summary>
        /// Loads npc list from XML
        /// </summary>
        private void loadNpcs()
        {
            xElem = PersistenceManager.initXML(Properties.Settings.Default.npcFile, "npcs");
            npcs = new Dictionary<int, Character>();
            foreach (var npc in xElem.Elements())
            {
                Character c;
                if (Int32.Parse(npc.Element("isEnemy").Value) == 0)
                {
                    c = new Character();
                }
                else
                {
                    c = new Enemy();
                    ((Enemy)c).maxHealth = float.Parse(npc.Element("maxHealth").Value);
                    ((Enemy)c).dmg = float.Parse(npc.Element("dmg").Value);
                }
                c.name = npc.Element("name").Value;
                npcs.Add(Int32.Parse(npc.Element("id").Value), c);
            }
        }

        /// <summary>
        /// Add a Character both locally and to XML
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void addNpc(int id, string name)
        {
            Character c = new Character(name);
            if (npcSecurityCheck(c))
            {
                try
                {
                    npcs.Add(id, c);
                }
                catch (ArgumentException e)
                {
                    Random r = new Random();
                    while (npcs.ContainsKey(id))
                    {
                        id = r.Next();
                    }
                    npcs.Add(id, c);
                }
                addNpcToXML(id, c);
                GUI.Instance.appendToOutput("NPC` added with id:" + id);
            }
            else
            {
                GUI.Instance.appendToOutput("Could not add Npc");
            }
        }

        /// <summary>
        /// Add an Character to XML
        /// </summary>
        /// <param name="c"></param>
        private void addNpcToXML(int id, Character c)
        {
            xElem.Add(new XElement("npc", new XElement("id", id), new XElement("name", c.name), new XElement("isEnemy", 0)));
            xElem.Save(Properties.Settings.Default.npcFile);
        }

        /// <summary>
        /// Add an enemy locally and to xml
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="maxHealth"></param>
        /// <param name="dmg"></param>
        public void addEnemy(int id, string name, float maxHealth, float dmg)
        {
            Enemy c = new Enemy(maxHealth, name, dmg);
            if (npcSecurityCheck(c))
            {
                try
                {
                    npcs.Add(id, c);
                }
                catch (ArgumentException e)
                {
                    Random r = new Random();
                    while (npcs.ContainsKey(id))
                    {
                        id = r.Next();
                    }
                    npcs.Add(id, c);
                }
                addEnemyToXML(id, c);
                GUI.Instance.appendToOutput("Enemy added with id:" + id);
            }
            else
            {
                GUI.Instance.appendToOutput("Could not add enemy");
            }
        }

        /// <summary>
        /// Adds an enemy to the XML file
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enemy"></param>
        private void addEnemyToXML(int id, Enemy enemy)
        {
            xElem.Add(new XElement("npc", new XElement("id", id), new XElement("name", enemy.name), new XElement("maxHealth", enemy.maxHealth), new XElement("dmg", enemy.dmg), new XElement("isEnemy", 1)));
            xElem.Save(Properties.Settings.Default.npcFile);
        }

        /// <summary>
        /// Return an npc by id
        /// </summary>
        /// <param name="id"></param>
        public Character getNpc(int id)
        {
            //TO DO: check if item id exists
            return npcs[id];
        }

        /// <summary>
        /// Return an Enemy by id
        /// </summary>
        /// <param name="id"></param>
        public Enemy getEnemy(int id)
        {
            if (npcs[id] is Enemy)
            {
                return (Enemy)npcs[id];
            }
            return null;
        }

        /// <summary>
        /// Set the name of an existing npc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void setNpcName(int id, string name)
        {
            setNpcProperty(id, "name", name);
        }

        /// <summary>
        /// Set the maximum health of an existing npc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hp"></param>
        public void setNpcMaxHealth(int id, int hp)
        {
            setNpcProperty(id, "maxHealth", hp);
        }

        /// <summary>
        /// Set the damage of an existing npc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dmg"></param>
        public void setNpcDmg(int id, int dmg)
        {
            setNpcProperty(id, "dmg", dmg);
        }

        /// <summary>
        /// Set a property for an existing npc
        /// </summary>
        /// <param name="id"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void setNpcProperty(int id, string property, object value)
        {
            Character c;
            if (npcs.TryGetValue(id, out c))
            {
                Character temp = c;
                temp[property] = value;
                if (npcSecurityCheck(temp))
                {
                    var ch = from npc in xElem.Elements("npc")
                             where (int)npc.Element("id") == id
                             select npc;
                    foreach (XElement xel in ch)
                    {
                        xel.Element(property).SetValue(value);
                    }
                    c[property] = value;
                    xElem.Save(Properties.Settings.Default.npcFile);
                    GUI.Instance.appendToOutput("Npc " + property + " changed to " + value.ToString());
                }
                else
                {
                    GUI.Instance.appendToOutput("Could not change the npc " + property);
                }
            }
            else
            {
                GUI.Instance.appendToOutput("Could not change the npc " + property);
            }
        }

        /// <summary>
        /// Security check for a character
        /// </summary>
        /// <param name="character"></param>
        private bool npcSecurityCheck(Character character)
        {
            //TO-DO: add security checks for npc properties
            return true;
        }
    }
}
