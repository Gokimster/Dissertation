using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace TextRPG
{
    public static class GameManager
    {
        /// <summary>
        /// Change the current area to the one connecting to it by a given string connection
        /// </summary>
        /// <param name="direction"></param>
        public static bool changeCurrentArea(string direction)
        {
            return AreaManager.Instance.changeCurrentArea(direction);
        }

        /// <summary>
        /// Show the area description of the current area
        /// </summary>
        public static void showAreaDescription()
        {
            GUI.Instance.appendToOutput(AreaManager.Instance.currentArea.description);
        }

        /// <summary>
        /// Show the full area description of the current area
        /// </summary>
        public static void showFullAreaDescription()
        {
            GUI.Instance.appendToOutput(AreaManager.Instance.getFullAreaDescription(AreaManager.Instance.currentArea));
        }

        /// <summary>
        /// Output if there is no area in a direction
        /// </summary>
        /// <param name="direction"></param>
        public static void noAreaInDirection(string direction)
        {
            GUI.Instance.appendToOutput("There is no Area in direction: " + direction);
        }

        /// <summary>
        /// Show player inventory
        /// </summary>
        public static void showPlayerInventory()
        {
            GUI.Instance.appendToOutput(PlayerInventory.Instance.getListOfItems());
        }

        /// <summary>
        /// Pick up an item by name
        /// </summary>
        /// <param name="itemName"></param>
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

        /// <summary>
        /// Equip an item by name
        /// </summary>
        /// <param name="itemName"></param>
        public static void equipItem(string itemName)
        {
            if(PlayerInventory.Instance.equipItem(itemName))
            {
                GUI.Instance.appendToOutput("Equipped " + itemName);
            }
            else
            {
                GUI.Instance.appendToOutput("Could not equip " + itemName);
            }
        }

        /// <summary>
        /// Unequip an item by name
        /// </summary>
        /// <param name="itemName"></param>
        public static void unequipItem(string itemName)
        {
            if (PlayerInventory.Instance.unequipItem(itemName))
            {
                GUI.Instance.appendToOutput("Unequipped " + itemName);
            }
            else
            {
                GUI.Instance.appendToOutput("Could not unequip " + itemName);
            }
        }

        /// <summary>
        /// Return an NPC from XML by ID
        /// </summary>
        /// <param name="id"></param>
        public static Character getNpc(int id)
        {
            XElement xElem = PersistenceManager.initXML(Properties.Settings.Default.npcFile, "npcs");
            Character c = null;
            foreach (var npc in xElem.Elements())
            {
                if (id == Int32.Parse(npc.Element("id").Value))
                {
                    if (npc.Element("isEnemy") == null || (npc.Element("isEnemy") == null && Int32.Parse(npc.Element("isEnemy").Value) == 0))
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

        /// <summary>
        /// Return an item from XML by id
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Inspect an item by name - show its description
        /// </summary>
        /// <param name="s"></param>
        public static void inspect(string s)
        {
            Item i = AreaManager.Instance.getCurrAreaItemFromName(s);
            if(i != null)
            {
                GUI.Instance.appendToOutput(i.description);
            }
            else
            {
                i = PlayerInventory.Instance.getItemFromName(s);
                if (i != null)
                {
                    GUI.Instance.appendToOutput(i.description);
                }
                else
                {
                    i = PlayerInventory.Instance.getEquippedItemByName(s);
                    if (i != null)
                    {
                        GUI.Instance.appendToOutput(i.description);
                    }
                }
            }
        }

        /// <summary>
        /// Do combat with an enemy by name
        /// </summary>
        /// <param name="s"></param>
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
