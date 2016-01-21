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

        public void addItem(Item i)
        {
            items.Add(i);
            addItemToXML(i);
        }

        private void addItemToXML(Item i)
        {
            xElem.Add(new XElement("item", new XElement("name", i.name), new XElement("description", i.description)));
        }

        public void removeItem(Item i)
        {
            items.Remove(i);
            removeItemFromXML(i);
        }

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
