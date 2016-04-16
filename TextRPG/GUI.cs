using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextRPG
{
    public partial class GUI : Form
    {
        public static readonly GUI Instance = new GUI();
        public GUI()
        {
            InitializeComponent();
            //appendToOutput(AreaManager.Instance.getFullAreaDescription(AreaManager.Instance.currentArea));
        }

        /// <summary>
        /// Handles pressing Enter while in input textbox;
        /// add the input to the output;
        /// clear input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void input_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                Grammar g = new Grammar();
                appendToOutput(input.Text, true);
                if (!g.parse(input.Text))
                {
                    appendToOutput("Invalid Command");
                }
                input.Clear();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// append a string to the output textbox and add a new line after
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fromUser"></param>
        public void appendToOutput(String s, bool fromUser = false)
        {
            if (fromUser)
            {
                output.SelectionStart = output.TextLength;
                output.SelectionLength = 0;

                output.SelectionColor = Color.LightSteelBlue;
                output.AppendText(">" + s + Environment.NewLine);
                output.SelectionColor = output.ForeColor;
            }
            else
            {
                output.AppendText(s + Environment.NewLine);
            }
            output.SelectionStart = output.Text.Length;
            output.ScrollToCaret();
        }

        /// <summary>
        /// clear the output textbox
        /// </summary>
        public void clearOutput()
        {
            output.Clear();
        }
    }
}
