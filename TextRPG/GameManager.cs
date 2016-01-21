namespace TextRPG
{
    public static class GameManager
    {
        public static bool changeCurrentArea(string direction)
        {
            return AreaManager.Instance.changeCurrentArea(direction);
        }

        public static void showAreaDescription()
        {
            GUI.Instance.appendToOutput(AreaManager.Instance.currentArea.description);
        }

        public static void showFullAreaDescription()
        {
            GUI.Instance.appendToOutput(AreaManager.Instance.getFullAreaDescription(AreaManager.Instance.currentArea));
        }

        public static void noAreaInDirection(string direction)
        {
            GUI.Instance.appendToOutput("There is no Area in direction: " + direction);
        }

        public static void showPlayerInventory()
        {
            GUI.Instance.appendToOutput(PlayerInventory.Instance.getListOfitems());
        }

        public static void pickUpItem(string itemName)
        {
            Item i = AreaManager.Instance.getCurrAreaItemFromName(itemName);
            if (i != null)
            {
                AreaManager.Instance.currentArea.removeItem(i);
                PlayerInventory.Instance.addItem(i);
                GUI.Instance.appendToOutput(i.name + " added to your inventory.");
            }
            else
            {
                GUI.Instance.appendToOutput("There is no such item here.");
            }
        }
    }
}
