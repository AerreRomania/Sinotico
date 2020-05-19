//namespace Sinotico
//    {
//    partial class FrmDataBase
//        {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//            {
//            if (disposing && (components != null))
//                {
//                components.Dispose();
//                }
//            base.Dispose(disposing);
//            }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//            {
//            this.tabPage7 = new System.Windows.Forms.TabPage();
//            this.btnModifyRsn = new System.Windows.Forms.Button();
//            this.groupBox7 = new System.Windows.Forms.GroupBox();
//            this.btnAddRsn = new System.Windows.Forms.Button();
//            this.label7 = new System.Windows.Forms.Label();
//            this.txtReasons = new System.Windows.Forms.TextBox();
//            this.btnRemoveRsn = new System.Windows.Forms.Button();
//            this.lstReasons = new System.Windows.Forms.ListBox();
//            this.tabPage6 = new System.Windows.Forms.TabPage();
//            this.btnAddMac = new System.Windows.Forms.Button();
//            this.btnModifyMac = new System.Windows.Forms.Button();
//            this.groupBox6 = new System.Windows.Forms.GroupBox();
//            this.cboMacFins = new System.Windows.Forms.ComboBox();
//            this.cboMacLines = new System.Windows.Forms.ComboBox();
//            this.label13 = new System.Windows.Forms.Label();
//            this.label14 = new System.Windows.Forms.Label();
//            this.cboMacBlocks = new System.Windows.Forms.ComboBox();
//            this.label12 = new System.Windows.Forms.Label();
//            this.label11 = new System.Windows.Forms.Label();
//            this.txtMacDesc = new System.Windows.Forms.TextBox();
//            this.label10 = new System.Windows.Forms.Label();
//            this.txtMacNumber = new System.Windows.Forms.TextBox();
//            this.btnRemoveMac = new System.Windows.Forms.Button();
//            this.lstMacs = new System.Windows.Forms.ListBox();
//            this.tabPage5 = new System.Windows.Forms.TabPage();
//            this.btnModifyLine = new System.Windows.Forms.Button();
//            this.groupBox5 = new System.Windows.Forms.GroupBox();
//            this.btnAddLine = new System.Windows.Forms.Button();
//            this.label6 = new System.Windows.Forms.Label();
//            this.txtLineNumber = new System.Windows.Forms.TextBox();
//            this.btnRemoveLine = new System.Windows.Forms.Button();
//            this.lstLines = new System.Windows.Forms.ListBox();
//            this.tabPage4 = new System.Windows.Forms.TabPage();
//            this.btnModifyFin = new System.Windows.Forms.Button();
//            this.groupBox4 = new System.Windows.Forms.GroupBox();
//            this.label9 = new System.Windows.Forms.Label();
//            this.txtFinDesc = new System.Windows.Forms.TextBox();
//            this.btnAddFin = new System.Windows.Forms.Button();
//            this.label8 = new System.Windows.Forms.Label();
//            this.txtFinNumber = new System.Windows.Forms.TextBox();
//            this.btnRemoveFin = new System.Windows.Forms.Button();
//            this.lstFins = new System.Windows.Forms.ListBox();
//            this.tabPage3 = new System.Windows.Forms.TabPage();
//            this.btnModifyBlock = new System.Windows.Forms.Button();
//            this.groupBox3 = new System.Windows.Forms.GroupBox();
//            this.txtAddBlock = new System.Windows.Forms.Button();
//            this.label5 = new System.Windows.Forms.Label();
//            this.txtBlock = new System.Windows.Forms.TextBox();
//            this.btnRemoveBlock = new System.Windows.Forms.Button();
//            this.lstBlocks = new System.Windows.Forms.ListBox();
//            this.tabPage2 = new System.Windows.Forms.TabPage();
//            this.btnModifyArt = new System.Windows.Forms.Button();
//            this.groupBox2 = new System.Windows.Forms.GroupBox();
//            this.btnAddArt = new System.Windows.Forms.Button();
//            this.label4 = new System.Windows.Forms.Label();
//            this.txtArt = new System.Windows.Forms.TextBox();
//            this.btnRemoveArt = new System.Windows.Forms.Button();
//            this.lstArts = new System.Windows.Forms.ListBox();
//            this.tabPage1 = new System.Windows.Forms.TabPage();
//            this.btnModifyShift = new System.Windows.Forms.Button();
//            this.groupBox1 = new System.Windows.Forms.GroupBox();
//            this.ndNumber = new System.Windows.Forms.NumericUpDown();
//            this.label3 = new System.Windows.Forms.Label();
//            this.label1 = new System.Windows.Forms.Label();
//            this.btnAddShift = new System.Windows.Forms.Button();
//            this.mtxtTo = new System.Windows.Forms.MaskedTextBox();
//            this.label2 = new System.Windows.Forms.Label();
//            this.mtxtFrom = new System.Windows.Forms.MaskedTextBox();
//            this.btnRemoveShift = new System.Windows.Forms.Button();
//            this.lstShifts = new System.Windows.Forms.ListView();
//            this.tabControl1 = new System.Windows.Forms.TabControl();
//            this.tabPage7.SuspendLayout();
//            this.groupBox7.SuspendLayout();
//            this.tabPage6.SuspendLayout();
//            this.groupBox6.SuspendLayout();
//            this.tabPage5.SuspendLayout();
//            this.groupBox5.SuspendLayout();
//            this.tabPage4.SuspendLayout();
//            this.groupBox4.SuspendLayout();
//            this.tabPage3.SuspendLayout();
//            this.groupBox3.SuspendLayout();
//            this.tabPage2.SuspendLayout();
//            this.groupBox2.SuspendLayout();
//            this.tabPage1.SuspendLayout();
//            this.groupBox1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ndNumber)).BeginInit();
//            this.tabControl1.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // tabPage7
//            // 
//            this.tabPage7.Controls.Add(this.btnModifyRsn);
//            this.tabPage7.Controls.Add(this.groupBox7);
//            this.tabPage7.Controls.Add(this.btnRemoveRsn);
//            this.tabPage7.Controls.Add(this.lstReasons);
//            this.tabPage7.Location = new System.Drawing.Point(4, 29);
//            this.tabPage7.Name = "tabPage7";
//            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage7.Size = new System.Drawing.Size(507, 570);
//            this.tabPage7.TabIndex = 6;
//            this.tabPage7.Text = "Reasons";
//            this.tabPage7.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyRsn
//            // 
//            this.btnModifyRsn.Location = new System.Drawing.Point(244, 239);
//            this.btnModifyRsn.Name = "btnModifyRsn";
//            this.btnModifyRsn.Size = new System.Drawing.Size(114, 33);
//            this.btnModifyRsn.TabIndex = 34;
//            this.btnModifyRsn.Text = "Modify";
//            this.btnModifyRsn.UseVisualStyleBackColor = true;
//            // 
//            // groupBox7
//            // 
//            this.groupBox7.Controls.Add(this.btnAddRsn);
//            this.groupBox7.Controls.Add(this.label7);
//            this.groupBox7.Controls.Add(this.txtReasons);
//            this.groupBox7.Location = new System.Drawing.Point(22, 23);
//            this.groupBox7.Name = "groupBox7";
//            this.groupBox7.Size = new System.Drawing.Size(357, 179);
//            this.groupBox7.TabIndex = 33;
//            this.groupBox7.TabStop = false;
//            this.groupBox7.Text = "Add article parameters";
//            // 
//            // btnAddRsn
//            // 
//            this.btnAddRsn.Location = new System.Drawing.Point(36, 115);
//            this.btnAddRsn.Name = "btnAddRsn";
//            this.btnAddRsn.Size = new System.Drawing.Size(114, 33);
//            this.btnAddRsn.TabIndex = 25;
//            this.btnAddRsn.Text = "Add";
//            this.btnAddRsn.UseVisualStyleBackColor = true;
//            // 
//            // label7
//            // 
//            this.label7.AutoSize = true;
//            this.label7.Location = new System.Drawing.Point(32, 46);
//            this.label7.Name = "label7";
//            this.label7.Size = new System.Drawing.Size(89, 20);
//            this.label7.TabIndex = 15;
//            this.label7.Text = "Description";
//            // 
//            // txtReasons
//            // 
//            this.txtReasons.Location = new System.Drawing.Point(36, 69);
//            this.txtReasons.Name = "txtReasons";
//            this.txtReasons.Size = new System.Drawing.Size(277, 26);
//            this.txtReasons.TabIndex = 14;
//            // 
//            // btnRemoveRsn
//            // 
//            this.btnRemoveRsn.Location = new System.Drawing.Point(364, 239);
//            this.btnRemoveRsn.Name = "btnRemoveRsn";
//            this.btnRemoveRsn.Size = new System.Drawing.Size(114, 33);
//            this.btnRemoveRsn.TabIndex = 32;
//            this.btnRemoveRsn.Text = "Remove";
//            this.btnRemoveRsn.UseVisualStyleBackColor = true;
//            // 
//            // lstReasons
//            // 
//            this.lstReasons.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstReasons.FormattingEnabled = true;
//            this.lstReasons.ItemHeight = 20;
//            this.lstReasons.Location = new System.Drawing.Point(3, 323);
//            this.lstReasons.Name = "lstReasons";
//            this.lstReasons.Size = new System.Drawing.Size(501, 244);
//            this.lstReasons.TabIndex = 31;
//            // 
//            // tabPage6
//            // 
//            this.tabPage6.Controls.Add(this.btnAddMac);
//            this.tabPage6.Controls.Add(this.btnModifyMac);
//            this.tabPage6.Controls.Add(this.groupBox6);
//            this.tabPage6.Controls.Add(this.btnRemoveMac);
//            this.tabPage6.Controls.Add(this.lstMacs);
//            this.tabPage6.Location = new System.Drawing.Point(4, 29);
//            this.tabPage6.Name = "tabPage6";
//            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage6.Size = new System.Drawing.Size(507, 570);
//            this.tabPage6.TabIndex = 5;
//            this.tabPage6.Text = "Machines";
//            this.tabPage6.UseVisualStyleBackColor = true;
//            // 
//            // btnAddMac
//            // 
//            this.btnAddMac.Location = new System.Drawing.Point(27, 237);
//            this.btnAddMac.Name = "btnAddMac";
//            this.btnAddMac.Size = new System.Drawing.Size(114, 33);
//            this.btnAddMac.TabIndex = 25;
//            this.btnAddMac.Text = "Add";
//            this.btnAddMac.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyMac
//            // 
//            this.btnModifyMac.Location = new System.Drawing.Point(150, 237);
//            this.btnModifyMac.Name = "btnModifyMac";
//            this.btnModifyMac.Size = new System.Drawing.Size(114, 33);
//            this.btnModifyMac.TabIndex = 26;
//            this.btnModifyMac.Text = "Modify";
//            this.btnModifyMac.UseVisualStyleBackColor = true;
//            // 
//            // groupBox6
//            // 
//            this.groupBox6.Controls.Add(this.cboMacFins);
//            this.groupBox6.Controls.Add(this.cboMacLines);
//            this.groupBox6.Controls.Add(this.label13);
//            this.groupBox6.Controls.Add(this.label14);
//            this.groupBox6.Controls.Add(this.cboMacBlocks);
//            this.groupBox6.Controls.Add(this.label12);
//            this.groupBox6.Controls.Add(this.label11);
//            this.groupBox6.Controls.Add(this.txtMacDesc);
//            this.groupBox6.Controls.Add(this.label10);
//            this.groupBox6.Controls.Add(this.txtMacNumber);
//            this.groupBox6.Location = new System.Drawing.Point(17, 15);
//            this.groupBox6.Name = "groupBox6";
//            this.groupBox6.Size = new System.Drawing.Size(466, 191);
//            this.groupBox6.TabIndex = 24;
//            this.groupBox6.TabStop = false;
//            this.groupBox6.Text = "Add machines with parameters";
//            // 
//            // cboMacFins
//            // 
//            this.cboMacFins.FormattingEnabled = true;
//            this.cboMacFins.Location = new System.Drawing.Point(295, 48);
//            this.cboMacFins.Name = "cboMacFins";
//            this.cboMacFins.Size = new System.Drawing.Size(130, 28);
//            this.cboMacFins.TabIndex = 25;
//            // 
//            // cboMacLines
//            // 
//            this.cboMacLines.FormattingEnabled = true;
//            this.cboMacLines.Location = new System.Drawing.Point(113, 115);
//            this.cboMacLines.Name = "cboMacLines";
//            this.cboMacLines.Size = new System.Drawing.Size(117, 28);
//            this.cboMacLines.TabIndex = 24;
//            // 
//            // label13
//            // 
//            this.label13.AutoSize = true;
//            this.label13.Location = new System.Drawing.Point(224, 51);
//            this.label13.Name = "label13";
//            this.label13.Size = new System.Drawing.Size(65, 20);
//            this.label13.TabIndex = 23;
//            this.label13.Text = "Finesse";
//            // 
//            // label14
//            // 
//            this.label14.AutoSize = true;
//            this.label14.Location = new System.Drawing.Point(18, 118);
//            this.label14.Name = "label14";
//            this.label14.Size = new System.Drawing.Size(39, 20);
//            this.label14.TabIndex = 21;
//            this.label14.Text = "Line";
//            // 
//            // cboMacBlocks
//            // 
//            this.cboMacBlocks.FormattingEnabled = true;
//            this.cboMacBlocks.Location = new System.Drawing.Point(324, 83);
//            this.cboMacBlocks.Name = "cboMacBlocks";
//            this.cboMacBlocks.Size = new System.Drawing.Size(117, 28);
//            this.cboMacBlocks.TabIndex = 19;
//            // 
//            // label12
//            // 
//            this.label12.AutoSize = true;
//            this.label12.Location = new System.Drawing.Point(265, 88);
//            this.label12.Name = "label12";
//            this.label12.Size = new System.Drawing.Size(48, 20);
//            this.label12.TabIndex = 18;
//            this.label12.Text = "Block";
//            // 
//            // label11
//            // 
//            this.label11.AutoSize = true;
//            this.label11.Location = new System.Drawing.Point(18, 86);
//            this.label11.Name = "label11";
//            this.label11.Size = new System.Drawing.Size(89, 20);
//            this.label11.TabIndex = 17;
//            this.label11.Text = "Description";
//            // 
//            // txtMacDesc
//            // 
//            this.txtMacDesc.Location = new System.Drawing.Point(113, 83);
//            this.txtMacDesc.Name = "txtMacDesc";
//            this.txtMacDesc.Size = new System.Drawing.Size(139, 26);
//            this.txtMacDesc.TabIndex = 16;
//            // 
//            // label10
//            // 
//            this.label10.AutoSize = true;
//            this.label10.Location = new System.Drawing.Point(18, 51);
//            this.label10.Name = "label10";
//            this.label10.Size = new System.Drawing.Size(25, 20);
//            this.label10.TabIndex = 15;
//            this.label10.Text = "Nr";
//            // 
//            // txtMacNumber
//            // 
//            this.txtMacNumber.Location = new System.Drawing.Point(113, 51);
//            this.txtMacNumber.Name = "txtMacNumber";
//            this.txtMacNumber.Size = new System.Drawing.Size(88, 26);
//            this.txtMacNumber.TabIndex = 14;
//            // 
//            // btnRemoveMac
//            // 
//            this.btnRemoveMac.Location = new System.Drawing.Point(270, 237);
//            this.btnRemoveMac.Name = "btnRemoveMac";
//            this.btnRemoveMac.Size = new System.Drawing.Size(114, 33);
//            this.btnRemoveMac.TabIndex = 23;
//            this.btnRemoveMac.Text = "Remove";
//            this.btnRemoveMac.UseVisualStyleBackColor = true;
//            // 
//            // lstMacs
//            // 
//            this.lstMacs.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstMacs.FormattingEnabled = true;
//            this.lstMacs.ItemHeight = 20;
//            this.lstMacs.Location = new System.Drawing.Point(3, 323);
//            this.lstMacs.Name = "lstMacs";
//            this.lstMacs.Size = new System.Drawing.Size(501, 244);
//            this.lstMacs.TabIndex = 22;
//            // 
//            // tabPage5
//            // 
//            this.tabPage5.Controls.Add(this.btnModifyLine);
//            this.tabPage5.Controls.Add(this.groupBox5);
//            this.tabPage5.Controls.Add(this.btnRemoveLine);
//            this.tabPage5.Controls.Add(this.lstLines);
//            this.tabPage5.Location = new System.Drawing.Point(4, 29);
//            this.tabPage5.Name = "tabPage5";
//            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage5.Size = new System.Drawing.Size(507, 570);
//            this.tabPage5.TabIndex = 4;
//            this.tabPage5.Text = "Lines";
//            this.tabPage5.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyLine
//            // 
//            this.btnModifyLine.Location = new System.Drawing.Point(244, 239);
//            this.btnModifyLine.Name = "btnModifyLine";
//            this.btnModifyLine.Size = new System.Drawing.Size(114, 33);
//            this.btnModifyLine.TabIndex = 34;
//            this.btnModifyLine.Text = "Modify";
//            this.btnModifyLine.UseVisualStyleBackColor = true;
//            // 
//            // groupBox5
//            // 
//            this.groupBox5.Controls.Add(this.btnAddLine);
//            this.groupBox5.Controls.Add(this.label6);
//            this.groupBox5.Controls.Add(this.txtLineNumber);
//            this.groupBox5.Location = new System.Drawing.Point(21, 20);
//            this.groupBox5.Name = "groupBox5";
//            this.groupBox5.Size = new System.Drawing.Size(357, 179);
//            this.groupBox5.TabIndex = 33;
//            this.groupBox5.TabStop = false;
//            this.groupBox5.Text = "Add line";
//            // 
//            // btnAddLine
//            // 
//            this.btnAddLine.Location = new System.Drawing.Point(36, 115);
//            this.btnAddLine.Name = "btnAddLine";
//            this.btnAddLine.Size = new System.Drawing.Size(114, 33);
//            this.btnAddLine.TabIndex = 25;
//            this.btnAddLine.Text = "Add";
//            this.btnAddLine.UseVisualStyleBackColor = true;
//            // 
//            // label6
//            // 
//            this.label6.AutoSize = true;
//            this.label6.Location = new System.Drawing.Point(32, 46);
//            this.label6.Name = "label6";
//            this.label6.Size = new System.Drawing.Size(65, 20);
//            this.label6.TabIndex = 15;
//            this.label6.Text = "Number";
//            // 
//            // txtLineNumber
//            // 
//            this.txtLineNumber.Location = new System.Drawing.Point(36, 69);
//            this.txtLineNumber.Name = "txtLineNumber";
//            this.txtLineNumber.Size = new System.Drawing.Size(277, 26);
//            this.txtLineNumber.TabIndex = 14;
//            // 
//            // btnRemoveLine
//            // 
//            this.btnRemoveLine.Location = new System.Drawing.Point(364, 239);
//            this.btnRemoveLine.Name = "btnRemoveLine";
//            this.btnRemoveLine.Size = new System.Drawing.Size(114, 33);
//            this.btnRemoveLine.TabIndex = 32;
//            this.btnRemoveLine.Text = "Remove";
//            this.btnRemoveLine.UseVisualStyleBackColor = true;
//            // 
//            // lstLines
//            // 
//            this.lstLines.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstLines.FormattingEnabled = true;
//            this.lstLines.ItemHeight = 20;
//            this.lstLines.Location = new System.Drawing.Point(3, 323);
//            this.lstLines.Name = "lstLines";
//            this.lstLines.Size = new System.Drawing.Size(501, 244);
//            this.lstLines.TabIndex = 31;
//            // 
//            // tabPage4
//            // 
//            this.tabPage4.Controls.Add(this.btnModifyFin);
//            this.tabPage4.Controls.Add(this.groupBox4);
//            this.tabPage4.Controls.Add(this.btnRemoveFin);
//            this.tabPage4.Controls.Add(this.lstFins);
//            this.tabPage4.Location = new System.Drawing.Point(4, 29);
//            this.tabPage4.Name = "tabPage4";
//            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage4.Size = new System.Drawing.Size(507, 570);
//            this.tabPage4.TabIndex = 3;
//            this.tabPage4.Text = "Finesses";
//            this.tabPage4.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyFin
//            // 
//            this.btnModifyFin.Location = new System.Drawing.Point(245, 242);
//            this.btnModifyFin.Name = "btnModifyFin";
//            this.btnModifyFin.Size = new System.Drawing.Size(114, 33);
//            this.btnModifyFin.TabIndex = 30;
//            this.btnModifyFin.Text = "Modify";
//            this.btnModifyFin.UseVisualStyleBackColor = true;
//            // 
//            // groupBox4
//            // 
//            this.groupBox4.Controls.Add(this.label9);
//            this.groupBox4.Controls.Add(this.txtFinDesc);
//            this.groupBox4.Controls.Add(this.btnAddFin);
//            this.groupBox4.Controls.Add(this.label8);
//            this.groupBox4.Controls.Add(this.txtFinNumber);
//            this.groupBox4.Location = new System.Drawing.Point(28, 16);
//            this.groupBox4.Name = "groupBox4";
//            this.groupBox4.Size = new System.Drawing.Size(453, 203);
//            this.groupBox4.TabIndex = 29;
//            this.groupBox4.TabStop = false;
//            this.groupBox4.Text = "Add finesse";
//            // 
//            // label9
//            // 
//            this.label9.AutoSize = true;
//            this.label9.Location = new System.Drawing.Point(29, 81);
//            this.label9.Name = "label9";
//            this.label9.Size = new System.Drawing.Size(89, 20);
//            this.label9.TabIndex = 27;
//            this.label9.Text = "Description";
//            // 
//            // txtFinDesc
//            // 
//            this.txtFinDesc.Location = new System.Drawing.Point(124, 75);
//            this.txtFinDesc.Name = "txtFinDesc";
//            this.txtFinDesc.Size = new System.Drawing.Size(277, 26);
//            this.txtFinDesc.TabIndex = 26;
//            // 
//            // btnAddFin
//            // 
//            this.btnAddFin.Location = new System.Drawing.Point(33, 136);
//            this.btnAddFin.Name = "btnAddFin";
//            this.btnAddFin.Size = new System.Drawing.Size(114, 33);
//            this.btnAddFin.TabIndex = 25;
//            this.btnAddFin.Text = "Add";
//            this.btnAddFin.UseVisualStyleBackColor = true;
//            // 
//            // label8
//            // 
//            this.label8.AutoSize = true;
//            this.label8.Location = new System.Drawing.Point(29, 49);
//            this.label8.Name = "label8";
//            this.label8.Size = new System.Drawing.Size(65, 20);
//            this.label8.TabIndex = 15;
//            this.label8.Text = "Number";
//            // 
//            // txtFinNumber
//            // 
//            this.txtFinNumber.Location = new System.Drawing.Point(124, 43);
//            this.txtFinNumber.Name = "txtFinNumber";
//            this.txtFinNumber.Size = new System.Drawing.Size(114, 26);
//            this.txtFinNumber.TabIndex = 14;
//            // 
//            // btnRemoveFin
//            // 
//            this.btnRemoveFin.Location = new System.Drawing.Point(365, 242);
//            this.btnRemoveFin.Name = "btnRemoveFin";
//            this.btnRemoveFin.Size = new System.Drawing.Size(114, 33);
//            this.btnRemoveFin.TabIndex = 28;
//            this.btnRemoveFin.Text = "Remove";
//            this.btnRemoveFin.UseVisualStyleBackColor = true;
//            // 
//            // lstFins
//            // 
//            this.lstFins.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstFins.FormattingEnabled = true;
//            this.lstFins.ItemHeight = 20;
//            this.lstFins.Location = new System.Drawing.Point(3, 323);
//            this.lstFins.Name = "lstFins";
//            this.lstFins.Size = new System.Drawing.Size(501, 244);
//            this.lstFins.TabIndex = 27;
//            // 
//            // tabPage3
//            // 
//            this.tabPage3.Controls.Add(this.btnModifyBlock);
//            this.tabPage3.Controls.Add(this.groupBox3);
//            this.tabPage3.Controls.Add(this.btnRemoveBlock);
//            this.tabPage3.Controls.Add(this.lstBlocks);
//            this.tabPage3.Location = new System.Drawing.Point(4, 29);
//            this.tabPage3.Name = "tabPage3";
//            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage3.Size = new System.Drawing.Size(507, 570);
//            this.tabPage3.TabIndex = 2;
//            this.tabPage3.Text = "Blocks";
//            this.tabPage3.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyBlock
//            // 
//            this.btnModifyBlock.Location = new System.Drawing.Point(244, 235);
//            this.btnModifyBlock.Name = "btnModifyBlock";
//            this.btnModifyBlock.Size = new System.Drawing.Size(114, 33);
//            this.btnModifyBlock.TabIndex = 30;
//            this.btnModifyBlock.Text = "Modify";
//            this.btnModifyBlock.UseVisualStyleBackColor = true;
//            // 
//            // groupBox3
//            // 
//            this.groupBox3.Controls.Add(this.txtAddBlock);
//            this.groupBox3.Controls.Add(this.label5);
//            this.groupBox3.Controls.Add(this.txtBlock);
//            this.groupBox3.Location = new System.Drawing.Point(27, 23);
//            this.groupBox3.Name = "groupBox3";
//            this.groupBox3.Size = new System.Drawing.Size(357, 179);
//            this.groupBox3.TabIndex = 29;
//            this.groupBox3.TabStop = false;
//            this.groupBox3.Text = "Add block";
//            // 
//            // txtAddBlock
//            // 
//            this.txtAddBlock.Location = new System.Drawing.Point(36, 115);
//            this.txtAddBlock.Name = "txtAddBlock";
//            this.txtAddBlock.Size = new System.Drawing.Size(114, 33);
//            this.txtAddBlock.TabIndex = 25;
//            this.txtAddBlock.Text = "Add";
//            this.txtAddBlock.UseVisualStyleBackColor = true;
//            // 
//            // label5
//            // 
//            this.label5.AutoSize = true;
//            this.label5.Location = new System.Drawing.Point(32, 46);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(48, 20);
//            this.label5.TabIndex = 15;
//            this.label5.Text = "Block";
//            // 
//            // txtBlock
//            // 
//            this.txtBlock.Location = new System.Drawing.Point(36, 69);
//            this.txtBlock.Name = "txtBlock";
//            this.txtBlock.Size = new System.Drawing.Size(277, 26);
//            this.txtBlock.TabIndex = 14;
//            // 
//            // btnRemoveBlock
//            // 
//            this.btnRemoveBlock.Location = new System.Drawing.Point(364, 235);
//            this.btnRemoveBlock.Name = "btnRemoveBlock";
//            this.btnRemoveBlock.Size = new System.Drawing.Size(114, 33);
//            this.btnRemoveBlock.TabIndex = 28;
//            this.btnRemoveBlock.Text = "Remove";
//            this.btnRemoveBlock.UseVisualStyleBackColor = true;
//            // 
//            // lstBlocks
//            // 
//            this.lstBlocks.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstBlocks.FormattingEnabled = true;
//            this.lstBlocks.ItemHeight = 20;
//            this.lstBlocks.Location = new System.Drawing.Point(3, 323);
//            this.lstBlocks.Name = "lstBlocks";
//            this.lstBlocks.Size = new System.Drawing.Size(501, 244);
//            this.lstBlocks.TabIndex = 27;
//            // 
//            // tabPage2
//            // 
//            this.tabPage2.Controls.Add(this.btnModifyArt);
//            this.tabPage2.Controls.Add(this.groupBox2);
//            this.tabPage2.Controls.Add(this.btnRemoveArt);
//            this.tabPage2.Controls.Add(this.lstArts);
//            this.tabPage2.Location = new System.Drawing.Point(4, 29);
//            this.tabPage2.Name = "tabPage2";
//            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage2.Size = new System.Drawing.Size(507, 570);
//            this.tabPage2.TabIndex = 1;
//            this.tabPage2.Text = "Articles";
//            this.tabPage2.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyArt
//            // 
//            this.btnModifyArt.Location = new System.Drawing.Point(244, 235);
//            this.btnModifyArt.Name = "btnModifyArt";
//            this.btnModifyArt.Size = new System.Drawing.Size(114, 33);
//            this.btnModifyArt.TabIndex = 26;
//            this.btnModifyArt.Text = "Modify";
//            this.btnModifyArt.UseVisualStyleBackColor = true;
//            this.btnModifyArt.Click += new System.EventHandler(this.btnModifyArt_Click);
//            // 
//            // groupBox2
//            // 
//            this.groupBox2.Controls.Add(this.btnAddArt);
//            this.groupBox2.Controls.Add(this.label4);
//            this.groupBox2.Controls.Add(this.txtArt);
//            this.groupBox2.Location = new System.Drawing.Point(27, 23);
//            this.groupBox2.Name = "groupBox2";
//            this.groupBox2.Size = new System.Drawing.Size(357, 179);
//            this.groupBox2.TabIndex = 24;
//            this.groupBox2.TabStop = false;
//            this.groupBox2.Text = "Add article";
//            // 
//            // btnAddArt
//            // 
//            this.btnAddArt.Location = new System.Drawing.Point(36, 115);
//            this.btnAddArt.Name = "btnAddArt";
//            this.btnAddArt.Size = new System.Drawing.Size(114, 33);
//            this.btnAddArt.TabIndex = 25;
//            this.btnAddArt.Text = "Add";
//            this.btnAddArt.UseVisualStyleBackColor = true;
//            this.btnAddArt.Click += new System.EventHandler(this.btnAddArt_Click);
//            // 
//            // label4
//            // 
//            this.label4.AutoSize = true;
//            this.label4.Location = new System.Drawing.Point(32, 46);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(53, 20);
//            this.label4.TabIndex = 15;
//            this.label4.Text = "Article";
//            // 
//            // txtArt
//            // 
//            this.txtArt.Location = new System.Drawing.Point(36, 69);
//            this.txtArt.Name = "txtArt";
//            this.txtArt.Size = new System.Drawing.Size(277, 26);
//            this.txtArt.TabIndex = 14;
//            // 
//            // btnRemoveArt
//            // 
//            this.btnRemoveArt.Location = new System.Drawing.Point(364, 235);
//            this.btnRemoveArt.Name = "btnRemoveArt";
//            this.btnRemoveArt.Size = new System.Drawing.Size(114, 33);
//            this.btnRemoveArt.TabIndex = 23;
//            this.btnRemoveArt.Text = "Remove";
//            this.btnRemoveArt.UseVisualStyleBackColor = true;
//            this.btnRemoveArt.Click += new System.EventHandler(this.btnRemoveArt_Click);
//            // 
//            // lstArts
//            // 
//            this.lstArts.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstArts.FormattingEnabled = true;
//            this.lstArts.ItemHeight = 20;
//            this.lstArts.Location = new System.Drawing.Point(3, 323);
//            this.lstArts.Name = "lstArts";
//            this.lstArts.Size = new System.Drawing.Size(501, 244);
//            this.lstArts.TabIndex = 22;
//            this.lstArts.SelectedIndexChanged += new System.EventHandler(this.lstArts_SelectedIndexChanged);
//            // 
//            // tabPage1
//            // 
//            this.tabPage1.Controls.Add(this.btnModifyShift);
//            this.tabPage1.Controls.Add(this.groupBox1);
//            this.tabPage1.Controls.Add(this.btnRemoveShift);
//            this.tabPage1.Controls.Add(this.lstShifts);
//            this.tabPage1.Location = new System.Drawing.Point(4, 29);
//            this.tabPage1.Name = "tabPage1";
//            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPage1.Size = new System.Drawing.Size(507, 570);
//            this.tabPage1.TabIndex = 0;
//            this.tabPage1.Text = "Shifts";
//            this.tabPage1.UseVisualStyleBackColor = true;
//            // 
//            // btnModifyShift
//            // 
//            this.btnModifyShift.Location = new System.Drawing.Point(234, 255);
//            this.btnModifyShift.Name = "btnModifyShift";
//            this.btnModifyShift.Size = new System.Drawing.Size(113, 34);
//            this.btnModifyShift.TabIndex = 18;
//            this.btnModifyShift.Text = "Modify";
//            this.btnModifyShift.UseVisualStyleBackColor = true;
//            // 
//            // groupBox1
//            // 
//            this.groupBox1.Controls.Add(this.ndNumber);
//            this.groupBox1.Controls.Add(this.label3);
//            this.groupBox1.Controls.Add(this.label1);
//            this.groupBox1.Controls.Add(this.btnAddShift);
//            this.groupBox1.Controls.Add(this.mtxtTo);
//            this.groupBox1.Controls.Add(this.label2);
//            this.groupBox1.Controls.Add(this.mtxtFrom);
//            this.groupBox1.Location = new System.Drawing.Point(26, 21);
//            this.groupBox1.Name = "groupBox1";
//            this.groupBox1.Size = new System.Drawing.Size(450, 196);
//            this.groupBox1.TabIndex = 17;
//            this.groupBox1.TabStop = false;
//            this.groupBox1.Text = "Add shift with parameters";
//            // 
//            // ndNumber
//            // 
//            this.ndNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.ndNumber.Location = new System.Drawing.Point(70, 42);
//            this.ndNumber.Minimum = new decimal(new int[] {
//            1,
//            0,
//            0,
//            0});
//            this.ndNumber.Name = "ndNumber";
//            this.ndNumber.Size = new System.Drawing.Size(78, 28);
//            this.ndNumber.TabIndex = 3;
//            this.ndNumber.Value = new decimal(new int[] {
//            1,
//            0,
//            0,
//            0});
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.Location = new System.Drawing.Point(227, 84);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(31, 20);
//            this.label3.TabIndex = 8;
//            this.label3.Text = "To:";
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Location = new System.Drawing.Point(35, 51);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(29, 20);
//            this.label1.TabIndex = 6;
//            this.label1.Text = "Nr:";
//            // 
//            // btnAddShift
//            // 
//            this.btnAddShift.Location = new System.Drawing.Point(39, 133);
//            this.btnAddShift.Name = "btnAddShift";
//            this.btnAddShift.Size = new System.Drawing.Size(123, 34);
//            this.btnAddShift.TabIndex = 15;
//            this.btnAddShift.Text = "Add";
//            this.btnAddShift.UseVisualStyleBackColor = true;
//            this.btnAddShift.Click += new System.EventHandler(this.btnAdd_Click_1);
//            // 
//            // mtxtTo
//            // 
//            this.mtxtTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.mtxtTo.Location = new System.Drawing.Point(264, 76);
//            this.mtxtTo.Name = "mtxtTo";
//            this.mtxtTo.Size = new System.Drawing.Size(77, 28);
//            this.mtxtTo.TabIndex = 5;
//            this.mtxtTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
//            // 
//            // label2
//            // 
//            this.label2.AutoSize = true;
//            this.label2.Location = new System.Drawing.Point(208, 50);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(50, 20);
//            this.label2.TabIndex = 7;
//            this.label2.Text = "From:";
//            // 
//            // mtxtFrom
//            // 
//            this.mtxtFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.mtxtFrom.Location = new System.Drawing.Point(264, 41);
//            this.mtxtFrom.Name = "mtxtFrom";
//            this.mtxtFrom.Size = new System.Drawing.Size(77, 28);
//            this.mtxtFrom.TabIndex = 4;
//            this.mtxtFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
//            this.mtxtFrom.ValidatingType = typeof(System.DateTime);
//            // 
//            // btnRemoveShift
//            // 
//            this.btnRemoveShift.Location = new System.Drawing.Point(353, 255);
//            this.btnRemoveShift.Name = "btnRemoveShift";
//            this.btnRemoveShift.Size = new System.Drawing.Size(123, 34);
//            this.btnRemoveShift.TabIndex = 16;
//            this.btnRemoveShift.Text = "Remove";
//            this.btnRemoveShift.UseVisualStyleBackColor = true;
//            this.btnRemoveShift.Click += new System.EventHandler(this.btnRemove_Click_1);
//            // 
//            // lstShifts
//            // 
//            this.lstShifts.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.lstShifts.Location = new System.Drawing.Point(3, 341);
//            this.lstShifts.Name = "lstShifts";
//            this.lstShifts.Size = new System.Drawing.Size(501, 226);
//            this.lstShifts.TabIndex = 14;
//            this.lstShifts.UseCompatibleStateImageBehavior = false;
//            this.lstShifts.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged_1);
//            // 
//            // tabControl1
//            // 
//            this.tabControl1.Controls.Add(this.tabPage1);
//            this.tabControl1.Controls.Add(this.tabPage2);
//            this.tabControl1.Controls.Add(this.tabPage3);
//            this.tabControl1.Controls.Add(this.tabPage4);
//            this.tabControl1.Controls.Add(this.tabPage5);
//            this.tabControl1.Controls.Add(this.tabPage6);
//            this.tabControl1.Controls.Add(this.tabPage7);
//            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tabControl1.Location = new System.Drawing.Point(0, 0);
//            this.tabControl1.Multiline = true;
//            this.tabControl1.Name = "tabControl1";
//            this.tabControl1.SelectedIndex = 0;
//            this.tabControl1.Size = new System.Drawing.Size(515, 603);
//            this.tabControl1.TabIndex = 0;
//            // 
//            // FrmDataBase
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(515, 603);
//            this.Controls.Add(this.tabControl1);
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
//            this.MaximizeBox = false;
//            this.MinimizeBox = false;
//            this.Name = "FrmDataBase";
//            this.Text = "Database";
//            this.Load += new System.EventHandler(this.FrmDataBase_Load);
//            this.tabPage7.ResumeLayout(false);
//            this.groupBox7.ResumeLayout(false);
//            this.groupBox7.PerformLayout();
//            this.tabPage6.ResumeLayout(false);
//            this.groupBox6.ResumeLayout(false);
//            this.groupBox6.PerformLayout();
//            this.tabPage5.ResumeLayout(false);
//            this.groupBox5.ResumeLayout(false);
//            this.groupBox5.PerformLayout();
//            this.tabPage4.ResumeLayout(false);
//            this.groupBox4.ResumeLayout(false);
//            this.groupBox4.PerformLayout();
//            this.tabPage3.ResumeLayout(false);
//            this.groupBox3.ResumeLayout(false);
//            this.groupBox3.PerformLayout();
//            this.tabPage2.ResumeLayout(false);
//            this.groupBox2.ResumeLayout(false);
//            this.groupBox2.PerformLayout();
//            this.tabPage1.ResumeLayout(false);
//            this.groupBox1.ResumeLayout(false);
//            this.groupBox1.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ndNumber)).EndInit();
//            this.tabControl1.ResumeLayout(false);
//            this.ResumeLayout(false);

//            }

//        #endregion
//        private System.Windows.Forms.TabPage tabPage7;
//        private System.Windows.Forms.Button btnModifyRsn;
//        private System.Windows.Forms.GroupBox groupBox7;
//        private System.Windows.Forms.Button btnAddRsn;
//        private System.Windows.Forms.Label label7;
//        private System.Windows.Forms.TextBox txtReasons;
//        private System.Windows.Forms.Button btnRemoveRsn;
//        private System.Windows.Forms.ListBox lstReasons;
//        private System.Windows.Forms.TabPage tabPage6;
//        private System.Windows.Forms.Button btnAddMac;
//        private System.Windows.Forms.Button btnModifyMac;
//        private System.Windows.Forms.GroupBox groupBox6;
//        private System.Windows.Forms.ComboBox cboMacFins;
//        private System.Windows.Forms.ComboBox cboMacLines;
//        private System.Windows.Forms.Label label13;
//        private System.Windows.Forms.Label label14;
//        private System.Windows.Forms.ComboBox cboMacBlocks;
//        private System.Windows.Forms.Label label12;
//        private System.Windows.Forms.Label label11;
//        private System.Windows.Forms.TextBox txtMacDesc;
//        private System.Windows.Forms.Label label10;
//        private System.Windows.Forms.TextBox txtMacNumber;
//        private System.Windows.Forms.Button btnRemoveMac;
//        private System.Windows.Forms.ListBox lstMacs;
//        private System.Windows.Forms.TabPage tabPage5;
//        private System.Windows.Forms.Button btnModifyLine;
//        private System.Windows.Forms.GroupBox groupBox5;
//        private System.Windows.Forms.Button btnAddLine;
//        private System.Windows.Forms.Label label6;
//        private System.Windows.Forms.TextBox txtLineNumber;
//        private System.Windows.Forms.Button btnRemoveLine;
//        private System.Windows.Forms.ListBox lstLines;
//        private System.Windows.Forms.TabPage tabPage4;
//        private System.Windows.Forms.Button btnModifyFin;
//        private System.Windows.Forms.GroupBox groupBox4;
//        private System.Windows.Forms.Label label9;
//        private System.Windows.Forms.TextBox txtFinDesc;
//        private System.Windows.Forms.Button btnAddFin;
//        private System.Windows.Forms.Label label8;
//        private System.Windows.Forms.TextBox txtFinNumber;
//        private System.Windows.Forms.Button btnRemoveFin;
//        private System.Windows.Forms.ListBox lstFins;
//        private System.Windows.Forms.TabPage tabPage3;
//        private System.Windows.Forms.Button btnModifyBlock;
//        private System.Windows.Forms.GroupBox groupBox3;
//        private System.Windows.Forms.Button txtAddBlock;
//        private System.Windows.Forms.Label label5;
//        private System.Windows.Forms.TextBox txtBlock;
//        private System.Windows.Forms.Button btnRemoveBlock;
//        private System.Windows.Forms.ListBox lstBlocks;
//        private System.Windows.Forms.TabPage tabPage2;
//        private System.Windows.Forms.Button btnModifyArt;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private System.Windows.Forms.Button btnAddArt;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.TextBox txtArt;
//        private System.Windows.Forms.Button btnRemoveArt;
//        private System.Windows.Forms.ListBox lstArts;
//        private System.Windows.Forms.TabPage tabPage1;
//        private System.Windows.Forms.Button btnModifyShift;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private System.Windows.Forms.NumericUpDown ndNumber;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.Button btnAddShift;
//        private System.Windows.Forms.MaskedTextBox mtxtTo;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.MaskedTextBox mtxtFrom;
//        private System.Windows.Forms.Button btnRemoveShift;
//        private System.Windows.Forms.ListView lstShifts;
//        private System.Windows.Forms.TabControl tabControl1;
//        }
//    }