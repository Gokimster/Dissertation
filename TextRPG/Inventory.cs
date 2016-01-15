using System.Collections.Generic;
using System.Xml.Linq;

namespace TextRPG
{
    public class Inventory
    {
        private List<Item> items;
        private XElement xElem;

        public Inventory()
        { 
        }

        private void loadInventory()
        {
            xElem = PersistenceMgr.initXML(Properties.Settings.Default.inventoryFile, "inventory");
            items = new List<Item>();
            foreach (var item in xElem.Elements())
            {
                Item i = new Item(item.Element("name").Value, item.Element("description").Value);
                items.Add(i);
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

        private void removeItemFromXML(Item i)
        {

        }

        public string showListOfitems()
        {
            string listOfItems = "";
            if(items.Count <= 0)
            {
                return "Inventory is Empty";
            }
            foreach (Item i in items)
            {
                if(listOfItems == "")
                {
                    listOfItems = listOfItems + i.name;
                }
                listOfItems = listOfItems +", " +  i.name;
            }
            return listOfItems;
        }

    }
}
