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
        private Area currentArea;
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
                return false;
            }
            currentArea = areas[areaId];
            return true;
        }

        private void loadAreas()
        {
            xElem = PersistenceMgr.initXML(Properties.Settings.Default.areaFile, "areas");
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
                a.description = area.Element("description").Value;
                a.name = area.Element("name").Value;
                areas.Add(Int32.Parse(area.Element("id").Value), a);
            }
        }

        public void addArea(int id, string description)
        {
            Area a = new Area();
            a.description = description;
            areas.Add(id, a);
        }

        private void addAreaToXML(int id, Area a)
        {
            xElem.Add(new XElement("area", new XElement("id", id), new XElement("description", a.description), new XElement("name", a.name), new XElement("connections")));
        }

        private void addConnectionToAreaInXML(int areaId, string connection, int connectingAreaId)
        {
            var areas = from area in xElem.Elements("area")
                             where (int)area.Element("Id") == areaId
                             select area;
            foreach (XElement xel in areas)
            {
                xel.Element("connections").Add(new XElement("connection",new XElement("con", connection), new XElement("area", connectingAreaId)));
            }
        }
    }
}
