namespace ArtFileChange
    {
    partial class FrmDatabase
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDatabase));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnGenSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pbSep = new System.Windows.Forms.PictureBox();
            this.myTextBox3 = new ArtFileChange.MyTextBox();
            this.myTextBox2 = new ArtFileChange.MyTextBox();
            this.myTextBox1 = new ArtFileChange.MyTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnGenSave);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.pbSep);
            this.panel1.Controls.Add(this.myTextBox3);
            this.panel1.Controls.Add(this.myTextBox2);
            this.panel1.Controls.Add(this.myTextBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1307, 229);
            this.panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Location = new System.Drawing.Point(0, 227);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1307, 1);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // btnGenSave
            // 
            this.btnGenSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenSave.BackColor = System.Drawing.Color.YellowGreen;
            this.btnGenSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGenSave.FlatAppearance.BorderSize = 0;
            this.btnGenSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGenSave.Image = global::ArtFileChange.Properties.Resources.format_32;
            this.btnGenSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenSave.Location = new System.Drawing.Point(1112, 93);
            this.btnGenSave.Name = "btnGenSave";
            this.btnGenSave.Size = new System.Drawing.Size(155, 90);
            this.btnGenSave.TabIndex = 32;
            this.btnGenSave.Text = " Add";
            this.btnGenSave.UseVisualStyleBackColor = false;
            this.btnGenSave.Click += new System.EventHandler(this.btnGenSave_Click);
            this.btnGenSave.Paint += new System.Windows.Forms.PaintEventHandler(this.btnGenSave_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "Store data to database";
            // 
            // pbSep
            // 
            this.pbSep.BackColor = System.Drawing.Color.Silver;
            this.pbSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbSep.Location = new System.Drawing.Point(0, 228);
            this.pbSep.Name = "pbSep";
            this.pbSep.Size = new System.Drawing.Size(1307, 1);
            this.pbSep.TabIndex = 15;
            this.pbSep.TabStop = false;
            // 
            // myTextBox3
            // 
            this.myTextBox3.AllowTyping = true;
            this.myTextBox3.BorderColor = System.Drawing.Color.Orange;
            this.myTextBox3.Collection = null;
            this.myTextBox3.Location = new System.Drawing.Point(718, 103);
            this.myTextBox3.MaximumSize = new System.Drawing.Size(1000, 80);
            this.myTextBox3.MinimumSize = new System.Drawing.Size(0, 80);
            this.myTextBox3.Name = "myTextBox3";
            this.myTextBox3.Size = new System.Drawing.Size(333, 80);
            this.myTextBox3.TabIndex = 2;
            this.myTextBox3.TitleText = "Extended description";
            this.myTextBox3.UserText = "";
            this.myTextBox3.UseTextualOnly = true;
            this.myTextBox3.UseUpperCase = false;
            // 
            // myTextBox2
            // 
            this.myTextBox2.AllowTyping = true;
            this.myTextBox2.BorderColor = System.Drawing.Color.Orange;
            this.myTextBox2.Collection = null;
            this.myTextBox2.Location = new System.Drawing.Point(374, 103);
            this.myTextBox2.MaximumSize = new System.Drawing.Size(1000, 80);
            this.myTextBox2.MinimumSize = new System.Drawing.Size(0, 80);
            this.myTextBox2.Name = "myTextBox2";
            this.myTextBox2.Size = new System.Drawing.Size(333, 80);
            this.myTextBox2.TabIndex = 1;
            this.myTextBox2.TitleText = "Description";
            this.myTextBox2.UserText = "";
            this.myTextBox2.UseTextualOnly = true;
            this.myTextBox2.UseUpperCase = false;
            // 
            // myTextBox1
            // 
            this.myTextBox1.AllowTyping = true;
            this.myTextBox1.BorderColor = System.Drawing.Color.Orange;
            this.myTextBox1.Collection = null;
            this.myTextBox1.Location = new System.Drawing.Point(30, 103);
            this.myTextBox1.MaximumSize = new System.Drawing.Size(1000, 80);
            this.myTextBox1.MinimumSize = new System.Drawing.Size(0, 80);
            this.myTextBox1.Name = "myTextBox1";
            this.myTextBox1.Size = new System.Drawing.Size(333, 80);
            this.myTextBox1.TabIndex = 0;
            this.myTextBox1.TitleText = "Part code";
            this.myTextBox1.UserText = "";
            this.myTextBox1.UseTextualOnly = true;
            this.myTextBox1.UseUpperCase = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 29;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(56, 271);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1199, 447);
            this.dataGridView1.TabIndex = 1;
            // 
            // FrmDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1307, 757);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1325, 804);
            this.Name = "FrmDatabase";
            this.Text = "Sinotico file combiner - Database";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private MyTextBox myTextBox3;
        private MyTextBox myTextBox2;
        private MyTextBox myTextBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.PictureBox pbSep;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGenSave;
        private System.Windows.Forms.PictureBox pictureBox1;
        }
    }