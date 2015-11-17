using System;
using System.Windows.Forms;

namespace TextRPG
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles pressing Enter while in input textbox
        /// add the input to the output
        /// clear input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                appendToOutput(">" + input.Text);
                input.Clear();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// append a string to the output textbox and add a new line after
        /// </summary>
        /// <param name="s"></param>
        public void appendToOutput(String s)
        {
            output.AppendText(s + Environment.NewLine);
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
