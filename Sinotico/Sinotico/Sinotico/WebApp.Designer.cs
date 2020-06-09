namespace Sinotico
    {
    partial class WebApp
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.fpnNav = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnForw = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnRel = new System.Windows.Forms.Button();
            this.txtLink = new System.Windows.Forms.TextBox();
            this.fpnNav.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 52);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(800, 398);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser1_Navigated);
            // 
            // fpnNav
            // 
            this.fpnNav.BackColor = System.Drawing.Color.White;
            this.fpnNav.Controls.Add(this.btnBack);
            this.fpnNav.Controls.Add(this.btnForw);
            this.fpnNav.Controls.Add(this.btnHome);
            this.fpnNav.Controls.Add(this.btnRel);
            this.fpnNav.Controls.Add(this.txtLink);
            this.fpnNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.fpnNav.Location = new System.Drawing.Point(0, 0);
            this.fpnNav.Name = "fpnNav";
            this.fpnNav.Size = new System.Drawing.Size(800, 52);
            this.fpnNav.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImage = global::Sinotico.Properties.Resources.back_icon_32;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Location = new System.Drawing.Point(3, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(46, 49);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "<";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnForw
            // 
            this.btnForw.BackgroundImage = global::Sinotico.Properties.Resources.nav_forw_30;
            this.btnForw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnForw.FlatAppearance.BorderSize = 0;
            this.btnForw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnForw.Location = new System.Drawing.Point(55, 3);
            this.btnForw.Name = "btnForw";
            this.btnForw.Size = new System.Drawing.Size(46, 49);
            this.btnForw.TabIndex = 1;
            this.btnForw.Text = ">";
            this.btnForw.UseVisualStyleBackColor = true;
            this.btnForw.Click += new System.EventHandler(this.btnForw_Click);
            // 
            // btnHome
            // 
            this.btnHome.FlatAppearance.BorderSize = 0;
            this.btnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHome.Image = global::Sinotico.Properties.Resources.structural_30;
            this.btnHome.Location = new System.Drawing.Point(107, 3);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(46, 49);
            this.btnHome.TabIndex = 4;
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnRel
            // 
            this.btnRel.FlatAppearance.BorderSize = 0;
            this.btnRel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRel.Image = global::Sinotico.Properties.Resources.web_redir_30;
            this.btnRel.Location = new System.Drawing.Point(159, 3);
            this.btnRel.Name = "btnRel";
            this.btnRel.Size = new System.Drawing.Size(75, 49);
            this.btnRel.TabIndex = 3;
            this.btnRel.UseVisualStyleBackColor = true;
            this.btnRel.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtLink
            // 
            this.txtLink.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLink.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLink.Location = new System.Drawing.Point(240, 3);
            this.txtLink.Name = "txtLink";
            this.txtLink.Size = new System.Drawing.Size(553, 24);
            this.txtLink.TabIndex = 2;
            // 
            // WebApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.fpnNav);
            this.Name = "WebApp";
            this.Text = "WebApp";
            this.Load += new System.EventHandler(this.WebApp_Load);
            this.fpnNav.ResumeLayout(false);
            this.fpnNav.PerformLayout();
            this.ResumeLayout(false);

            }

        #endregion
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.FlowLayoutPanel fpnNav;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForw;
        private System.Windows.Forms.Button btnRel;
        private System.Windows.Forms.TextBox txtLink;
        private System.Windows.Forms.Button btnHome;
        }
    }