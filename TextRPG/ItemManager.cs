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

        public void addItem(int id, string name, string description)
        {
            Item i = new Item(name, description);
            try
            {
                items.Add(id, i);
            }
            catch (ArgumentException e)
            {
                Random r = new Random();
                while (items.ContainsKey(id))
                {
                    id = r.Next();
                }
                items.Add(id, i);
            }
            addItemToXML(id, i);
            GUI.Instance.appendToOutput("Item added with id:" + id);
        }
        
        private void addItemToXML(int id, Item i)
        {
            xElem.Add(new XElement("item", new XElement("id", id), new XElement("description", i.description), new XElement("name", i.name)));
            xElem.Save(Properties.Settings.Default.itemFile);
        }

        public Item getItem(int id)
        {
            //TO DO: check if item id exists
            return items[id];
        }
    }
}
