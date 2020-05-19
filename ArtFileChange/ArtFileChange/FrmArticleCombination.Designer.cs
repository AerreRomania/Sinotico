namespace ArtFileChange
    {
    partial class FrmArticleCombination
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmArticleCombination));
            this.btnGenSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTArt = new System.Windows.Forms.Label();
            this.myTextBox1 = new ArtFileChange.MyTextBox();
            this.pnTags = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPart = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pbSep = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSep)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenSave
            // 
            this.btnGenSave.BackColor = System.Drawing.Color.SkyBlue;
            this.btnGenSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGenSave.FlatAppearance.BorderSize = 0;
            this.btnGenSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGenSave.Image = ((System.Drawing.Image)(resources.GetObject("btnGenSave.Image")));
            this.btnGenSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenSave.Location = new System.Drawing.Point(129, 480);
            this.btnGenSave.Name = "btnGenSave";
            this.btnGenSave.Size = new System.Drawing.Size(267, 90);
            this.btnGenSave.TabIndex = 31;
            this.btnGenSave.Text = " Save";
            this.btnGenSave.UseVisualStyleBackColor = false;
            this.btnGenSave.Click += new System.EventHandler(this.btnGenSave_Click);
            this.btnGenSave.Paint += new System.Windows.Forms.PaintEventHandler(this.btnGenSave_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 23);
            this.label1.TabIndex = 32;
            this.label1.Text = "Article";
            // 
            // lblTArt
            // 
            this.lblTArt.AutoSize = true;
            this.lblTArt.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTArt.ForeColor = System.Drawing.Color.Black;
            this.lblTArt.Location = new System.Drawing.Point(24, 49);
            this.lblTArt.Name = "lblTArt";
            this.lblTArt.Size = new System.Drawing.Size(137, 38);
            this.lblTArt.TabIndex = 33;
            this.lblTArt.Text = "12345678";
            // 
            // myTextBox1
            // 
            this.myTextBox1.AllowTyping = false;
            this.myTextBox1.BorderColor = System.Drawing.Color.Green;
            this.myTextBox1.Collection = null;
            this.myTextBox1.Location = new System.Drawing.Point(51, 149);
            this.myTextBox1.MaximumSize = new System.Drawing.Size(1000, 80);
            this.myTextBox1.MinimumSize = new System.Drawing.Size(0, 80);
            this.myTextBox1.Name = "myTextBox1";
            this.myTextBox1.Size = new System.Drawing.Size(246, 80);
            this.myTextBox1.TabIndex = 34;
            this.myTextBox1.TitleText = "Part name to add";
            this.myTextBox1.UserText = "";
            this.myTextBox1.UseTextualOnly = false;
            this.myTextBox1.UseUpperCase = false;
            // 
            // pnTags
            // 
            this.pnTags.AutoScroll = true;
            this.pnTags.Location = new System.Drawing.Point(51, 282);
            this.pnTags.Name = "pnTags";
            this.pnTags.Size = new System.Drawing.Size(423, 132);
            this.pnTags.TabIndex = 35;
            this.pnTags.Paint += new System.Windows.Forms.PaintEventHandler(this.pnTags_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lblPart);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.pbSep);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblTArt);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(525, 107);
            this.panel2.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(222, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 23);
            this.label2.TabIndex = 34;
            this.label2.Text = "Loaded part";
            // 
            // lblPart
            // 
            this.lblPart.AutoSize = true;
            this.lblPart.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPart.ForeColor = System.Drawing.Color.Black;
            this.lblPart.Location = new System.Drawing.Point(219, 49);
            this.lblPart.Name = "lblPart";
            this.lblPart.Size = new System.Drawing.Size(51, 38);
            this.lblPart.TabIndex = 35;
            this.lblPart.Text = "XX";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Gray;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox2.Location = new System.Drawing.Point(0, 105);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(525, 1);
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // pbSep
            // 
            this.pbSep.BackColor = System.Drawing.Color.Silver;
            this.pbSep.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbSep.Location = new System.Drawing.Point(0, 106);
            this.pbSep.Name = "pbSep";
            this.pbSep.Size = new System.Drawing.Size(525, 1);
            this.pbSep.TabIndex = 14;
            this.pbSep.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.YellowGreen;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(324, 172);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 57);
            this.button1.TabIndex = 37;
            this.button1.Text = " Add";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.Paint += new System.Windows.Forms.PaintEventHandler(this.button1_Paint);
            // 
            // FrmArticleCombination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(525, 631);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnTags);
            this.Controls.Add(this.myTextBox1);
            this.Controls.Add(this.btnGenSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmArticleCombination";
            this.Text = "Sinotico file combiner - Add combinations";
            this.Load += new System.EventHandler(this.FrmArticleCombination_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSep)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.Button btnGenSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTArt;
        private MyTextBox myTextBox1;
        private System.Windows.Forms.Panel pnTags;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pbSep;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPart;
        }
    }