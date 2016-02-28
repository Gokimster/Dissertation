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
            loadAreas();
            currentArea = areas.Values.First();
        }

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

        public void setCurrentArea(int areaId)
        {
            currentArea = areas[areaId];
            GameManager.showFullAreaDescription();
        }

        public string getFullAreaDescription(Area a)
        {
            string full = a.getFullDescription();
            full += "Connections: \n";
            foreach(var con in a.connections)
            {
                full += con.Key.ToString() + ": " + areas[con.Value].name + "\n";
            }
            return full;
        }

        public Item getCurrAreaItemFromName(string itemName)
        {
            return currentArea.getItemFromName(itemName);
        }

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

        public void addArea(int id, string name, string description)
        {
            Area a = new Area();
            a.name = name;
            a.description = description;
            try {
                areas.Add(id, a);
            }
            catch(ArgumentException e)
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

        private void addAreaToXML(int id, Area a)
        {
            xElem.Add(new XElement("area", new XElement("id", id), new XElement("description", a.description), new XElement("name", a.name), new XElement("connections")));
            xElem.Save(Properties.Settings.Default.areaFile);
        }

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
        
    }
}
