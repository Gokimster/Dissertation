using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public static class GameManager
    {
        public static void changeCurrentArea(string direction)
        {
            AreaManager.Instance.changeCurrentArea(direction);
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
    }
}
