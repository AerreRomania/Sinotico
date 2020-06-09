namespace Sinotico
    {
    partial class RepEffMachines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepEffMachines));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGo = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnHide = new System.Windows.Forms.Button();
            this.btnMedia = new System.Windows.Forms.Button();
            this.cbMedia = new System.Windows.Forms.ComboBox();
            this.btnSquadra = new System.Windows.Forms.Button();
            this.cboFin = new System.Windows.Forms.ComboBox();
            this.cboArt = new System.Windows.Forms.ComboBox();
            this.cboMac = new System.Windows.Forms.ComboBox();
            this.cbSquadra = new System.Windows.Forms.ComboBox();
            this.btnFin = new System.Windows.Forms.Button();
            this.btnArt = new System.Windows.Forms.Button();
            this.btnMachine = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.btnSort);
            this.panel1.Controls.Add(this.btnHide);
            this.panel1.Controls.Add(this.btnMedia);
            this.panel1.Controls.Add(this.cbMedia);
            this.panel1.Controls.Add(this.btnSquadra);
            this.panel1.Controls.Add(this.cboFin);
            this.panel1.Controls.Add(this.cboArt);
            this.panel1.Controls.Add(this.cboMac);
            this.panel1.Controls.Add(this.cbSquadra);
            this.panel1.Controls.Add(this.btnFin);
            this.panel1.Controls.Add(this.btnArt);
            this.panel1.Controls.Add(this.btnMachine);
            this.panel1.Controls.Add(this.lblTo);
            this.panel1.Controls.Add(this.lblFrom);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 104);
            this.panel1.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.White;
            this.btnGo.FlatAppearance.BorderSize = 0;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btnGo.ForeColor = System.Drawing.Color.LightBlue;
            this.btnGo.Image = global::Sinotico.Properties.Resources.checkmark_30;
            this.btnGo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGo.Location = new System.Drawing.Point(377, 18);
            this.btnGo.Margin = new System.Windows.Forms.Padding(2);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(79, 50);
            this.btnGo.TabIndex = 27;
            this.btnGo.Text = "Go";
            this.btnGo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(249, 54);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(116, 20);
            this.dtpTo.TabIndex = 22;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(91, 54);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(116, 20);
            this.dtpFrom.TabIndex = 21;
            // 
            // btnSort
            // 
            this.btnSort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSort.BackColor = System.Drawing.Color.White;
            this.btnSort.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnSort.FlatAppearance.BorderSize = 2;
            this.btnSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnSort.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnSort.Location = new System.Drawing.Point(851, 33);
            this.btnSort.Margin = new System.Windows.Forms.Padding(2);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(107, 24);
            this.btnSort.TabIndex = 20;
            this.btnSort.Text = "Order by";
            this.btnSort.UseVisualStyleBackColor = false;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.BackColor = System.Drawing.Color.White;
            this.btnHide.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnHide.FlatAppearance.BorderSize = 2;
            this.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnHide.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnHide.Image = global::Sinotico.Properties.Resources.hide;
            this.btnHide.Location = new System.Drawing.Point(962, 17);
            this.btnHide.Margin = new System.Windows.Forms.Padding(2);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(52, 57);
            this.btnHide.TabIndex = 19;
            this.btnHide.UseVisualStyleBackColor = false;
            this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
            // 
            // btnMedia
            // 
            this.btnMedia.BackColor = System.Drawing.Color.White;
            this.btnMedia.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnMedia.FlatAppearance.BorderSize = 2;
            this.btnMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMedia.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMedia.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnMedia.Location = new System.Drawing.Point(914, 17);
            this.btnMedia.Margin = new System.Windows.Forms.Padding(2);
            this.btnMedia.Name = "btnMedia";
            this.btnMedia.Size = new System.Drawing.Size(107, 24);
            this.btnMedia.TabIndex = 18;
            this.btnMedia.Text = "Media";
            this.btnMedia.UseVisualStyleBackColor = false;
            this.btnMedia.Click += new System.EventHandler(this.btnMedia_Click);
            // 
            // cbMedia
            // 
            this.cbMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMedia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbMedia.FormattingEnabled = true;
            this.cbMedia.Location = new System.Drawing.Point(914, 42);
            this.cbMedia.Margin = new System.Windows.Forms.Padding(2);
            this.cbMedia.Name = "cbMedia";
            this.cbMedia.Size = new System.Drawing.Size(109, 24);
            this.cbMedia.TabIndex = 17;
            this.cbMedia.SelectedIndexChanged += new System.EventHandler(this.cbMedia_SelectedIndexChanged);
            // 
            // btnSquadra
            // 
            this.btnSquadra.BackColor = System.Drawing.Color.White;
            this.btnSquadra.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnSquadra.FlatAppearance.BorderSize = 2;
            this.btnSquadra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSquadra.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnSquadra.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnSquadra.Location = new System.Drawing.Point(802, 17);
            this.btnSquadra.Margin = new System.Windows.Forms.Padding(2);
            this.btnSquadra.Name = "btnSquadra";
            this.btnSquadra.Size = new System.Drawing.Size(107, 24);
            this.btnSquadra.TabIndex = 16;
            this.btnSquadra.Text = "Squadra";
            this.btnSquadra.UseVisualStyleBackColor = false;
            this.btnSquadra.Click += new System.EventHandler(this.btnSquadra_Click);
            // 
            // cboFin
            // 
            this.cboFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cboFin.FormattingEnabled = true;
            this.cboFin.Items.AddRange(new object[] {
            "<All>",
            "3",
            "7",
            "14"});
            this.cboFin.Location = new System.Drawing.Point(691, 42);
            this.cboFin.Margin = new System.Windows.Forms.Padding(2);
            this.cboFin.Name = "cboFin";
            this.cboFin.Size = new System.Drawing.Size(109, 24);
            this.cboFin.TabIndex = 15;
            this.cboFin.SelectedIndexChanged += new System.EventHandler(this.cboFin_SelectedIndexChanged);
            // 
            // cboArt
            // 
            this.cboArt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cboArt.FormattingEnabled = true;
            this.cboArt.Items.AddRange(new object[] {
            "<All>"});
            this.cboArt.Location = new System.Drawing.Point(580, 42);
            this.cboArt.Margin = new System.Windows.Forms.Padding(2);
            this.cboArt.Name = "cboArt";
            this.cboArt.Size = new System.Drawing.Size(109, 24);
            this.cboArt.TabIndex = 14;
            this.cboArt.SelectedIndexChanged += new System.EventHandler(this.cboArt_SelectedIndexChanged);
            // 
            // cboMac
            // 
            this.cboMac.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMac.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cboMac.FormattingEnabled = true;
            this.cboMac.Items.AddRange(new object[] {
            "<All>"});
            this.cboMac.Location = new System.Drawing.Point(468, 42);
            this.cboMac.Margin = new System.Windows.Forms.Padding(2);
            this.cboMac.Name = "cboMac";
            this.cboMac.Size = new System.Drawing.Size(109, 24);
            this.cboMac.TabIndex = 13;
            this.cboMac.SelectedIndexChanged += new System.EventHandler(this.cboMac_SelectedIndexChanged);
            // 
            // cbSquadra
            // 
            this.cbSquadra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSquadra.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cbSquadra.FormattingEnabled = true;
            this.cbSquadra.Items.AddRange(new object[] {
            "<All>",
            "Squadra 1",
            "Squadra 2",
            "Squadra 3"});
            this.cbSquadra.Location = new System.Drawing.Point(802, 42);
            this.cbSquadra.Margin = new System.Windows.Forms.Padding(2);
            this.cbSquadra.Name = "cbSquadra";
            this.cbSquadra.Size = new System.Drawing.Size(109, 24);
            this.cbSquadra.TabIndex = 11;
            this.cbSquadra.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnFin
            // 
            this.btnFin.BackColor = System.Drawing.Color.White;
            this.btnFin.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnFin.FlatAppearance.BorderSize = 2;
            this.btnFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnFin.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnFin.Location = new System.Drawing.Point(691, 17);
            this.btnFin.Margin = new System.Windows.Forms.Padding(2);
            this.btnFin.Name = "btnFin";
            this.btnFin.Size = new System.Drawing.Size(107, 24);
            this.btnFin.TabIndex = 10;
            this.btnFin.Text = "Finezza";
            this.btnFin.UseVisualStyleBackColor = false;
            this.btnFin.Click += new System.EventHandler(this.btnFin_Click);
            // 
            // btnArt
            // 
            this.btnArt.BackColor = System.Drawing.Color.White;
            this.btnArt.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnArt.FlatAppearance.BorderSize = 2;
            this.btnArt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnArt.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnArt.Location = new System.Drawing.Point(580, 17);
            this.btnArt.Margin = new System.Windows.Forms.Padding(2);
            this.btnArt.Name = "btnArt";
            this.btnArt.Size = new System.Drawing.Size(107, 24);
            this.btnArt.TabIndex = 8;
            this.btnArt.Text = "Articolo";
            this.btnArt.UseVisualStyleBackColor = false;
            this.btnArt.Click += new System.EventHandler(this.btnArt_Click);
            // 
            // btnMachine
            // 
            this.btnMachine.BackColor = System.Drawing.Color.White;
            this.btnMachine.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnMachine.FlatAppearance.BorderSize = 2;
            this.btnMachine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMachine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMachine.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnMachine.Location = new System.Drawing.Point(468, 17);
            this.btnMachine.Margin = new System.Windows.Forms.Padding(2);
            this.btnMachine.Name = "btnMachine";
            this.btnMachine.Size = new System.Drawing.Size(107, 24);
            this.btnMachine.TabIndex = 7;
            this.btnMachine.Text = "Macchina";
            this.btnMachine.UseVisualStyleBackColor = false;
            this.btnMachine.Click += new System.EventHandler(this.btnMachine_Click);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblTo.Location = new System.Drawing.Point(265, 46);
            this.lblTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(0, 15);
            this.lblTo.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(108, 47);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(0, 15);
            this.lblFrom.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(227, 54);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "A:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Da:";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "EFFICIENZA PER MACCHINA/TURNO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvReport
            // 
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.OldLace;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.ColumnHeadersHeight = 90;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.GridColor = System.Drawing.Color.White;
            this.dgvReport.Location = new System.Drawing.Point(0, 104);
            this.dgvReport.Margin = new System.Windows.Forms.Padding(2);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.RowTemplate.Height = 28;
            this.dgvReport.Size = new System.Drawing.Size(1028, 310);
            this.dgvReport.TabIndex = 2;
            this.dgvReport.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvReport_CellPainting);
            this.dgvReport.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvReport_Scroll);
            this.dgvReport.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvReport_Paint);
            // 
            // RepEffMachines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1028, 414);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RepEffMachines";
            this.Text = "Eff Machines";
            this.Load += new System.EventHandler(this.RepEffMachines_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnFin;
        private System.Windows.Forms.Button btnArt;
        private System.Windows.Forms.Button btnMachine;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.ComboBox cbSquadra;
        private System.Windows.Forms.ComboBox cboFin;
        private System.Windows.Forms.ComboBox cboArt;
        private System.Windows.Forms.ComboBox cboMac;
        private System.Windows.Forms.Button btnSquadra;
        private System.Windows.Forms.Button btnMedia;
        private System.Windows.Forms.ComboBox cbMedia;
        private System.Windows.Forms.Button btnHide;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnGo;
    }
    }