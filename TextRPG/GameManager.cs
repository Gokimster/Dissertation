using System;
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
            XElement xElem = PersistenceMgr.initXML(Properties.Settings.Default.npcFile, "npcs");
            Character c = null;
            foreach (var npc in xElem.Elements())
            {
                if (id == Int32.Parse(npc.Element("id").Value))
                {
                    if (Int32.Parse(npc.Element("isEnemy").Value) == 0)
                        c = new Character(float.Parse(npc.Element("id").Value), npc.Element("name").Value);
                    else
                    {
                        c = new Enemy(float.Parse(npc.Element("id").Value), npc.Element("name").Value);
                    }
                }
            }
            return c;
        }
    }
}
