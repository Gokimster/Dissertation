using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TextRPG
{
    public class Inventory
    {
        protected List<Item> items;
        protected XElement xElem;

        public Inventory()
        {
            items = new List<Item>();
        }
        
        /// <summary>
        /// Loads  inventory from XElement
        /// </summary>
        /// <param name="elem"></param>
        public void loadInventory(XElement elem)
        {
            if (elem.Element("items") != null)
            {
                foreach (var item in elem.Element("items").Elements())
                {
                    items.Add(ItemManager.Instance.getItem(Int32.Parse(item.Element("id").Value)));
                }
            }
        }

        /// <summary>
        /// Add an item to the inventory both locally and to xml
        /// </summary>
        /// <param name="item"></param>
        public void addItem(Item item)
        {
            items.Add(item);
            addItemToXML(item);
        }
        
        /// <summary>
        /// Add an item by id to the local inventory and to xml
        /// </summary>
        /// <param name="id"></param>
        public bool addItemToInventory(int id)
        {
            Item i = GameManager.getItem(id);
            if (i != null)
            {
                items.Add(i);
            }
            else
            {
                Console.WriteLine("Couldn't add item to inventory");
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// add an item to inventory in XML
        /// </summary>
        /// <param name="item"></param>
        private void addItemToXML(Item item)
        {
            xElem.Add(new XElement("item", new XElement("name", item.name), new XElement("description", item.description)));
        }

        public void removeItem(Item i)
        {
            items.Remove(i);
            removeItemFromXML(i);
        }

        /// <summary>
        /// Get item from inventory by name
        /// </summary>
        /// <param name="itemName"></param>
        public Item getItemFromName(string itemName)
        {
            foreach(Item i in items)
            {
                if (i.name.Equals(itemName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }
            return null;
        }

        //If you remove item from XML in areas then you will not be able to restore the items in rooms
        //think of having a bool that says if the item has been picked up, then you can reset it when reseting game, or on new game
        private void removeItemFromXML(Item i)
        {

        }
        
        /// <summary>
        /// Get a string representation of list of items in inventory 
        /// </summary>
        public string getListOfitems()
        {
            string listOfItems = "";
            if(items.Count <= 0)
            {
                return "Inventory is Empty";
            }
            foreach (Item i in items)
            {
                if (listOfItems == "")
                {
                    listOfItems = listOfItems + i.name;
                }
                else
                {
                    listOfItems = listOfItems + ", " + i.name;
                }
            }
            return listOfItems;
        }

    }
}
