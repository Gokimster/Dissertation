using System.Collections.Generic;

namespace TextRPG
{
    public class PlayerInventory: Inventory
    {
        public static readonly PlayerInventory Instance = new PlayerInventory();
        private List<Item> equippedItems;

        public PlayerInventory():base()
        {
            equippedItems = new List<Item>();
            loadInventory();
        }

        /// <summary>
        /// Load player inventory from XML
        /// </summary>
        private void loadInventory()
        {
            xElem = PersistenceManager.initXML(Properties.Settings.Default.playerFile, "player");
            base.loadInventory(xElem.Element("player"));
        }

        /// <summary>
        /// Equip an item by name
        /// </summary>
        /// <param name="name"></param>
        public bool equipItem(string name)
        {
            Item i = getItemFromName(name);
            if(i != null && (i is EquipItem))
            {
                equippedItems.Add(i);
                items.Remove(i);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get an item that is equipped by name
        /// </summary>
        /// <param name="name"></param>
        public Item getEquippedItemByName(string name)
        {
            return getItemFromNameInList(name, equippedItems);
        }

        /// <summary>
        /// Unequip an item by name
        /// </summary>
        /// <param name="name"></param>
        public bool unequipItem(string name)
        {
            Item i = getEquippedItemByName(name);
            if(i != null)
            {
                items.Add(i);
                equippedItems.Remove(i);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get the damage bonus provided by all equipped items
        /// </summary>
        public float getEquippedDmgBonus()
        {
            float bonus = 0;
            foreach(Item item in equippedItems)
            {
                bonus += ((EquipItem)item).dmgBonus;
            }
            return bonus;
        }

        /// <summary>
        /// Get a list of all items in inventory
        /// </summary>
        public string getListOfItems()
        {
            string list = "";
            if(items.Count > 0 )
            {
                list += "Inventory: ";
            }
            list += base.getListOfItems();
            if (equippedItems.Count <= 0)
            {
                list += "\nNo Equipped Items";
            }
            else
            {
                list += "\nEquipped Items: " + getListOfItemsIn(equippedItems);
            }
            return list;
        }

    }
}
