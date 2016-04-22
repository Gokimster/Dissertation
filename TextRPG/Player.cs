using System.Linq;
using System.Xml.Linq;

namespace TextRPG
{
    public class Player:CombatCharacter
    {
        public static readonly Player Instance = new Player();

        public Player()
        {
            loadCharacter();
        }

        /// <summary>
        /// Load player data from XML
        /// </summary>
        private void loadCharacter()
        {
            XElement xElem = PersistenceManager.initXML(Properties.Settings.Default.playerFile, "players");
            var player = xElem.Elements().First();
            maxHealth = float.Parse(player.Element("maxHealth").Value);
            currHealth = maxHealth;
            dmg = float.Parse(player.Element("dmg").Value);
            name = player.Element("name").Value;
        }

        /// <summary>
        /// Get the damage that the player does
        /// </summary>
        new public float getDmgDone()
        {
            return base.getDmgDone() + PlayerInventory.Instance.getEquippedDmgBonus();
        }
    }
}
