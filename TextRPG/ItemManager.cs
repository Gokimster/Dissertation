using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

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

        
        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        public Item getItem(int id)
        {
            //TO DO: check if item id exists
            return items[id];
        }
        
        /// <summary>
        /// Load all items from xml
        /// </summary>
        private void loadItems()
        {
            xElem = PersistenceManager.initXML(Properties.Settings.Default.itemFile, "items");
            items = new Dictionary<int, Item>();
            foreach (var item in xElem.Elements())
            {
                Item i;
                if (item.Element("isEquippable")!= null && Int32.Parse(item.Element("isEquippable").Value) == 1)
                {
                    i = new EquipItem();
                    if (item.Element("dmgBonus") != null)
                    {
                        ((EquipItem)i).dmgBonus = Int32.Parse(item.Element("dmgBonus").Value);
                    }
                }
                else {
                    i = new Item();
                }
                i.description = item.Element("description").Value;
                i.name = item.Element("name").Value;
                items.Add(Int32.Parse(item.Element("id").Value), i);
            }
        }
        
        /// <summary>
        /// Add an item both locally and to xml
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void addItem(int id, string name, string description)
        {
            Item i = new Item(name, description);
            if (itemSecurityCheck(i))
            {
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
            else
            {
                GUI.Instance.appendToOutput("Could not add item");
            }
        }
        
        /// <summary>
        /// Set the name of an existing item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void setItemName(int id, string name)
        {
            setItemProperty(id, "name", name);
        }
        
        /// <summary>
        /// Set the description of an existing item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="desc"></param>
        public void setItemDescription(int id, string desc)
        {
            setItemProperty(id, "description", desc);
        }
        
        /// <summary>
        /// Set a property of an existing item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void setItemProperty(int id, string property, object value)
        {
            Item i;
            if (items.TryGetValue(id, out i))
            {
                Item temp = i;
                temp[property] = value;
                if (itemSecurityCheck(temp))
                {
                    var it = from item in xElem.Elements("item")
                             where (int)item.Element("id") == id
                             select item;
                    foreach (XElement xel in it)
                    {
                        xel.Element(property).SetValue(value);
                    }
                    i[property] = value;
                    xElem.Save(Properties.Settings.Default.itemFile);
                    GUI.Instance.appendToOutput("Area " + property + " changed to " + value.ToString());
                }
                else
                {
                    GUI.Instance.appendToOutput("Could not change the item " + property + " to: " + value.ToString());
                }
            }
            else
            {
                GUI.Instance.appendToOutput("Could not change the item " + property);
            }
        }
        
        /// <summary>
        /// Add an item to xml
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        private void addItemToXML(int id, Item item)
        {
            xElem.Add(new XElement("item", new XElement("id", id), new XElement("isEquippable", 0), new XElement("name", item.name), new XElement("description", item.description)));
            xElem.Save(Properties.Settings.Default.itemFile);
        }
        
        /// <summary>
        /// Security check for an item
        /// </summary>
        /// <param name="item"></param>
        private bool itemSecurityCheck(Item item)
        {
            //TO-DO: add security checks for item properties
            return true;
        }


    }
}
