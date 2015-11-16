using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextRPG
{
    public partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                output.AppendText(">" + input.Text + Environment.NewLine);
                input.Clear();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
