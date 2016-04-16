using System;
using System.Windows.Forms;

namespace TextRPG
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ItemManager.Instance.init();
            NpcManager.Instance.init();
            AreaManager.Instance.init();
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(GUI.Instance);
        }
    }
}
