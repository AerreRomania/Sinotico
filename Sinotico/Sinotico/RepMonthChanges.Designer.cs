namespace Sinotico
    {
    partial class RepMonthChanges
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSquadraTot = new System.Windows.Forms.Button();
            this.btnLineTot = new System.Windows.Forms.Button();
            this.lblTo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblFrom = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_articolo_total = new System.Windows.Forms.Label();
            this.lbl_colore_total = new System.Windows.Forms.Label();
            this.lbl_taglia_total = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AllowUserToResizeColumns = false;
            this.dgvReport.AllowUserToResizeRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvReport.ColumnHeadersHeight = 50;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.GridColor = System.Drawing.Color.Black;
            this.dgvReport.Location = new System.Drawing.Point(0, 104);
            this.dgvReport.Margin = new System.Windows.Forms.Padding(2);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.RowTemplate.Height = 28;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(926, 422);
            this.dgvReport.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lbl_taglia_total);
            this.panel1.Controls.Add(this.lbl_colore_total);
            this.panel1.Controls.Add(this.lbl_articolo_total);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnSquadraTot);
            this.panel1.Controls.Add(this.btnLineTot);
            this.panel1.Controls.Add(this.lblTo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.btnReload);
            this.panel1.Controls.Add(this.lblFrom);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(926, 104);
            this.panel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.SteelBlue;
            this.button1.Location = new System.Drawing.Point(829, 39);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 24);
            this.button1.TabIndex = 32;
            this.button1.Text = "Taglia";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSquadraTot
            // 
            this.btnSquadraTot.BackColor = System.Drawing.Color.White;
            this.btnSquadraTot.FlatAppearance.BorderColor = System.Drawing.Color.LightBlue;
            this.btnSquadraTot.FlatAppearance.BorderSize = 2;
            this.btnSquadraTot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSquadraTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.btnSquadraTot.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnSquadraTot.Location = new System.Drawing.Point(724, 39);
            this.btnSquadraTot.Margin = new System.Windows.Forms.Padding(2);
            this.btnSquadraTot.Name = "btnSquadraTot";
            this.btnSquadraTot.Size = new System.Drawing.Size(100, 24);
            this.btnSquadraTot.TabIndex = 31;
            this.btnSquadraTot.Text = "Colore";
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
            this.btnLineTot.Location = new System.Drawing.Point(620, 39);
            this.btnLineTot.Margin = new System.Windows.Forms.Padding(2);
            this.btnLineTot.Name = "btnLineTot";
            this.btnLineTot.Size = new System.Drawing.Size(100, 24);
            this.btnLineTot.TabIndex = 30;
            this.btnLineTot.Text = "Articolo";
            this.btnLineTot.UseVisualStyleBackColor = false;
            this.btnLineTot.Click += new System.EventHandler(this.btnLineTot_Click);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblTo.Location = new System.Drawing.Point(201, 59);
            this.lblTo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(0, 15);
            this.lblTo.TabIndex = 26;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(174, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "A:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(502, 15);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Anno:";
            // 
            // cboYear
            // 
            this.cboYear.BackColor = System.Drawing.Color.White;
            this.cboYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(504, 37);
            this.cboYear.Margin = new System.Windows.Forms.Padding(2);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(92, 34);
            this.cboYear.TabIndex = 23;
            // 
            // btnReload
            // 
            this.btnReload.BackColor = System.Drawing.Color.White;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btnReload.ForeColor = System.Drawing.Color.LightBlue;
            this.btnReload.Image = global::Sinotico.Properties.Resources.checkmark_30;
            this.btnReload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReload.Location = new System.Drawing.Point(387, 28);
            this.btnReload.Margin = new System.Windows.Forms.Padding(2);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(79, 50);
            this.btnReload.TabIndex = 22;
            this.btnReload.Text = "Go";
            this.btnReload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblFrom.Location = new System.Drawing.Point(82, 58);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(0, 15);
            this.lblFrom.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Anno:";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Bisque;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(365, 35);
            this.label1.TabIndex = 1;
            this.label1.Text = "NUMERO CAMBIO ARTICOLO";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_articolo_total
            // 
            this.lbl_articolo_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_articolo_total.ForeColor = System.Drawing.Color.Black;
            this.lbl_articolo_total.Location = new System.Drawing.Point(620, 70);
            this.lbl_articolo_total.Name = "lbl_articolo_total";
            this.lbl_articolo_total.Size = new System.Drawing.Size(100, 20);
            this.lbl_articolo_total.TabIndex = 33;
            this.lbl_articolo_total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_colore_total
            // 
            this.lbl_colore_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_colore_total.ForeColor = System.Drawing.Color.Black;
            this.lbl_colore_total.Location = new System.Drawing.Point(724, 70);
            this.lbl_colore_total.Name = "lbl_colore_total";
            this.lbl_colore_total.Size = new System.Drawing.Size(100, 20);
            this.lbl_colore_total.TabIndex = 34;
            this.lbl_colore_total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_taglia_total
            // 
            this.lbl_taglia_total.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_taglia_total.ForeColor = System.Drawing.Color.Black;
            this.lbl_taglia_total.Location = new System.Drawing.Point(829, 70);
            this.lbl_taglia_total.Name = "lbl_taglia_total";
            this.lbl_taglia_total.Size = new System.Drawing.Size(100, 20);
            this.lbl_taglia_total.TabIndex = 35;
            this.lbl_taglia_total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RepMonthChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 526);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RepMonthChanges";
            this.ShowIcon = false;
            this.Text = "RepMonthChanges";
            this.Load += new System.EventHandler(this.RepMonthChanges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSquadraTot;
        private System.Windows.Forms.Button btnLineTot;
        private System.Windows.Forms.Label lbl_taglia_total;
        private System.Windows.Forms.Label lbl_colore_total;
        private System.Windows.Forms.Label lbl_articolo_total;
    }
    }