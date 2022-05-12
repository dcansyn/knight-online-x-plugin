namespace KO.UI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.MenuGeneral = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTopMost = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHide = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDivider1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuTableEditor = new System.Windows.Forms.ToolStripMenuItem();
            this.TbLogin = new System.Windows.Forms.TabControl();
            this.TbGames = new System.Windows.Forms.TabPage();
            this.GbGames = new System.Windows.Forms.GroupBox();
            this.ChkSelectAll = new System.Windows.Forms.CheckBox();
            this.BtnRefreshGame = new System.Windows.Forms.Button();
            this.BtnLoadGame = new System.Windows.Forms.Button();
            this.LvGames = new System.Windows.Forms.ListView();
            this.GameClName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GameClChrName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GameClChrClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GameClChrLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GameClChrMain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TbAccounts = new System.Windows.Forms.TabPage();
            this.GbAccounts = new System.Windows.Forms.GroupBox();
            this.BtnOpenAccount = new System.Windows.Forms.Button();
            this.BtnAddAccount = new System.Windows.Forms.Button();
            this.LvAccounts = new System.Windows.Forms.ListView();
            this.AccClDetail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AccClName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AccClUserName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AccClPassword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AccClPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TxtAccountDetail = new System.Windows.Forms.TextBox();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TopMenu.SuspendLayout();
            this.TbLogin.SuspendLayout();
            this.TbGames.SuspendLayout();
            this.GbGames.SuspendLayout();
            this.TbAccounts.SuspendLayout();
            this.GbAccounts.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuGeneral,
            this.MenuTableEditor});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(737, 24);
            this.TopMenu.TabIndex = 4;
            this.TopMenu.Text = "Menü";
            // 
            // MenuGeneral
            // 
            this.MenuGeneral.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuTopMost,
            this.MenuHide,
            this.MenuSave,
            this.MenuDivider1,
            this.MenuClose});
            this.MenuGeneral.Name = "MenuGeneral";
            this.MenuGeneral.Size = new System.Drawing.Size(59, 20);
            this.MenuGeneral.Tag = "General";
            this.MenuGeneral.Text = "General";
            // 
            // MenuTopMost
            // 
            this.MenuTopMost.Name = "MenuTopMost";
            this.MenuTopMost.Size = new System.Drawing.Size(143, 22);
            this.MenuTopMost.Text = "Top Most";
            this.MenuTopMost.CheckedChanged += new System.EventHandler(this.MenuTopMost_CheckedChanged);
            this.MenuTopMost.Click += new System.EventHandler(this.MenuTopMost_Click);
            // 
            // MenuHide
            // 
            this.MenuHide.Name = "MenuHide";
            this.MenuHide.Size = new System.Drawing.Size(143, 22);
            this.MenuHide.Text = "Hide";
            this.MenuHide.Click += new System.EventHandler(this.MenuHide_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.Size = new System.Drawing.Size(143, 22);
            this.MenuSave.Text = "Save Settings";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuDivider1
            // 
            this.MenuDivider1.Name = "MenuDivider1";
            this.MenuDivider1.Size = new System.Drawing.Size(140, 6);
            // 
            // MenuClose
            // 
            this.MenuClose.Name = "MenuClose";
            this.MenuClose.Size = new System.Drawing.Size(143, 22);
            this.MenuClose.Text = "Close";
            this.MenuClose.Click += new System.EventHandler(this.MenuClose_Click);
            // 
            // MenuTableEditor
            // 
            this.MenuTableEditor.Name = "MenuTableEditor";
            this.MenuTableEditor.Size = new System.Drawing.Size(80, 20);
            this.MenuTableEditor.Text = "Table Editor";
            this.MenuTableEditor.Click += new System.EventHandler(this.MenuTableEditor_Click);
            // 
            // TbLogin
            // 
            this.TbLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbLogin.Controls.Add(this.TbGames);
            this.TbLogin.Controls.Add(this.TbAccounts);
            this.TbLogin.Location = new System.Drawing.Point(12, 27);
            this.TbLogin.Name = "TbLogin";
            this.TbLogin.SelectedIndex = 0;
            this.TbLogin.Size = new System.Drawing.Size(713, 612);
            this.TbLogin.TabIndex = 5;
            // 
            // TbGames
            // 
            this.TbGames.Controls.Add(this.GbGames);
            this.TbGames.Location = new System.Drawing.Point(4, 22);
            this.TbGames.Name = "TbGames";
            this.TbGames.Size = new System.Drawing.Size(705, 586);
            this.TbGames.TabIndex = 2;
            this.TbGames.Text = "Games";
            // 
            // GbGames
            // 
            this.GbGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbGames.Controls.Add(this.ChkSelectAll);
            this.GbGames.Controls.Add(this.BtnRefreshGame);
            this.GbGames.Controls.Add(this.BtnLoadGame);
            this.GbGames.Controls.Add(this.LvGames);
            this.GbGames.ForeColor = System.Drawing.Color.Black;
            this.GbGames.Location = new System.Drawing.Point(3, 3);
            this.GbGames.Name = "GbGames";
            this.GbGames.Size = new System.Drawing.Size(699, 582);
            this.GbGames.TabIndex = 3;
            this.GbGames.TabStop = false;
            this.GbGames.Text = "Games";
            // 
            // ChkSelectAll
            // 
            this.ChkSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChkSelectAll.AutoSize = true;
            this.ChkSelectAll.Location = new System.Drawing.Point(88, 556);
            this.ChkSelectAll.Name = "ChkSelectAll";
            this.ChkSelectAll.Size = new System.Drawing.Size(70, 17);
            this.ChkSelectAll.TabIndex = 8;
            this.ChkSelectAll.Text = "Select All";
            this.ChkSelectAll.UseVisualStyleBackColor = true;
            this.ChkSelectAll.CheckedChanged += new System.EventHandler(this.ChkSelectAll_CheckedChanged);
            // 
            // BtnRefreshGame
            // 
            this.BtnRefreshGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRefreshGame.Location = new System.Drawing.Point(617, 550);
            this.BtnRefreshGame.Name = "BtnRefreshGame";
            this.BtnRefreshGame.Size = new System.Drawing.Size(76, 26);
            this.BtnRefreshGame.TabIndex = 7;
            this.BtnRefreshGame.Text = "Refresh";
            this.BtnRefreshGame.UseVisualStyleBackColor = true;
            this.BtnRefreshGame.Click += new System.EventHandler(this.BtnRefreshGame_Click);
            // 
            // BtnLoadGame
            // 
            this.BtnLoadGame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnLoadGame.Location = new System.Drawing.Point(6, 550);
            this.BtnLoadGame.Name = "BtnLoadGame";
            this.BtnLoadGame.Size = new System.Drawing.Size(76, 26);
            this.BtnLoadGame.TabIndex = 6;
            this.BtnLoadGame.Text = "Load";
            this.BtnLoadGame.UseVisualStyleBackColor = true;
            this.BtnLoadGame.Click += new System.EventHandler(this.BtnLoadGame_Click);
            // 
            // LvGames
            // 
            this.LvGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LvGames.BackColor = System.Drawing.SystemColors.Window;
            this.LvGames.CheckBoxes = true;
            this.LvGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GameClName,
            this.GameClChrName,
            this.GameClChrClass,
            this.GameClChrLevel,
            this.GameClChrMain});
            this.LvGames.ForeColor = System.Drawing.Color.Black;
            this.LvGames.FullRowSelect = true;
            this.LvGames.GridLines = true;
            this.LvGames.HideSelection = false;
            this.LvGames.Location = new System.Drawing.Point(6, 19);
            this.LvGames.MultiSelect = false;
            this.LvGames.Name = "LvGames";
            this.LvGames.Size = new System.Drawing.Size(687, 525);
            this.LvGames.TabIndex = 4;
            this.LvGames.UseCompatibleStateImageBehavior = false;
            this.LvGames.View = System.Windows.Forms.View.Details;
            this.LvGames.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LvGames_MouseClick);
            this.LvGames.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvGames_MouseDoubleClick);
            // 
            // GameClName
            // 
            this.GameClName.Text = "Game";
            this.GameClName.Width = 120;
            // 
            // GameClChrName
            // 
            this.GameClChrName.Text = "Character";
            this.GameClChrName.Width = 120;
            // 
            // GameClChrClass
            // 
            this.GameClChrClass.Text = "Class";
            this.GameClChrClass.Width = 120;
            // 
            // GameClChrLevel
            // 
            this.GameClChrLevel.Text = "Level";
            this.GameClChrLevel.Width = 120;
            // 
            // GameClChrMain
            // 
            this.GameClChrMain.Text = "Main";
            this.GameClChrMain.Width = 83;
            // 
            // TbAccounts
            // 
            this.TbAccounts.Controls.Add(this.GbAccounts);
            this.TbAccounts.Location = new System.Drawing.Point(4, 22);
            this.TbAccounts.Name = "TbAccounts";
            this.TbAccounts.Size = new System.Drawing.Size(705, 586);
            this.TbAccounts.TabIndex = 1;
            this.TbAccounts.Text = "Accounts";
            // 
            // GbAccounts
            // 
            this.GbAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbAccounts.Controls.Add(this.BtnOpenAccount);
            this.GbAccounts.Controls.Add(this.BtnAddAccount);
            this.GbAccounts.Controls.Add(this.LvAccounts);
            this.GbAccounts.Controls.Add(this.TxtAccountDetail);
            this.GbAccounts.ForeColor = System.Drawing.Color.Black;
            this.GbAccounts.Location = new System.Drawing.Point(3, 3);
            this.GbAccounts.Name = "GbAccounts";
            this.GbAccounts.Size = new System.Drawing.Size(699, 580);
            this.GbAccounts.TabIndex = 4;
            this.GbAccounts.TabStop = false;
            this.GbAccounts.Text = "Accounts";
            // 
            // BtnOpenAccount
            // 
            this.BtnOpenAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnOpenAccount.Location = new System.Drawing.Point(6, 548);
            this.BtnOpenAccount.Name = "BtnOpenAccount";
            this.BtnOpenAccount.Size = new System.Drawing.Size(76, 26);
            this.BtnOpenAccount.TabIndex = 12;
            this.BtnOpenAccount.Text = "Open";
            this.BtnOpenAccount.UseVisualStyleBackColor = true;
            this.BtnOpenAccount.Click += new System.EventHandler(this.BtnOpenAccount_Click);
            // 
            // BtnAddAccount
            // 
            this.BtnAddAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAddAccount.Location = new System.Drawing.Point(617, 548);
            this.BtnAddAccount.Name = "BtnAddAccount";
            this.BtnAddAccount.Size = new System.Drawing.Size(76, 26);
            this.BtnAddAccount.TabIndex = 11;
            this.BtnAddAccount.Text = "Add";
            this.BtnAddAccount.UseVisualStyleBackColor = true;
            this.BtnAddAccount.Click += new System.EventHandler(this.BtnAddAccount_Click);
            // 
            // LvAccounts
            // 
            this.LvAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LvAccounts.BackColor = System.Drawing.SystemColors.Window;
            this.LvAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.AccClDetail,
            this.AccClName,
            this.AccClUserName,
            this.AccClPassword,
            this.AccClPath});
            this.LvAccounts.ForeColor = System.Drawing.Color.Black;
            this.LvAccounts.FullRowSelect = true;
            this.LvAccounts.GridLines = true;
            this.LvAccounts.HideSelection = false;
            this.LvAccounts.Location = new System.Drawing.Point(6, 19);
            this.LvAccounts.Name = "LvAccounts";
            this.LvAccounts.ShowItemToolTips = true;
            this.LvAccounts.Size = new System.Drawing.Size(687, 523);
            this.LvAccounts.TabIndex = 10;
            this.LvAccounts.UseCompatibleStateImageBehavior = false;
            this.LvAccounts.View = System.Windows.Forms.View.Details;
            this.LvAccounts.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LvAccounts_MouseClick);
            this.LvAccounts.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvAccounts_MouseDoubleClick);
            // 
            // AccClDetail
            // 
            this.AccClDetail.Text = "Detail";
            this.AccClDetail.Width = 167;
            // 
            // AccClName
            // 
            this.AccClName.Text = "Name";
            this.AccClName.Width = 130;
            // 
            // AccClUserName
            // 
            this.AccClUserName.Text = "User Name";
            this.AccClUserName.Width = 134;
            // 
            // AccClPassword
            // 
            this.AccClPassword.Text = "User Password";
            this.AccClPassword.Width = 116;
            // 
            // AccClPath
            // 
            this.AccClPath.Text = "Path";
            this.AccClPath.Width = 112;
            // 
            // TxtAccountDetail
            // 
            this.TxtAccountDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtAccountDetail.Location = new System.Drawing.Point(88, 551);
            this.TxtAccountDetail.Name = "TxtAccountDetail";
            this.TxtAccountDetail.ReadOnly = true;
            this.TxtAccountDetail.Size = new System.Drawing.Size(523, 20);
            this.TxtAccountDetail.TabIndex = 14;
            this.TxtAccountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 651);
            this.Controls.Add(this.TopMenu);
            this.Controls.Add(this.TbLogin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.Load += new System.EventHandler(this.Login_Load);
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.TbLogin.ResumeLayout(false);
            this.TbGames.ResumeLayout(false);
            this.GbGames.ResumeLayout(false);
            this.GbGames.PerformLayout();
            this.TbAccounts.ResumeLayout(false);
            this.GbAccounts.ResumeLayout(false);
            this.GbAccounts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.MenuStrip TopMenu;
        internal System.Windows.Forms.ToolStripMenuItem MenuGeneral;
        internal System.Windows.Forms.ToolStripMenuItem MenuTopMost;
        internal System.Windows.Forms.ToolStripMenuItem MenuHide;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        internal System.Windows.Forms.ToolStripSeparator MenuDivider1;
        internal System.Windows.Forms.ToolStripMenuItem MenuClose;
        private System.Windows.Forms.ToolStripMenuItem MenuTableEditor;
        internal System.Windows.Forms.TabControl TbLogin;
        private System.Windows.Forms.TabPage TbAccounts;
        internal System.Windows.Forms.GroupBox GbAccounts;
        internal System.Windows.Forms.Button BtnOpenAccount;
        internal System.Windows.Forms.Button BtnAddAccount;
        internal System.Windows.Forms.ListView LvAccounts;
        private System.Windows.Forms.ColumnHeader AccClName;
        internal System.Windows.Forms.ColumnHeader AccClUserName;
        private System.Windows.Forms.ColumnHeader AccClPassword;
        private System.Windows.Forms.ColumnHeader AccClPath;
        private System.Windows.Forms.TabPage TbGames;
        internal System.Windows.Forms.GroupBox GbGames;
        internal System.Windows.Forms.Button BtnRefreshGame;
        internal System.Windows.Forms.Button BtnLoadGame;
        internal System.Windows.Forms.ListView LvGames;
        internal System.Windows.Forms.ColumnHeader GameClName;
        internal System.Windows.Forms.ColumnHeader GameClChrName;
        internal System.Windows.Forms.ColumnHeader GameClChrClass;
        private System.Windows.Forms.ColumnHeader GameClChrLevel;
        private System.Windows.Forms.ColumnHeader GameClChrMain;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.ColumnHeader AccClDetail;
        private System.Windows.Forms.CheckBox ChkSelectAll;
        private System.Windows.Forms.TextBox TxtAccountDetail;
    }
}