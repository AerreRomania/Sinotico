namespace ArtFileChange
    {
    partial class FrmRename
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRename));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFilename = new System.Windows.Forms.Label();
            this.lblPath = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblReset = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pbSep = new System.Windows.Forms.PictureBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pbFileico = new System.Windows.Forms.PictureBox();
            this.lblDb = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operatorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jobTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox4 = new ArtFileChange.MyTextBox();
            this.textBox5 = new ArtFileChange.MyTextBox();
            this.textBox3 = new ArtFileChange.MyTextBox();
            this.textBox2 = new ArtFileChange.MyTextBox();
            this.textBox1 = new ArtFileChange.MyTextBox();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFileico)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Gainsboro;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1147, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.resetToolStripMenuItem,
            this.fromatToolStripMenuItem,
            this.historyToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 24);
            this.fileToolStripMenuItem.Text = "FILE";
            // 
            // historyToolStripMenuItem
            // 
            this.historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            this.historyToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.historyToolStripMenuItem.Text = "History";
            this.historyToolStripMenuItem.Click += new System.EventHandler(this.historyToolStripMenuItem_Click_1);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.operatorsToolStripMenuItem,
            this.jobTypesToolStripMenuItem});
            this.databaseToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.databaseToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.databaseToolStripMenuItem.Text = "EDIT";
            // 
            // lblFilename
            // 
            this.lblFilename.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilename.Location = new System.Drawing.Point(161, 68);
            this.lblFilename.Name = "lblFilename";
            this.lblFilename.Size = new System.Drawing.Size(593, 37);
            this.lblFilename.TabIndex = 2;
            // 
            // lblPath
            // 
            this.lblPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPath.Location = new System.Drawing.Point(161, 116);
            this.lblPath.Name = "lblPath";
            this.lblPath.Size = new System.Drawing.Size(593, 47);
            this.lblPath.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblReset);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pbSep);
            this.panel1.Controls.Add(this.lblSearch);
            this.panel1.Controls.Add(this.pbFileico);
            this.panel1.Controls.Add(this.lblDb);
            this.panel1.Controls.Add(this.lblPath);
            this.panel1.Controls.Add(this.lblFilename);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1147, 190);
            this.panel1.TabIndex = 28;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1147, 53);
            this.panel2.TabIndex = 31;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 25);
            this.label1.TabIndex = 32;
            this.label1.Text = "Work file";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1147, 25);
            this.statusStrip1.TabIndex = 32;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(50, 20);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // lblLogo
            // 
            this.lblLogo.BackColor = System.Drawing.Color.White;
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLogo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = System.Drawing.Color.Black;
            this.lblLogo.Location = new System.Drawing.Point(0, 530);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(1147, 109);
            this.lblLogo.TabIndex = 33;
            this.lblLogo.Text = "Sinotico file combiner - Sinotico extension 2019.1";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblLogo.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.pictureBox1.Location = new System.Drawing.Point(550, 241);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1, 306);
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // lblReset
            // 
            this.lblReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblReset.Image = ((System.Drawing.Image)(resources.GetObject("lblReset.Image")));
            this.lblReset.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblReset.Location = new System.Drawing.Point(877, 133);
            this.lblReset.Name = "lblReset";
            this.lblReset.Size = new System.Drawing.Size(109, 42);
            this.lblReset.TabIndex = 29;
            this.lblReset.Text = "Refresh";
            this.lblReset.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblReset.Click += new System.EventHandler(this.lblReset_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox2.Location = new System.Drawing.Point(0, 188);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(1147, 1);
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // pbSep
            // 
            this.pbSep.BackColor = System.Drawing.Color.Silver;
            this.pbSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbSep.Location = new System.Drawing.Point(0, 189);
            this.pbSep.Name = "pbSep";
            this.pbSep.Size = new System.Drawing.Size(1147, 1);
            this.pbSep.TabIndex = 14;
            this.pbSep.TabStop = false;
            // 
            // lblSearch
            // 
            this.lblSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSearch.Image = global::ArtFileChange.Properties.Resources.search_32;
            this.lblSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSearch.Location = new System.Drawing.Point(746, 133);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(109, 42);
            this.lblSearch.TabIndex = 30;
            this.lblSearch.Text = "Search";
            this.lblSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSearch.Click += new System.EventHandler(this.lblSearch_Click);
            // 
            // pbFileico
            // 
            this.pbFileico.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbFileico.Image = global::ArtFileChange.Properties.Resources.generate_24;
            this.pbFileico.Location = new System.Drawing.Point(51, 79);
            this.pbFileico.Name = "pbFileico";
            this.pbFileico.Size = new System.Drawing.Size(90, 84);
            this.pbFileico.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFileico.TabIndex = 1;
            this.pbFileico.TabStop = false;
            this.pbFileico.Click += new System.EventHandler(this.pbFileico_Click);
            this.pbFileico.DoubleClick += new System.EventHandler(this.pbFileico_DoubleClick);
            // 
            // lblDb
            // 
            this.lblDb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDb.Image = global::ArtFileChange.Properties.Resources.database_32;
            this.lblDb.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDb.Location = new System.Drawing.Point(1008, 133);
            this.lblDb.Name = "lblDb";
            this.lblDb.Size = new System.Drawing.Size(118, 42);
            this.lblDb.TabIndex = 31;
            this.lblDb.Text = "Codification";
            this.lblDb.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDb.Click += new System.EventHandler(this.lblDb_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.SkyBlue;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Cursor = System.Windows.Forms.Cursors.Default;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button3.Image = global::ArtFileChange.Properties.Resources.format_32;
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point(604, 393);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(246, 80);
            this.button3.TabIndex = 22;
            this.button3.Text = " Format";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            this.button3.Paint += new System.Windows.Forms.PaintEventHandler(this.button3_Paint);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::ArtFileChange.Properties.Resources.search_32;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Image = global::ArtFileChange.Properties.Resources.reset_32;
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.resetToolStripMenuItem.Text = "R&eset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // fromatToolStripMenuItem
            // 
            this.fromatToolStripMenuItem.Image = global::ArtFileChange.Properties.Resources.format_32;
            this.fromatToolStripMenuItem.Name = "fromatToolStripMenuItem";
            this.fromatToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.fromatToolStripMenuItem.Text = "Fromat";
            this.fromatToolStripMenuItem.Click += new System.EventHandler(this.fromatToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Image = global::ArtFileChange.Properties.Resources.database_32;
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.editToolStripMenuItem.Text = "Codification";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // operatorsToolStripMenuItem
            // 
            this.operatorsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("operatorsToolStripMenuItem.Image")));
            this.operatorsToolStripMenuItem.Name = "operatorsToolStripMenuItem";
            this.operatorsToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.operatorsToolStripMenuItem.Text = "Operators";
            this.operatorsToolStripMenuItem.Click += new System.EventHandler(this.operatorsToolStripMenuItem_Click);
            // 
            // jobTypesToolStripMenuItem
            // 
            this.jobTypesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jobTypesToolStripMenuItem.Image")));
            this.jobTypesToolStripMenuItem.Name = "jobTypesToolStripMenuItem";
            this.jobTypesToolStripMenuItem.Size = new System.Drawing.Size(216, 26);
            this.jobTypesToolStripMenuItem.Text = "Job types";
            this.jobTypesToolStripMenuItem.Click += new System.EventHandler(this.jobTypesToolStripMenuItem_Click);
            // 
            // textBox4
            // 
            this.textBox4.AllowTyping = false;
            this.textBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox4.BorderColor = System.Drawing.Color.Crimson;
            this.textBox4.Collection = null;
            this.textBox4.Location = new System.Drawing.Point(604, 263);
            this.textBox4.MaximumSize = new System.Drawing.Size(1000, 90);
            this.textBox4.MinimumSize = new System.Drawing.Size(0, 90);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(383, 90);
            this.textBox4.TabIndex = 31;
            this.textBox4.TextSize = 32767;
            this.textBox4.TitleText = "Formated article (file name)";
            this.textBox4.UserText = "";
            this.textBox4.UseTextualOnly = true;
            this.textBox4.UseUpperCase = false;
            // 
            // textBox5
            // 
            this.textBox5.AllowTyping = true;
            this.textBox5.BorderColor = System.Drawing.Color.Green;
            this.textBox5.Collection = null;
            this.textBox5.Location = new System.Drawing.Point(219, 411);
            this.textBox5.MaximumSize = new System.Drawing.Size(1000, 90);
            this.textBox5.MinimumSize = new System.Drawing.Size(0, 90);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(293, 90);
            this.textBox5.TabIndex = 4;
            this.textBox5.TextSize = 32767;
            this.textBox5.TitleText = "Article (File name)";
            this.textBox5.UserText = "";
            this.textBox5.UseTextualOnly = true;
            this.textBox5.UseUpperCase = false;
            // 
            // textBox3
            // 
            this.textBox3.AllowTyping = true;
            this.textBox3.BorderColor = System.Drawing.Color.DarkOrange;
            this.textBox3.Collection = null;
            this.textBox3.Location = new System.Drawing.Point(219, 263);
            this.textBox3.MaximumSize = new System.Drawing.Size(1000, 90);
            this.textBox3.MinimumSize = new System.Drawing.Size(0, 90);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(175, 90);
            this.textBox3.TabIndex = 2;
            this.textBox3.TextSize = 32767;
            this.textBox3.TitleText = "Color code";
            this.textBox3.UserText = "";
            this.textBox3.UseTextualOnly = true;
            this.textBox3.UseUpperCase = false;
            // 
            // textBox2
            // 
            this.textBox2.AllowTyping = true;
            this.textBox2.BorderColor = System.Drawing.Color.DarkOrange;
            this.textBox2.Collection = null;
            this.textBox2.Location = new System.Drawing.Point(39, 411);
            this.textBox2.MaximumSize = new System.Drawing.Size(1000, 90);
            this.textBox2.MinimumSize = new System.Drawing.Size(0, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(174, 90);
            this.textBox2.TabIndex = 3;
            this.textBox2.TextSize = 32767;
            this.textBox2.TitleText = "Size code";
            this.textBox2.UserText = "";
            this.textBox2.UseTextualOnly = true;
            this.textBox2.UseUpperCase = false;
            // 
            // textBox1
            // 
            this.textBox1.AllowTyping = false;
            this.textBox1.BorderColor = System.Drawing.Color.DarkOrange;
            this.textBox1.Collection = null;
            this.textBox1.Location = new System.Drawing.Point(39, 263);
            this.textBox1.MaximumSize = new System.Drawing.Size(1000, 90);
            this.textBox1.MinimumSize = new System.Drawing.Size(0, 90);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(174, 90);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextSize = 0;
            this.textBox1.TitleText = "Part name";
            this.textBox1.UserText = "";
            this.textBox1.UseTextualOnly = false;
            this.textBox1.UseUpperCase = false;
            // 
            // FrmRename
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1147, 664);
            this.Controls.Add(this.lblLogo);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1165, 711);
            this.Name = "FrmRename";
            this.Text = "Sinotico file combiner";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmRename_Load);
            this.SizeChanged += new System.EventHandler(this.FrmRename_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFileico)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.PictureBox pbFileico;
        private System.Windows.Forms.Label lblFilename;
        private System.Windows.Forms.Label lblPath;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.Button button3;
        private MyTextBox textBox5;
        private MyTextBox textBox3;
        private MyTextBox textBox2;
        private MyTextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbSep;
        private System.Windows.Forms.Label lblReset;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Label lblDb;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private MyTextBox textBox4;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem fromatToolStripMenuItem;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.ToolStripMenuItem operatorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jobTypesToolStripMenuItem;
        }
    }