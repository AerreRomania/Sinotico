namespace ArtFileChange
    {
    partial class Operators
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Operators));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtName = new ArtFileChange.MyTextBox();
            this.txtCode = new ArtFileChange.MyTextBox();
            this.btnGenSave = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEmail = new ArtFileChange.MyTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(322, 41);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(420, 438);
            this.dataGridView1.TabIndex = 0;
            // 
            // txtName
            // 
            this.txtName.AllowTyping = true;
            this.txtName.BorderColor = System.Drawing.Color.Teal;
            this.txtName.Collection = null;
            this.txtName.Location = new System.Drawing.Point(30, 41);
            this.txtName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtName.MaximumSize = new System.Drawing.Size(750, 65);
            this.txtName.MinimumSize = new System.Drawing.Size(0, 65);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(250, 65);
            this.txtName.TabIndex = 2;
            this.txtName.TextSize = 32767;
            this.txtName.TitleText = "Name";
            this.txtName.UserText = "";
            this.txtName.UseTextualOnly = true;
            this.txtName.UseUpperCase = true;
            // 
            // txtCode
            // 
            this.txtCode.AllowTyping = true;
            this.txtCode.BorderColor = System.Drawing.Color.Teal;
            this.txtCode.Collection = null;
            this.txtCode.Location = new System.Drawing.Point(30, 130);
            this.txtCode.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCode.MaximumSize = new System.Drawing.Size(750, 65);
            this.txtCode.MinimumSize = new System.Drawing.Size(0, 65);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(250, 65);
            this.txtCode.TabIndex = 3;
            this.txtCode.TextSize = 32767;
            this.txtCode.TitleText = "Code (pin)";
            this.txtCode.UserText = "";
            this.txtCode.UseTextualOnly = true;
            this.txtCode.UseUpperCase = false;
            // 
            // btnGenSave
            // 
            this.btnGenSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnGenSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGenSave.FlatAppearance.BorderSize = 0;
            this.btnGenSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGenSave.Image = global::ArtFileChange.Properties.Resources.format_32;
            this.btnGenSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenSave.Location = new System.Drawing.Point(91, 406);
            this.btnGenSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGenSave.Name = "btnGenSave";
            this.btnGenSave.Size = new System.Drawing.Size(116, 73);
            this.btnGenSave.TabIndex = 33;
            this.btnGenSave.Text = " Add";
            this.btnGenSave.UseVisualStyleBackColor = false;
            this.btnGenSave.Click += new System.EventHandler(this.btnGenSave_Click);
            this.btnGenSave.Paint += new System.Windows.Forms.PaintEventHandler(this.btnGenSave_Paint);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(30, 253);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(251, 38);
            this.comboBox1.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.label1.Location = new System.Drawing.Point(38, 220);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 19);
            this.label1.TabIndex = 35;
            this.label1.Text = "Job type";
            // 
            // txtEmail
            // 
            this.txtEmail.AllowTyping = true;
            this.txtEmail.BorderColor = System.Drawing.Color.Red;
            this.txtEmail.Collection = null;
            this.txtEmail.Location = new System.Drawing.Point(30, 305);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtEmail.MaximumSize = new System.Drawing.Size(750, 65);
            this.txtEmail.MinimumSize = new System.Drawing.Size(0, 65);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(250, 65);
            this.txtEmail.TabIndex = 36;
            this.txtEmail.TextSize = 32767;
            this.txtEmail.TitleText = "Email";
            this.txtEmail.UserText = "";
            this.txtEmail.UseTextualOnly = true;
            this.txtEmail.UseUpperCase = true;
            // 
            // Operators
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(772, 503);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnGenSave);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Operators";
            this.Text = "Add operators";
            this.Load += new System.EventHandler(this.Operators_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private MyTextBox txtName;
        private MyTextBox txtCode;
        private System.Windows.Forms.Button btnGenSave;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private MyTextBox txtEmail;
    }
    }