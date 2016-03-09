namespace TextRPG
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.output = new System.Windows.Forms.RichTextBox();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.inputLocationLabel = new System.Windows.Forms.Label();
            this.input = new System.Windows.Forms.TextBox();
            this.mainPanel.SuspendLayout();
            this.inputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.AutoSize = true;
            this.mainPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.mainPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.mainPanel.Controls.Add(this.output);
            this.mainPanel.Controls.Add(this.inputPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(664, 466);
            this.mainPanel.TabIndex = 1;
            // 
            // output
            // 
            this.output.BackColor = System.Drawing.Color.Black;
            this.output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.output.Cursor = System.Windows.Forms.Cursors.Default;
            this.output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output.Font = new System.Drawing.Font("Power Red and Green", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.ForeColor = System.Drawing.Color.White;
            this.output.Location = new System.Drawing.Point(0, 0);
            this.output.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.output.Size = new System.Drawing.Size(664, 425);
            this.output.TabIndex = 3;
            this.output.Text = "";
            // 
            // inputPanel
            // 
            this.inputPanel.AutoSize = true;
            this.inputPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.inputPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputPanel.Controls.Add(this.inputLocationLabel);
            this.inputPanel.Controls.Add(this.input);
            this.inputPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.inputPanel.Location = new System.Drawing.Point(0, 425);
            this.inputPanel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(664, 41);
            this.inputPanel.TabIndex = 2;
            // 
            // inputLocationLabel
            // 
            this.inputLocationLabel.AutoSize = true;
            this.inputLocationLabel.BackColor = System.Drawing.Color.Transparent;
            this.inputLocationLabel.Font = new System.Drawing.Font("Power Red and Green", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputLocationLabel.ForeColor = System.Drawing.Color.White;
            this.inputLocationLabel.Location = new System.Drawing.Point(4, 6);
            this.inputLocationLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.inputLocationLabel.Name = "inputLocationLabel";
            this.inputLocationLabel.Size = new System.Drawing.Size(22, 25);
            this.inputLocationLabel.TabIndex = 2;
            this.inputLocationLabel.Text = ">";
            // 
            // input
            // 
            this.input.AcceptsTab = true;
            this.input.AllowDrop = true;
            this.input.AutoCompleteCustomSource.AddRange(new string[] {
            "inspect",
            "go",
            "head",
            "inventory",
            "show",
            "attack",
            "ItemManager.Instance:",
            "AreaManager.Instance:",
            "NpcManager.Instance:"});
            this.input.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.input.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.input.BackColor = System.Drawing.Color.Black;
            this.input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.input.Font = new System.Drawing.Font("Power Red and Green", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.input.ForeColor = System.Drawing.Color.White;
            this.input.Location = new System.Drawing.Point(30, 5);
            this.input.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.input.Multiline = true;
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(608, 31);
            this.input.TabIndex = 1;
            this.input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.input_KeyDown);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(664, 466);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Power Red and Green", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TextRPG";
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            this.inputPanel.ResumeLayout(false);
            this.inputPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.Panel inputPanel;
        private System.Windows.Forms.Label inputLocationLabel;
        private System.Windows.Forms.RichTextBox output;
    }
}

