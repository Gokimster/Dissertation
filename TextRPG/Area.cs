using System.Collections.Generic;

namespace TextRPG
{
    public class Area
    {
        Dictionary<AreaConnection, int> connections;
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

        private AreaConnection getConnectionFromString(string s)
        {
            switch(s)
            {
                case "N": return AreaConnection.N;
                case "S": return AreaConnection.S;
                case "E": return AreaConnection.E;
                case "W": return AreaConnection.W;
                default: return AreaConnection.N;
            }
        }
    }
}
