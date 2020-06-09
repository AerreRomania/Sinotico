namespace Sinotico
{
    partial class FrmSettings
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
            this.dgvSettings = new System.Windows.Forms.DataGridView();
            this.btnCalendar = new System.Windows.Forms.Button();
            this.tcSettings = new System.Windows.Forms.TabControl();
            this.tpColors = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.tpHolidays = new System.Windows.Forms.TabPage();
            this.tpUpdate = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtDownloadSource = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.linkLabel1 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbUpdateRuntime = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).BeginInit();
            this.tcSettings.SuspendLayout();
            this.tpColors.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpHolidays.SuspendLayout();
            this.tpUpdate.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSettings
            // 
            this.dgvSettings.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSettings.Location = new System.Drawing.Point(3, 3);
            this.dgvSettings.Name = "dgvSettings";
            this.dgvSettings.Size = new System.Drawing.Size(386, 205);
            this.dgvSettings.TabIndex = 0;
            // 
            // btnCalendar
            // 
            this.btnCalendar.FlatAppearance.BorderSize = 0;
            this.btnCalendar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalendar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCalendar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnCalendar.Image = global::Sinotico.Properties.Resources.calendar_32x32;
            this.btnCalendar.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCalendar.Location = new System.Drawing.Point(8, 6);
            this.btnCalendar.Name = "btnCalendar";
            this.btnCalendar.Size = new System.Drawing.Size(97, 61);
            this.btnCalendar.TabIndex = 712;
            this.btnCalendar.Text = "Define Holidays";
            this.btnCalendar.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCalendar.UseVisualStyleBackColor = true;
            this.btnCalendar.Click += new System.EventHandler(this.btnCalendar_Click);
            // 
            // tcSettings
            // 
            this.tcSettings.Controls.Add(this.tpColors);
            this.tcSettings.Controls.Add(this.tpHolidays);
            this.tcSettings.Controls.Add(this.tpUpdate);
            this.tcSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcSettings.Location = new System.Drawing.Point(0, 0);
            this.tcSettings.Multiline = true;
            this.tcSettings.Name = "tcSettings";
            this.tcSettings.SelectedIndex = 0;
            this.tcSettings.Size = new System.Drawing.Size(400, 237);
            this.tcSettings.TabIndex = 3;
            // 
            // tpColors
            // 
            this.tpColors.Controls.Add(this.panel1);
            this.tpColors.Controls.Add(this.dgvSettings);
            this.tpColors.Location = new System.Drawing.Point(4, 22);
            this.tpColors.Name = "tpColors";
            this.tpColors.Padding = new System.Windows.Forms.Padding(3);
            this.tpColors.Size = new System.Drawing.Size(392, 211);
            this.tpColors.TabIndex = 0;
            this.tpColors.Text = "Colors";
            this.tpColors.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 177);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 31);
            this.panel1.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(0, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(386, 31);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // tpHolidays
            // 
            this.tpHolidays.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.tpHolidays.Controls.Add(this.btnCalendar);
            this.tpHolidays.Location = new System.Drawing.Point(4, 22);
            this.tpHolidays.Name = "tpHolidays";
            this.tpHolidays.Padding = new System.Windows.Forms.Padding(3);
            this.tpHolidays.Size = new System.Drawing.Size(392, 211);
            this.tpHolidays.TabIndex = 1;
            this.tpHolidays.Text = "Holidays";
            // 
            // tpUpdate
            // 
            this.tpUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.tpUpdate.Controls.Add(this.panel2);
            this.tpUpdate.Controls.Add(this.btnUpdate);
            this.tpUpdate.Controls.Add(this.label1);
            this.tpUpdate.Controls.Add(this.cbUpdateRuntime);
            this.tpUpdate.Location = new System.Drawing.Point(4, 22);
            this.tpUpdate.Name = "tpUpdate";
            this.tpUpdate.Padding = new System.Windows.Forms.Padding(3);
            this.tpUpdate.Size = new System.Drawing.Size(392, 211);
            this.tpUpdate.TabIndex = 2;
            this.tpUpdate.Text = "Update";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtDownloadSource);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.linkLabel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 135);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(386, 73);
            this.panel2.TabIndex = 6;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // txtDownloadSource
            // 
            this.txtDownloadSource.BackColor = System.Drawing.Color.White;
            this.txtDownloadSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDownloadSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDownloadSource.Location = new System.Drawing.Point(0, 47);
            this.txtDownloadSource.Name = "txtDownloadSource";
            this.txtDownloadSource.Size = new System.Drawing.Size(386, 24);
            this.txtDownloadSource.TabIndex = 3;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 30);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(386, 17);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.linkLabel1.Location = new System.Drawing.Point(0, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(386, 30);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::Sinotico.Properties.Resources.icons8_update_64;
            this.btnUpdate.Location = new System.Drawing.Point(320, 63);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(64, 64);
            this.btnUpdate.TabIndex = 5;
            this.btnUpdate.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.label1.Location = new System.Drawing.Point(6, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(378, 59);
            this.label1.TabIndex = 4;
            this.label1.Text = "Application version update and auto update options";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbUpdateRuntime
            // 
            this.cbUpdateRuntime.AutoSize = true;
            this.cbUpdateRuntime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.cbUpdateRuntime.Location = new System.Drawing.Point(11, 97);
            this.cbUpdateRuntime.Name = "cbUpdateRuntime";
            this.cbUpdateRuntime.Size = new System.Drawing.Size(86, 17);
            this.cbUpdateRuntime.TabIndex = 2;
            this.cbUpdateRuntime.Text = "Auto Update";
            this.cbUpdateRuntime.UseVisualStyleBackColor = true;
            this.cbUpdateRuntime.CheckedChanged += new System.EventHandler(this.cbUpdateRuntime_CheckedChanged_1);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.ClientSize = new System.Drawing.Size(400, 237);
            this.Controls.Add(this.tcSettings);
            this.Name = "FrmSettings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).EndInit();
            this.tcSettings.ResumeLayout(false);
            this.tpColors.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tpHolidays.ResumeLayout(false);
            this.tpUpdate.ResumeLayout(false);
            this.tpUpdate.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSettings;
        private System.Windows.Forms.Button btnCalendar;
        private System.Windows.Forms.TabControl tcSettings;
        private System.Windows.Forms.TabPage tpColors;
        private System.Windows.Forms.TabPage tpHolidays;
        private System.Windows.Forms.TabPage tpUpdate;
        private System.Windows.Forms.TextBox txtDownloadSource;
        private System.Windows.Forms.CheckBox cbUpdateRuntime;
        private System.Windows.Forms.Label linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox btnUpdate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btnSave;
    }
}