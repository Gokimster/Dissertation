using System.Collections.Generic;
using System.Xml.Linq;

namespace TextRPG
{
    public class Area
    {
        public Dictionary<AreaConnection, int> connections { get; }
        public enum AreaConnection { N, S, E, W };
        public string description { get; set; }
        public string name { get; set; }
        private Inventory inventory;

        public Area()
        {
            connections = new Dictionary<AreaConnection, int>();
            inventory = new Inventory();
        }

        public Area(string description)
        {
            this.description = description;
        }

        public void loadAreaInventory(XElement elem)
        {
            inventory.loadInventory(elem);
        }

        public string getFullDescription()
        {
            string full = name + "\n";
            full += description + "\n";
            full += "Items: " + inventory.getListOfitems() + "\n";
            //TO DO: Add Items
            return full;
        }

        public void addConnection(string connection, int connectingAreaId)
        {
            AreaConnection ac = getConnectionFromString(connection);
            connections.Add(ac, connectingAreaId);
        }

        public int getAreaIdAt(string connection)
        {
            AreaConnection con = getConnectionFromString(connection);
            if (connections.ContainsKey(con))
            {
                return connections[con];
            }
            return -1;
        }

        public int getAreaIdAt(AreaConnection connection)
        {
            if (connections.ContainsKey(connection))
            {
                return connections[connection];
            }
            return -1;
        }

        public Item getItemFromName(string itemName)
        {
            return inventory.getItemFromName(itemName);
        }

        public void removeItem(Item i)
        {
            inventory.removeItem(i);
        }

        private AreaConnection getConnectionFromString(string s)
        {
            switch(s)
            {
                case "N":
                case "north":   return AreaConnection.N;
                case "S":
                case "south":   return AreaConnection.S;
                case "E":
                case "east":    return AreaConnection.E;
                case "W":
                case "west":    return AreaConnection.W;
                default: return AreaConnection.N;
            }
        }
    }
}
