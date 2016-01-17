using System.Collections.Generic;

namespace TextRPG
{
    public class Area
    {
        public Dictionary<AreaConnection, int> connections { get; }
        public enum AreaConnection { N, S, E, W };
        public string description { get; set; }
        public string name { get; set; }

        public Area()
        {
            connections = new Dictionary<AreaConnection, int>();
        }

        public Area(string description)
        {
            this.description = description;
        }

        public string getFullDescription()
        {
            string full = name + "\n";
            full += description + "\n";
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

        private AreaConnection getConnectionFromString(string s)
        {
            switch(s)
            {
                case "N":
                case "north":  return AreaConnection.N;
                case "S":
                case "south":  return AreaConnection.S;
                case "E": return AreaConnection.E;
                case "W": return AreaConnection.W;
                default: return AreaConnection.N;
            }
        }
    }
}
