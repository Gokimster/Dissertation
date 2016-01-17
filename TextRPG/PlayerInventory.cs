using System.Collections.Generic;

namespace TextRPG
{
    public class PlayerInventory: Inventory
    {
        public static readonly PlayerInventory Instance = new PlayerInventory();

        public PlayerInventory():base()
        {
            loadInventory();
        }

        private void loadInventory()
        {
            xElem = PersistenceMgr.initXML(Properties.Settings.Default.playerFile, "player");
            base.loadInventory(xElem.Element("player"));
        }
    }
}
