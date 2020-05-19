namespace Sinotico
    {
    partial class RepMachines
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepMachines));
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRed = new System.Windows.Forms.CheckBox();
            this.cbYellow = new System.Windows.Forms.CheckBox();
            this.cbMostra = new System.Windows.Forms.CheckBox();
            this.cbGreen = new System.Windows.Forms.CheckBox();
            this.cboMedia = new System.Windows.Forms.ComboBox();
            this.btnMediaTot = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnSquadraTot = new System.Windows.Forms.Button();
            this.btnLineTot = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cboBlocks = new System.Windows.Forms.ComboBox();
            this.cboLines = new System.Windows.Forms.ComboBox();
            this.btnSort = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
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
            this.dgvReport.ColumnHeadersHeight = 40;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.GridColor = System.Drawing.Color.White;
            this.dgvReport.Location = new System.Drawing.Point(0, 117);
            this.dgvReport.Margin = new System.Windows.Forms.Padding(2);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.RowTemplate.Height = 28;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(1028, 345);
            this.dgvReport.TabIndex = 1;
            this.dgvReport.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvReport_CellPainting);
            this.dgvReport.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvReport_Scroll);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.cbRed);
            this.panel1.Controls.Add(this.cbYellow);
            this.panel1.Controls.Add(this.cbMostra);
            this.panel1.Controls.Add(this.cbGreen);
            this.panel1.Controls.Add(this.cboMedia);
            this.panel1.Controls.Add(this.btnMediaTot);
            this.panel1.Controls.Add(this.dtpTo);
            this.panel1.Controls.Add(this.dtpFrom);
            this.panel1.Controls.Add(this.btnSquadraTot);
            this.panel1.Controls.Add(this.btnLineTot);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.cboBlocks);
            this.panel1.Controls.Add(this.cboLines);
            this.panel1.Controls.Add(this.btnSort);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblTo);
            this.panel1.Controls.Add(this.lblFrom);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 117);
            this.panel1.TabIndex = 2;
            // 
            // cbRed
            // 
            this.cbRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRed.Location = new System.Drawing.Point(861, 21);
            this.cbRed.Name = "cbRed";
            this.cbRed.Size = new System.Drawing.Size(22, 22);
            this.cbRed.TabIndex = 726;
            this.cbRed.UseVisualStyleBackColor = true;
            // 
            // cbYellow
            // 
            this.cbYellow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbYellow.Location = new System.Drawing.Point(829, 21);
            this.cbYellow.Name = "cbYellow";
            this.cbYellow.Size = new System.Drawing.Size(22, 22);
            this.cbYellow.TabIndex = 725;
            this.cbYellow.UseVisualStyleBackColor = true;
            // 
            // cbMostra
            // 
            this.cbMostra.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMostra.Location = new System.Drawing.Point(893, 21);
            this.cbMostra.Name = "cbMostra";
            this.cbMostra.Size = new System.Drawing.Size(22, 22);
            this.cbMostra.TabIndex = 724;
            this.cbMostra.UseVisualStyleBackColor = true;
            // 
            // cbGreen
            // 
            this.cbGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbGreen.Location = new System.Drawing.Point(797, 21);
            this.cbGreen.Name = "cbGreen";
            this.cbGreen.Size = new System.Drawing.Size(22, 22);
            this.cbGreen.TabIndex = 723;
            this.cbGreen.UseVisualStyleBackColor = true;
            // 
            // cboMedia
            // 
            this.cboMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMedia.FormattingEnabled = true;
            this.cboMedia.Items.AddRange(new object[] {
            "<All>"});
            this.cboMedia.Location = new System.Drawing.Point(682, 41);
            this.cboMedia.Margin = new System.Windows.Forms.Padding(2);
            this.cboMedia.Name = "cboMedia";
            this.cboMedia.Size = new System.Drawing.Size(101, 21);
            this.cboMedia.TabIndex = 33;
            this.cboMedia.SelectedIndexChanged += new System.EventHandler(this.cboMedia_SelectedIndexChanged);
            // 
            // btnMediaTot
            // 
            this.btnMediaTot.BackColor = System.Drawing.Color.White;
            this.btnMediaTot.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnMediaTot.FlatAppearance.BorderSize = 2;
            this.btnMediaTot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMediaTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnMediaTot.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnMediaTot.Location = new System.Drawing.Point(683, 15);
            this.btnMediaTot.Margin = new System.Windows.Forms.Padding(2);
            this.btnMediaTot.Name = "btnMediaTot";
            this.btnMediaTot.Size = new System.Drawing.Size(100, 24);
            this.btnMediaTot.TabIndex = 32;
            this.btnMediaTot.Text = "Media";
            this.btnMediaTot.UseVisualStyleBackColor = false;
            this.btnMediaTot.Click += new System.EventHandler(this.btnMediaTot_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(253, 45);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(116, 20);
            this.dtpTo.TabIndex = 31;
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(93, 45);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(116, 20);
            this.dtpFrom.TabIndex = 30;
            // 
            // btnSquadraTot
            // 
            this.btnSquadraTot.BackColor = System.Drawing.Color.White;
            this.btnSquadraTot.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnSquadraTot.FlatAppearance.BorderSize = 2;
            this.btnSquadraTot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSquadraTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnSquadraTot.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnSquadraTot.Location = new System.Drawing.Point(579, 15);
            this.btnSquadraTot.Margin = new System.Windows.Forms.Padding(2);
            this.btnSquadraTot.Name = "btnSquadraTot";
            this.btnSquadraTot.Size = new System.Drawing.Size(100, 24);
            this.btnSquadraTot.TabIndex = 29;
            this.btnSquadraTot.Text = "Squadra";
            this.btnSquadraTot.UseVisualStyleBackColor = false;
            this.btnSquadraTot.Click += new System.EventHandler(this.btnSquadraTot_Click);
            // 
            // btnLineTot
            // 
            this.btnLineTot.BackColor = System.Drawing.Color.White;
            this.btnLineTot.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnLineTot.FlatAppearance.BorderSize = 2;
            this.btnLineTot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLineTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnLineTot.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnLineTot.Location = new System.Drawing.Point(474, 15);
            this.btnLineTot.Margin = new System.Windows.Forms.Padding(2);
            this.btnLineTot.Name = "btnLineTot";
            this.btnLineTot.Size = new System.Drawing.Size(100, 24);
            this.btnLineTot.TabIndex = 28;
            this.btnLineTot.Text = "Linea";
            this.btnLineTot.UseVisualStyleBackColor = false;
            this.btnLineTot.Click += new System.EventHandler(this.btnLineTot_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.LightBlue;
            this.button2.Image = global::Sinotico.Properties.Resources.checkmark_30;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(385, 15);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 50);
            this.button2.TabIndex = 21;
            this.button2.Text = "Go";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cboBlocks
            // 
            this.cboBlocks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboBlocks.FormattingEnabled = true;
            this.cboBlocks.Items.AddRange(new object[] {
            "<All>"});
            this.cboBlocks.Location = new System.Drawing.Point(579, 41);
            this.cboBlocks.Margin = new System.Windows.Forms.Padding(2);
            this.cboBlocks.Name = "cboBlocks";
            this.cboBlocks.Size = new System.Drawing.Size(101, 21);
            this.cboBlocks.TabIndex = 18;
            this.cboBlocks.SelectedIndexChanged += new System.EventHandler(this.cboBlocks_SelectedIndexChanged);
            // 
            // cboLines
            // 
            this.cboLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboLines.FormattingEnabled = true;
            this.cboLines.Items.AddRange(new object[] {
            "<All>"});
            this.cboLines.Location = new System.Drawing.Point(475, 41);
            this.cboLines.Margin = new System.Windows.Forms.Padding(2);
            this.cboLines.Name = "cboLines";
            this.cboLines.Size = new System.Drawing.Size(101, 21);
            this.cboLines.TabIndex = 17;
            this.cboLines.SelectedIndexChanged += new System.EventHandler(this.cboLines_SelectedIndexChanged);
            // 
            // btnSort
            // 
            this.btnSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSort.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnSort.FlatAppearance.BorderSize = 2;
            this.btnSort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSort.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnSort.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnSort.Location = new System.Drawing.Point(883, 15);
            this.btnSort.Margin = new System.Windows.Forms.Padding(2);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(142, 24);
            this.btnSort.TabIndex = 16;
            this.btnSort.Text = "Ordina per Efficienza";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 82);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1028, 35);
            this.panel2.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1028, 35);
            this.label4.TabIndex = 0;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblTo.Location = new System.Drawing.Point(262, 43);
            this.lblTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(0, 15);
            this.lblTo.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(104, 44);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(0, 15);
            this.lblFrom.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(231, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "A:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 50);
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
            this.label1.Location = new System.Drawing.Point(22, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "EFFICIENZA PER MACCHINA";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RepMachines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 462);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RepMachines";
            this.Text = "Machines";
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.ComboBox cboBlocks;
        private System.Windows.Forms.ComboBox cboLines;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnLineTot;
        private System.Windows.Forms.Button btnSquadraTot;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.ComboBox cboMedia;
        private System.Windows.Forms.Button btnMediaTot;
        private System.Windows.Forms.CheckBox cbGreen;
        private System.Windows.Forms.CheckBox cbMostra;
        private System.Windows.Forms.CheckBox cbRed;
        private System.Windows.Forms.CheckBox cbYellow;
    }
    }