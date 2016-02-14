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
                while(areas.ContainsKey(id))
                {
                    Random r = new Random();
                    id = r.Next();
                }
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
                xel.Element("connections").Add(new XElement("connection",new XElement("con", connection), new XElement("area", connectingAreaId)));
                areas[areaId].addConnection(connection, connectingAreaId);
            }

            ar = from area in xElem.Elements("area")
                        where (int)area.Element("id") == connectingAreaId
                        select area;
            foreach (XElement xel in ar)
            {
                string opCon = Area.getOpposingConnectionString(connection);
                xel.Element("connections").Add(new XElement("connection", new XElement("con", opCon), new XElement("area", areaId)));
                areas[connectingAreaId].addConnection(opCon, areaId);
            }

            
            xElem.Save(Properties.Settings.Default.areaFile);
        }
    }
}
