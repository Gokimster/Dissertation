using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TextRPG
{
    public class Area
    {
        public Dictionary<AreaConnection, int> connections { get; }
        public List<Character> npcs;
        public enum AreaConnection { N, S, E, W };
        public string description { get; set; }
        public string name { get; set; }
        private Inventory inventory;

        public Area()
        {
            connections = new Dictionary<AreaConnection, int>();
            inventory = new Inventory();
            npcs = new List<Character>();
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
            full += getNpcDescriptionList();
            return full;
        }

        public string getNpcDescriptionList()
        {
            string npcDesc = "Npcs: ";
            string enemiesDesc = "Enemies: ";
            bool containsEnemy = false;
            bool containsNpc = false;
            foreach (Character c in npcs)
            {
                if(c is Enemy)
                {
                    enemiesDesc += c.name + " ";
                    containsEnemy = true;
                }
                else
                {
                    npcDesc += c.name + " ";
                    containsNpc = true;
                }
            }
            if(!containsNpc)
            {
                npcDesc += "None";
            }

            if (!containsEnemy)
            {
                enemiesDesc += "None";
            }
            return npcDesc + "\n" + enemiesDesc + "\n";
        }

        public void addConnection(string connection, int connectingAreaId)
        {
            AreaConnection ac = getConnectionFromString(connection);
            connections.Add(ac, connectingAreaId);
        }

        public void addNpc(int id)
        {
            Character c = GameManager.getNpc(id);
            if (c != null)
            {
                npcs.Add(c);
            }
            else
            {
                Console.WriteLine("Couldn't add Enemy to room");
            }
        }

        public Enemy getEnemyFromName(string name)
        {
            foreach (Character c in npcs)
            {
                if (c.name.Equals(name, StringComparison.InvariantCultureIgnoreCase) && (c is Enemy))
                {
                    return (Enemy)c;
                }
            }
            return null;
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

        public static string getOpposingConnectionString(string s)
        {
            switch (s)
            {
                case "N":
                case "north": return "S";
                case "S":
                case "south": return "N";
                case "E":
                case "east": return "W";
                case "W":
                case "west": return "E";
                default: return "N";
            }
        }
    }
}
