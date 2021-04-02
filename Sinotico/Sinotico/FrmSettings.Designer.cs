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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.intensity_label = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.Intensity_slider = new System.Windows.Forms.TrackBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cb_interval = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSettings)).BeginInit();
            this.tcSettings.SuspendLayout();
            this.tpColors.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpHolidays.SuspendLayout();
            this.tpUpdate.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Intensity_slider)).BeginInit();
            this.tabPage2.SuspendLayout();
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
            this.tcSettings.Controls.Add(this.tabPage1);
            this.tcSettings.Controls.Add(this.tabPage2);
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
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.intensity_label);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btn_save);
            this.tabPage1.Controls.Add(this.Intensity_slider);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(392, 211);
            this.tabPage1.TabIndex = 3;
            this.tabPage1.Text = "Intensity";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(360, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(188, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "75";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(10, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "50";
            // 
            // intensity_label
            // 
            this.intensity_label.AutoSize = true;
            this.intensity_label.BackColor = System.Drawing.Color.Transparent;
            this.intensity_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.intensity_label.Location = new System.Drawing.Point(3, 76);
            this.intensity_label.Name = "intensity_label";
            this.intensity_label.Size = new System.Drawing.Size(51, 16);
            this.intensity_label.TabIndex = 6;
            this.intensity_label.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select alarm light intensity:";
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.Gainsboro;
            this.btn_save.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_save.FlatAppearance.BorderSize = 0;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Location = new System.Drawing.Point(3, 177);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(386, 31);
            this.btn_save.TabIndex = 4;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // Intensity_slider
            // 
            this.Intensity_slider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Intensity_slider.LargeChange = 25;
            this.Intensity_slider.Location = new System.Drawing.Point(6, 25);
            this.Intensity_slider.Maximum = 100;
            this.Intensity_slider.Minimum = 50;
            this.Intensity_slider.Name = "Intensity_slider";
            this.Intensity_slider.Size = new System.Drawing.Size(380, 45);
            this.Intensity_slider.SmallChange = 25;
            this.Intensity_slider.TabIndex = 0;
            this.Intensity_slider.Tag = "";
            this.Intensity_slider.TickFrequency = 25;
            this.Intensity_slider.Value = 50;
            this.Intensity_slider.ValueChanged += new System.EventHandler(this.Intensity_slider_ValueChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cb_interval);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(392, 211);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Data Interval";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(219, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Select data inteval for queries:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Gainsboro;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(3, 177);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(386, 31);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cb_interval
            // 
            this.cb_interval.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_interval.FormattingEnabled = true;
            this.cb_interval.Items.AddRange(new object[] {
            "3 Months",
            "Current Year",
            "History"});
            this.cb_interval.Location = new System.Drawing.Point(8, 22);
            this.cb_interval.Name = "cb_interval";
            this.cb_interval.Size = new System.Drawing.Size(214, 24);
            this.cb_interval.TabIndex = 8;
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
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Intensity_slider)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TrackBar Intensity_slider;
        private System.Windows.Forms.Label intensity_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_interval;
    }
}