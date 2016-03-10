using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace TextRPG
{
    public static class GameManager
    {
        public static bool changeCurrentArea(string direction)
        {
            return AreaManager.Instance.changeCurrentArea(direction);
        }

        public static void showAreaDescription()
        {
            GUI.Instance.appendToOutput(AreaManager.Instance.currentArea.description);
        }

        public static void showFullAreaDescription()
        {
            GUI.Instance.appendToOutput(AreaManager.Instance.getFullAreaDescription(AreaManager.Instance.currentArea));
        }

        public static void noAreaInDirection(string direction)
        {
            GUI.Instance.appendToOutput("There is no Area in direction: " + direction);
        }

        public static void showPlayerInventory()
        {
            GUI.Instance.appendToOutput(PlayerInventory.Instance.getListOfitems());
        }

        public static void pickUpItem(string itemName)
        {
            Item i = AreaManager.Instance.getCurrAreaItemFromName(itemName);
            if (i != null)
            {
                AreaManager.Instance.currentArea.removeItem(i);
                PlayerInventory.Instance.addItem(i);
                GUI.Instance.appendToOutput(i.name + " added to your inventory.");
            }
            else
            {
                GUI.Instance.appendToOutput("There is no such item here.");
            }
        }

        public static Character getNpc(int id)
        {
            XElement xElem = PersistenceManager.initXML(Properties.Settings.Default.npcFile, "npcs");
            Character c = null;
            foreach (var npc in xElem.Elements())
            {
                if (id == Int32.Parse(npc.Element("id").Value))
                {
                    if (Int32.Parse(npc.Element("isEnemy").Value) == 0)
                    {
                        c = new Character(npc.Element("name").Value);
                    }
                    else
                    {
                        c = new Enemy(float.Parse(npc.Element("maxHealth").Value), npc.Element("name").Value, float.Parse(npc.Element("dmg").Value));
                    }
                }
            }
            return c;
        }

        public static Item getItem(int id)
        {
            XElement xElem = PersistenceManager.initXML(Properties.Settings.Default.itemFile, "items");
            Item i = null;
            foreach (var item in xElem.Elements())
            {
                if (id == Int32.Parse(item.Element("id").Value))
                {
                    i = new Item(item.Element("name").Value, item.Element("description").Value);
                }
            }
            return i;
        }

        public static void inspect(string s)
        {
            Item i = AreaManager.Instance.getCurrAreaItemFromName(s);
            if(i != null)
            {
                GUI.Instance.appendToOutput(i.description);
            }
        }

        public static void doCombat(string s)
        {
            Enemy e = AreaManager.Instance.getCurrAreaEnemyFromName(s);
            if(e != null)
            {
                CombatManager.Instance.doCombat(e);
                GUI.Instance.appendToOutput("Combat ended with "+ e.name + " - Player Health:" + Player.Instance.currHealth + "; Enemy Health: " + e.currHealth);
            }
        }
    }
}
