namespace StockControl
{
    partial class Login
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cbConfig = new Telerik.WinControls.UI.RadCheckBox();
            this.btnConnect = new Telerik.WinControls.UI.RadButton();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.lblServer = new Telerik.WinControls.UI.RadLabel();
            this.lblDatabase = new Telerik.WinControls.UI.RadLabel();
            this.ddlServer = new Telerik.WinControls.UI.RadDropDownList();
            this.ddlDatabase = new Telerik.WinControls.UI.RadDropDownList();
            this.cbShow = new Telerik.WinControls.UI.RadCheckBox();
            this.lbVer = new System.Windows.Forms.Label();
            this.btnUpdate = new Telerik.WinControls.UI.RadButton();
            this.office2010BlueTheme1 = new Telerik.WinControls.Themes.Office2010BlueTheme();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDatabase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbShow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(156, 109);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(126, 20);
            this.txtUser.TabIndex = 0;
            this.txtUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(156, 135);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(126, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            // 
            // radLabel2
            // 
            this.radLabel2.BackColor = System.Drawing.Color.Transparent;
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel2.Location = new System.Drawing.Point(114, 109);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(36, 18);
            this.radLabel2.TabIndex = 7;
            this.radLabel2.Text = "User :";
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.Transparent;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.Location = new System.Drawing.Point(89, 135);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(63, 18);
            this.radLabel1.TabIndex = 7;
            this.radLabel1.Text = "Password :";
            // 
            // cbConfig
            // 
            this.cbConfig.BackColor = System.Drawing.Color.Transparent;
            this.cbConfig.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConfig.Location = new System.Drawing.Point(156, 161);
            this.cbConfig.Name = "cbConfig";
            this.cbConfig.Size = new System.Drawing.Size(56, 18);
            this.cbConfig.TabIndex = 19;
            this.cbConfig.TabStop = false;
            this.cbConfig.Text = "Config";
            this.cbConfig.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.cbConfig_ToggleStateChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(72, 189);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(72, 20);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Login";
            this.btnConnect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConnect.ThemeName = "Office2010Blue";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(150, 189);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 20);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.ThemeName = "Office2010Blue";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblServer
            // 
            this.lblServer.BackColor = System.Drawing.Color.Transparent;
            this.lblServer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServer.Location = new System.Drawing.Point(104, 263);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(46, 18);
            this.lblServer.TabIndex = 7;
            this.lblServer.Text = "Server :";
            // 
            // lblDatabase
            // 
            this.lblDatabase.BackColor = System.Drawing.Color.Transparent;
            this.lblDatabase.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabase.Location = new System.Drawing.Point(87, 286);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(62, 18);
            this.lblDatabase.TabIndex = 7;
            this.lblDatabase.Text = "Database :";
            // 
            // ddlServer
            // 
            this.ddlServer.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem1.Text = "1.179.133.222,2018";
            radListDataItem2.Text = "FILE_SERVER";
            radListDataItem3.Text = "110.170.161.35";
            this.ddlServer.Items.Add(radListDataItem1);
            this.ddlServer.Items.Add(radListDataItem2);
            this.ddlServer.Items.Add(radListDataItem3);
            this.ddlServer.Location = new System.Drawing.Point(156, 261);
            this.ddlServer.Name = "ddlServer";
            this.ddlServer.Size = new System.Drawing.Size(131, 20);
            this.ddlServer.TabIndex = 4;
            this.ddlServer.ThemeName = "Office2010Blue";
            // 
            // ddlDatabase
            // 
            this.ddlDatabase.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.ddlDatabase.Location = new System.Drawing.Point(156, 286);
            this.ddlDatabase.Name = "ddlDatabase";
            this.ddlDatabase.Size = new System.Drawing.Size(131, 20);
            this.ddlDatabase.TabIndex = 5;
            this.ddlDatabase.ThemeName = "Office2010Blue";
            // 
            // cbShow
            // 
            this.cbShow.BackColor = System.Drawing.Color.Transparent;
            this.cbShow.Location = new System.Drawing.Point(287, 135);
            this.cbShow.Name = "cbShow";
            this.cbShow.Size = new System.Drawing.Size(47, 18);
            this.cbShow.TabIndex = 19;
            this.cbShow.TabStop = false;
            this.cbShow.Text = "Show";
            this.cbShow.Visible = false;
            this.cbShow.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.cbShow_ToggleStateChanged);
            // 
            // lbVer
            // 
            this.lbVer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVer.BackColor = System.Drawing.Color.Transparent;
            this.lbVer.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lbVer.Location = new System.Drawing.Point(204, 357);
            this.lbVer.Name = "lbVer";
            this.lbVer.Size = new System.Drawing.Size(198, 18);
            this.lbVer.TabIndex = 21;
            this.lbVer.Text = "version : 1.0.2";
            this.lbVer.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(238, 189);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(72, 20);
            this.btnUpdate.TabIndex = 20;
            this.btnUpdate.TabStop = false;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdate.ThemeName = "Office2010Blue";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(206)))), ((int)(((byte)(225)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(406, 378);
            this.Controls.Add(this.lbVer);
            this.Controls.Add(this.ddlDatabase);
            this.Controls.Add(this.ddlServer);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cbShow);
            this.Controls.Add(this.cbConfig);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDatabase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbShow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadCheckBox cbConfig;
        private Telerik.WinControls.UI.RadButton btnConnect;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadLabel lblServer;
        private Telerik.WinControls.UI.RadLabel lblDatabase;
        private Telerik.WinControls.UI.RadDropDownList ddlServer;
        private Telerik.WinControls.UI.RadDropDownList ddlDatabase;
        private Telerik.WinControls.UI.RadCheckBox cbShow;
        private System.Windows.Forms.Label lbVer;
        private Telerik.WinControls.UI.RadButton btnUpdate;
        private Telerik.WinControls.Themes.Office2010BlueTheme office2010BlueTheme1;
    }
}