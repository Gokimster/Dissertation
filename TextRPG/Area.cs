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

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

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

        /// <summary>
        /// Load area inventory
        /// </summary>
        /// <param name="elem"></param>
        public void loadAreaInventory(XElement elem)
        {
            inventory.loadInventory(elem);
        }

        /// <summary>
        /// Return the full description of this area
        /// </summary>
        public string getFullDescription()
        {
            string full = name + "\n";
            full += description + "\n";
            full += "Items: " + inventory.getListOfItems() + "\n";
            full += getNpcDescriptionList();
            return full;
        }

        /// <summary>
        /// Return a string list of all NPCs in the area
        /// </summary>
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

        /// <summary>
        /// Add a connection to an area by id
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="connectingAreaId"></param>
        public void addConnection(string connection, int connectingAreaId)
        {
            AreaConnection ac = getConnectionFromString(connection);
            connections.Add(ac, connectingAreaId);
        }

        /// <summary>
        /// Add an NPC to the area
        /// </summary>
        /// <param name="id"></param>
        public bool addNpc(int id)
        {
            Character c = GameManager.getNpc(id);
            if (c != null)
            {
                npcs.Add(c);
            }
            else
            {
                Console.WriteLine("Couldn't add Enemy to room");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Add an item to the area by id
        /// </summary>
        /// <param name="id"></param>
        public bool addItem(int id)
        {
            return inventory.addItemToInventory(id);
        }

        /// <summary>
        /// Get an enemy in the area by name
        /// </summary>
        /// <param name="name"></param>
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

        /// <summary>
        /// Returns the id of the area connecnted to this area througha  given connection as a string
        /// </summary>
        /// <param name="connection"></param>
        public int getAreaIdAt(string connection)
        {
            AreaConnection con = getConnectionFromString(connection);
            if (connections.ContainsKey(con))
            {
                return connections[con];
            }
            return -1;
        }

        /// <summary>
        /// Returns the id of the area connecnted to this area througha  given connection as an AreaConnection 
        /// </summary>
        /// <param name="connection"></param>
        public int getAreaIdAt(AreaConnection connection)
        {
            if (connections.ContainsKey(connection))
            {
                return connections[connection];
            }
            return -1;
        }

        /// <summary>
        /// Return item in area by name
        /// </summary>
        /// <param name="itemName"></param>
        public Item getItemFromName(string itemName)
        {
            return inventory.getItemFromName(itemName);
        }

        /// <summary>
        /// Remove item from inventory
        /// </summary>
        /// <param name="i"></param>
        public void removeItem(Item i)
        {
            inventory.removeItem(i);
        }

        /// <summary>
        /// Return AreaConnection corresponding to a string
        /// </summary>
        /// <param name="s"></param>
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

        /// <summary>
        /// Return the opposing area connection to a connection by string
        /// </summary>
        /// <param name="s"></param>
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
