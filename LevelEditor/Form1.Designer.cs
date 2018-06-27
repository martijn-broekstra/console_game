using System.Windows.Forms;

namespace LevelEditor
{
    partial class Form1
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Controls.Clear();

            this.levelWidth = new System.Windows.Forms.TextBox();
            this.levelHeight = new System.Windows.Forms.TextBox();
            this.resizeButton = new System.Windows.Forms.Button();
            this.gridPanel = new Panel();

            this.gridPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // tableLayoutPanel1
            // 

            this.levelWidth.Location = new System.Drawing.Point(10, 20);
            this.levelWidth.Width = 100;
            this.levelWidth.TabIndex = 0;

            this.levelHeight.Location = new System.Drawing.Point(120, 20);
            this.levelHeight.Width = 100;
            this.levelHeight.TabIndex = 0;

            this.resizeButton.Location = new System.Drawing.Point(230, 20);
            this.resizeButton.Text = "resize";
            this.resizeButton.Click += new System.EventHandler(ClickResize);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 590);

            this.gridPanel.Location = new System.Drawing.Point(22, 95);
            this.gridPanel.Size = new System.Drawing.Size(871, 483);

            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.levelWidth);
            this.Controls.Add(this.levelHeight);
            this.Controls.Add(this.resizeButton);

            this.Name = "LevelEditor";
            this.Text = "LevelEditor";
            this.gridPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private TableLayoutPanel CreateTable(int width, int height)
        {
            var table = new TableLayoutPanel();
            table.ColumnCount = width;
            table.RowCount = height;

            for (int i = 0; i < width; i++)
            {
                table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100 / width));
                for (int j = 0; j < height; j++)
                {
                    var button = new System.Windows.Forms.Button();
                    table.Controls.Add(button, i, j);

                    button.Location = new System.Drawing.Point(3, 3);
                    button.Name = $"button{i}_{j}";
                    button.Size = new System.Drawing.Size(75, 23);
                    button.TabIndex = 0;
                    button.Text = $"button{i}_{j}";
                    button.UseVisualStyleBackColor = true;

                    button.Click += new System.EventHandler(ClickButton);
                }
            }
            for (int j = 0; j < height; j++)
            {
                table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100 / height));
            }

            table.Dock = DockStyle.Fill;
            table.Name = "levelGrid";
            table.TabIndex = 0;

            return table;
        }

        private void ClickResize(object sender, System.EventArgs e)
        {
            width = int.Parse(levelWidth.Text);
            height = int.Parse(levelHeight.Text);

            this.gridPanel.Controls.Clear();
            this.gridPanel.Controls.Add(CreateTable(width, height));
        }

        private void ClickButton(object sender, System.EventArgs e)
        {
            var form = new GridItemPopupForm();

            form.ShowDialog(this);
        }

        private int width = 10;
        private int height = 10;

        private System.Windows.Forms.Button resizeButton;
        private System.Windows.Forms.TextBox levelWidth;
        private System.Windows.Forms.TextBox levelHeight;
        private System.Windows.Forms.Panel gridPanel;
    }
}

