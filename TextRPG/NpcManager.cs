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

        public void addNpc(int id, string name)
        {
            Character c = new Character(name);
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

        private void addNpcToXML(int id, Character c)
        {
            xElem.Add(new XElement("npc", new XElement("id", id), new XElement("name", c.name), new XElement("isEnemy", 0)));
            xElem.Save(Properties.Settings.Default.npcFile);
        }

        public void addEnemy(int id, string name, float maxHealth, float dmg)
        {
            Enemy c = new Enemy(maxHealth, name, dmg);
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

        private void addEnemyToXML(int id, Enemy e)
        {
            xElem.Add(new XElement("npc", new XElement("id", id), new XElement("name", e.name), new XElement("maxHealth", e.maxHealth), new XElement("dmg", e.dmg), new XElement("isEnemy", 1)));
            xElem.Save(Properties.Settings.Default.npcFile);
        }

        public Character getNpc(int id)
        {
            //TO DO: check if item id exists
            return npcs[id];
        }

        public Enemy getEnemy(int id)
        {
            if (npcs[id] is Enemy)
            {
                return (Enemy)npcs[id];
            }
            return null;
        }

        public void setNpcName(int id, string name)
        {
            setNpcProperty(id, "name", name);
        }

        public void setNpcMaxHealth(int id, int hp)
        {
            setNpcProperty(id, "maxHealth", hp);
        }

        public void setNpcDmg(int id, int dmg)
        {
            setNpcProperty(id, "dmg", dmg);
        }

        public void setNpcProperty(int id, string property, object value)
        {
            Character c;
            if (npcs.TryGetValue(id, out c))
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
    }
}
