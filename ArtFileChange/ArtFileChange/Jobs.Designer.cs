namespace ArtFileChange
    {
    partial class Jobs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Jobs));
            this.txtName = new ArtFileChange.MyTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnGenSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.AllowTyping = true;
            this.txtName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtName.Collection = null;
            this.txtName.Location = new System.Drawing.Point(49, 94);
            this.txtName.MaximumSize = new System.Drawing.Size(1000, 80);
            this.txtName.MinimumSize = new System.Drawing.Size(0, 80);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(333, 80);
            this.txtName.TabIndex = 4;
            this.txtName.TextSize = 200;
            this.txtName.TitleText = "Job type";
            this.txtName.UserText = "";
            this.txtName.UseTextualOnly = true;
            this.txtName.UseUpperCase = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(439, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(360, 331);
            this.dataGridView1.TabIndex = 3;
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
            this.btnGenSave.Location = new System.Drawing.Point(121, 240);
            this.btnGenSave.Name = "btnGenSave";
            this.btnGenSave.Size = new System.Drawing.Size(155, 90);
            this.btnGenSave.TabIndex = 34;
            this.btnGenSave.Text = " Add";
            this.btnGenSave.UseVisualStyleBackColor = false;
            this.btnGenSave.Click += new System.EventHandler(this.btnGenSave_Click);
            this.btnGenSave.Paint += new System.Windows.Forms.PaintEventHandler(this.btnGenSave_Paint);
            // 
            // Jobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(840, 433);
            this.Controls.Add(this.btnGenSave);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Jobs";
            this.Text = "Add job types";
            this.Load += new System.EventHandler(this.Jobs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private MyTextBox txtName;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnGenSave;
        }
    }