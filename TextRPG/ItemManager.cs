using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TextRPG
{
    public class ItemManager
    {
        private Dictionary<int, Item> items;
        public static readonly ItemManager Instance = new ItemManager();
        private XElement xElem;

        public ItemManager()
        {
            loadItems();
        }

        private void loadItems()
        {
            xElem = PersistenceManager.initXML(Properties.Settings.Default.itemFile, "items");
            items = new Dictionary<int, Item>();
            foreach (var item in xElem.Elements())
            {
                Item i = new Item();
                i.description = item.Element("description").Value;
                i.name = item.Element("name").Value;
                items.Add(Int32.Parse(item.Element("id").Value), i);
            }
        }

        public Item getItem(int id)
        {
            //TO DO: check if item id exists
            return items[id];
        }
    }
}
