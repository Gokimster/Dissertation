using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Linq;

namespace TextRPG
{
    public class AreaManager
    {
        private Dictionary<int, Area> areas;
        private XElement xElem;
        public Area currentArea { set; get; }
        public static readonly AreaManager Instance = new AreaManager();
        public AreaManager()
        {
        }

        /// <summary>
        /// Initialise the AreaManager
        /// </summary>
        public void init()
        {
            loadAreas();
            currentArea = areas.Values.First();
            GameManager.showFullAreaDescription();
        }

        /// <summary>
        /// Change the current area to the one in a certain direction
        /// </summary>
        /// <param name="direction"></param>
        public bool changeCurrentArea(string direction)
        {
            int areaId = currentArea.getAreaIdAt(direction);
            if(areaId == -1)
            {
                GameManager.noAreaInDirection(direction);
                return false;
            }
            setCurrentArea(areaId);
            return true;
        }

        /// <summary>
        /// Set the current area to the area with a certain id
        /// </summary>
        /// <param name="areaId"></param>
        public void setCurrentArea(int areaId)
        {
            currentArea = areas[areaId];
            GameManager.showFullAreaDescription();
        }

        /// <summary>
        /// Return the full description of an area
        /// </summary>
        /// <param name="a"></param>
        public string getFullAreaDescription(Area a)
        {
            string full = a.getFullDescription();
            full += "Connections: \n";
            foreach(var con in a.connections)
            {
                full += con.Key.ToString() + ": " + areas[con.Value].name + "\n";
            }
            return full.Substring(0,full.LastIndexOf('\n'));
        }

        /// <summary>
        /// Get an item in the current area by name
        /// </summary>
        /// <param name="itemName"></param>
        public Item getCurrAreaItemFromName(string itemName)
        {
            return currentArea.getItemFromName(itemName);
        }

        /// <summary>
        /// Get an enemy in the current area by name
        /// </summary>
        /// <param name="enemyName"></param>
        public Enemy getCurrAreaEnemyFromName(string enemyName)
        {
            return currentArea.getEnemyFromName(enemyName);
        }

        //============================
        //XML Operations
        //============================
        
        private void loadAreas()
        {
            xElem = PersistenceManager.initXML(Properties.Settings.Default.areaFile, "areas");
            areas = new Dictionary<int, Area>();
            foreach (var area in xElem.Elements())
            {
                Area a = new Area();
                if (area.Element("connections") != null)
                {
                    foreach (var connection in area.Element("connections").Elements())
                    {
                        a.addConnection(connection.Element("con").Value, Int32.Parse(connection.Element("area").Value));
                    }
                }

                if (area.Element("npcs") != null)
                {
                    foreach (var npc in area.Element("npcs").Elements())
                    {
                        a.addNpc(Int32.Parse(npc.Element("id").Value));
                    }
                }

                a.description = area.Element("description").Value;
                a.name = area.Element("name").Value;
                a.loadAreaInventory(area);
                areas.Add(Int32.Parse(area.Element("id").Value), a);
            }
        }

        /// <summary>
        /// Add an area
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void addArea(int id, string name, string description)
        {
            Area a = new Area();
            a.name = name;
            a.description = description;
            if (areaSecurityCheck(a))
            {
                try
                {
                    areas.Add(id, a);
                }
                catch (ArgumentException e)
                {
                    Random r = new Random();
                    while (areas.ContainsKey(id))
                    {
                        id = r.Next();
                    }
                    areas.Add(id, a);
                }
                addAreaToXML(id, a);
                GUI.Instance.appendToOutput("Area added with id:" + id);
            }
            else
            {
                GUI.Instance.appendToOutput("Could not add area");
            }
        }

        private void addAreaToXML(int id, Area a)
        {
            xElem.Add(new XElement("area", new XElement("id", id), new XElement("description", a.description), new XElement("name", a.name), new XElement("connections")));
            xElem.Save(Properties.Settings.Default.areaFile);
        }

        /// <summary>
        /// Add a connection from an area to another in a certain direction
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="connection"></param>
        /// <param name="connectingAreaId"></param>
        public void addConnectionToArea(int areaId, string connection, int connectingAreaId)
        {
            var ar = from area in xElem.Elements("area")
                             where (int)area.Element("id") == areaId
                             select area;
            foreach (XElement xel in ar)
            {
                if(xel.Element("connections") == null)
                {
                    xel.Add(new XElement("connections"));
                }
                xel.Element("connections").Add(new XElement("connection",new XElement("con", connection), new XElement("area", connectingAreaId)));
                areas[areaId].addConnection(connection, connectingAreaId);
            }

            ar = from area in xElem.Elements("area")
                        where (int)area.Element("id") == connectingAreaId
                        select area;
            foreach (XElement xel in ar)
            {
                if (xel.Element("connections") == null)
                {
                    xel.Add(new XElement("connections"));
                }
                string opCon = Area.getOpposingConnectionString(connection);
                xel.Element("connections").Add(new XElement("connection", new XElement("con", opCon), new XElement("area", areaId)));
                areas[connectingAreaId].addConnection(opCon, areaId);
            }

            GUI.Instance.appendToOutput("Area connection from area - "+ areaId+" to area - "+ connectingAreaId);
            xElem.Save(Properties.Settings.Default.areaFile);
        }

        /// <summary>
        /// Add an item to an area
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="itemId"></param>
        public void addItemToArea(int areaId, int itemId)
        {
            var ar = from area in xElem.Elements("area")
                     where (int)area.Element("id") == areaId
                     select area;
            foreach (XElement xel in ar)
            {
                if (areas[areaId].addItem(itemId))
                {
                    if (xel.Element("items") == null)
                    {
                        xel.Add(new XElement("items"));
                    }
                    xel.Element("items").Add(new XElement("item", new XElement("id", itemId)));
                    xElem.Save(Properties.Settings.Default.areaFile);
                    GUI.Instance.appendToOutput("Item with id - " + itemId + " added to area - " + areaId);
                }
                else
                {
                    GUI.Instance.appendToOutput("Couldn't add item to area");
                }
            }
        }

        /// <summary>
        /// Add an NPC to an area
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="npcId"></param>
        public void addNpcToArea(int areaId, int npcId)
        {
            var ar = from area in xElem.Elements("area")
                     where (int)area.Element("id") == areaId
                     select area;
            foreach (XElement xel in ar)
            {
                if (areas[areaId].addNpc(npcId))
                {
                    if (xel.Element("npcs") == null)
                    {
                        xel.Add(new XElement("npcs"));
                    }
                    xel.Element("npcs").Add(new XElement("npc", new XElement("id", npcId)));
                    xElem.Save(Properties.Settings.Default.areaFile);
                    GUI.Instance.appendToOutput("NPC with id - " + npcId + " added to area - " + areaId);
                }
                else
                {
                    GUI.Instance.appendToOutput("Couldn't add npc to area");
                }
                
            }
        }

        /// <summary>
        /// Set the name of an area
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void setAreaName(int id, string name)
        {
            setAreaProperty(id, "name", name);
        }

        /// <summary>
        /// Set the descirpition of an area
        /// </summary>
        /// <param name="id"></param>
        /// <param name="desc"></param>
        public void setAreaDescription(int id, string desc)
        {
            setAreaProperty(id, "description", desc);
        }

        /// <summary>
        /// Set a property of an area
        /// </summary>
        /// <param name="id"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void setAreaProperty(int id, string property, object value)
        {
            Area a;
            if (areas.TryGetValue(id, out a))
            {
                Area temp = a;
                temp[property] = value;
                if (areaSecurityCheck(temp))
                {
                    var ar = from area in xElem.Elements("area")
                             where (int)area.Element("id") == id
                             select area;
                    foreach (XElement xel in ar)
                    {
                        xel.Element(property).SetValue(value);
                    }
                    a[property] = value;
                    xElem.Save(Properties.Settings.Default.areaFile);
                    GUI.Instance.appendToOutput("Area " + property + " changed to " + value.ToString());
                }
                else
                {
                    GUI.Instance.appendToOutput("Could not change the area " + property);
                }
            }
            else
            {
                GUI.Instance.appendToOutput("Could not change the area "+ property);
            }
        }

        private bool areaSecurityCheck(Area i)
        {
            //TO-DO: add security checks for area properties
            return true;
        }
    }
}
