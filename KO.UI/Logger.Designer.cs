namespace KO.UI
{
    partial class Logger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logger));
            this.GbLogger = new System.Windows.Forms.GroupBox();
            this.ChkReadSend = new System.Windows.Forms.CheckBox();
            this.ChkReadReceive = new System.Windows.Forms.CheckBox();
            this.BtnSendAll = new System.Windows.Forms.Button();
            this.BtnSend = new System.Windows.Forms.Button();
            this.LblTargetZ = new System.Windows.Forms.Label();
            this.TxtTargetZ = new System.Windows.Forms.TextBox();
            this.LblTargetY = new System.Windows.Forms.Label();
            this.TxtTargetY = new System.Windows.Forms.TextBox();
            this.LblTargetX = new System.Windows.Forms.Label();
            this.TxtTargetX = new System.Windows.Forms.TextBox();
            this.TxtTargetId = new System.Windows.Forms.TextBox();
            this.LblTargetId = new System.Windows.Forms.Label();
            this.LblCharacterZ = new System.Windows.Forms.Label();
            this.TxtCharacterZ = new System.Windows.Forms.TextBox();
            this.LblCharacterY = new System.Windows.Forms.Label();
            this.TxtCharacterY = new System.Windows.Forms.TextBox();
            this.LblCharacterX = new System.Windows.Forms.Label();
            this.TxtCharacterX = new System.Windows.Forms.TextBox();
            this.TxtCharacterId = new System.Windows.Forms.TextBox();
            this.LblCharacterId = new System.Windows.Forms.Label();
            this.BtnClear = new System.Windows.Forms.Button();
            this.TxtPacket = new System.Windows.Forms.TextBox();
            this.ChkSelectAll = new System.Windows.Forms.CheckBox();
            this.LvPackets = new System.Windows.Forms.ListView();
            this.ClPacketTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClPacketHex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClPacketType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClPacketCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChkLstPackets = new System.Windows.Forms.CheckedListBox();
            this.GbLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbLogger
            // 
            this.GbLogger.Controls.Add(this.ChkReadSend);
            this.GbLogger.Controls.Add(this.ChkReadReceive);
            this.GbLogger.Controls.Add(this.BtnSendAll);
            this.GbLogger.Controls.Add(this.BtnSend);
            this.GbLogger.Controls.Add(this.LblTargetZ);
            this.GbLogger.Controls.Add(this.TxtTargetZ);
            this.GbLogger.Controls.Add(this.LblTargetY);
            this.GbLogger.Controls.Add(this.TxtTargetY);
            this.GbLogger.Controls.Add(this.LblTargetX);
            this.GbLogger.Controls.Add(this.TxtTargetX);
            this.GbLogger.Controls.Add(this.TxtTargetId);
            this.GbLogger.Controls.Add(this.LblTargetId);
            this.GbLogger.Controls.Add(this.LblCharacterZ);
            this.GbLogger.Controls.Add(this.TxtCharacterZ);
            this.GbLogger.Controls.Add(this.LblCharacterY);
            this.GbLogger.Controls.Add(this.TxtCharacterY);
            this.GbLogger.Controls.Add(this.LblCharacterX);
            this.GbLogger.Controls.Add(this.TxtCharacterX);
            this.GbLogger.Controls.Add(this.TxtCharacterId);
            this.GbLogger.Controls.Add(this.LblCharacterId);
            this.GbLogger.Controls.Add(this.BtnClear);
            this.GbLogger.Controls.Add(this.TxtPacket);
            this.GbLogger.Controls.Add(this.ChkSelectAll);
            this.GbLogger.Controls.Add(this.LvPackets);
            this.GbLogger.Controls.Add(this.ChkLstPackets);
            this.GbLogger.Location = new System.Drawing.Point(12, 10);
            this.GbLogger.Name = "GbLogger";
            this.GbLogger.Size = new System.Drawing.Size(1080, 497);
            this.GbLogger.TabIndex = 1;
            this.GbLogger.TabStop = false;
            // 
            // ChkReadSend
            // 
            this.ChkReadSend.AutoSize = true;
            this.ChkReadSend.ForeColor = System.Drawing.Color.DarkRed;
            this.ChkReadSend.Location = new System.Drawing.Point(133, 13);
            this.ChkReadSend.Name = "ChkReadSend";
            this.ChkReadSend.Size = new System.Drawing.Size(51, 17);
            this.ChkReadSend.TabIndex = 56;
            this.ChkReadSend.Text = "Send";
            this.ChkReadSend.UseVisualStyleBackColor = true;
            // 
            // ChkReadReceive
            // 
            this.ChkReadReceive.AutoSize = true;
            this.ChkReadReceive.ForeColor = System.Drawing.Color.DarkGreen;
            this.ChkReadReceive.Location = new System.Drawing.Point(6, 13);
            this.ChkReadReceive.Name = "ChkReadReceive";
            this.ChkReadReceive.Size = new System.Drawing.Size(66, 17);
            this.ChkReadReceive.TabIndex = 55;
            this.ChkReadReceive.Text = "Receive";
            this.ChkReadReceive.UseVisualStyleBackColor = true;
            // 
            // BtnSendAll
            // 
            this.BtnSendAll.Location = new System.Drawing.Point(1000, 439);
            this.BtnSendAll.Name = "BtnSendAll";
            this.BtnSendAll.Size = new System.Drawing.Size(74, 50);
            this.BtnSendAll.TabIndex = 54;
            this.BtnSendAll.Text = "Send (X)";
            this.BtnSendAll.UseVisualStyleBackColor = true;
            this.BtnSendAll.Click += new System.EventHandler(this.BtnSendAll_Click);
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(1000, 353);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(74, 80);
            this.BtnSend.TabIndex = 53;
            this.BtnSend.Text = "Send";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // LblTargetZ
            // 
            this.LblTargetZ.AutoSize = true;
            this.LblTargetZ.Location = new System.Drawing.Point(945, 472);
            this.LblTargetZ.Name = "LblTargetZ";
            this.LblTargetZ.Size = new System.Drawing.Size(17, 13);
            this.LblTargetZ.TabIndex = 52;
            this.LblTargetZ.Text = "Z:";
            // 
            // TxtTargetZ
            // 
            this.TxtTargetZ.Location = new System.Drawing.Point(963, 469);
            this.TxtTargetZ.Name = "TxtTargetZ";
            this.TxtTargetZ.ReadOnly = true;
            this.TxtTargetZ.Size = new System.Drawing.Size(31, 20);
            this.TxtTargetZ.TabIndex = 51;
            this.TxtTargetZ.Text = "0000";
            // 
            // LblTargetY
            // 
            this.LblTargetY.AutoSize = true;
            this.LblTargetY.Location = new System.Drawing.Point(890, 472);
            this.LblTargetY.Name = "LblTargetY";
            this.LblTargetY.Size = new System.Drawing.Size(17, 13);
            this.LblTargetY.TabIndex = 50;
            this.LblTargetY.Text = "Y:";
            // 
            // TxtTargetY
            // 
            this.TxtTargetY.Location = new System.Drawing.Point(908, 469);
            this.TxtTargetY.Name = "TxtTargetY";
            this.TxtTargetY.ReadOnly = true;
            this.TxtTargetY.Size = new System.Drawing.Size(31, 20);
            this.TxtTargetY.TabIndex = 49;
            this.TxtTargetY.Text = "0000";
            // 
            // LblTargetX
            // 
            this.LblTargetX.AutoSize = true;
            this.LblTargetX.Location = new System.Drawing.Point(835, 472);
            this.LblTargetX.Name = "LblTargetX";
            this.LblTargetX.Size = new System.Drawing.Size(17, 13);
            this.LblTargetX.TabIndex = 48;
            this.LblTargetX.Text = "X:";
            // 
            // TxtTargetX
            // 
            this.TxtTargetX.Location = new System.Drawing.Point(853, 469);
            this.TxtTargetX.Name = "TxtTargetX";
            this.TxtTargetX.ReadOnly = true;
            this.TxtTargetX.Size = new System.Drawing.Size(31, 20);
            this.TxtTargetX.TabIndex = 47;
            this.TxtTargetX.Text = "0000";
            // 
            // TxtTargetId
            // 
            this.TxtTargetId.Location = new System.Drawing.Point(796, 469);
            this.TxtTargetId.Name = "TxtTargetId";
            this.TxtTargetId.ReadOnly = true;
            this.TxtTargetId.Size = new System.Drawing.Size(35, 20);
            this.TxtTargetId.TabIndex = 46;
            this.TxtTargetId.Text = "0000";
            this.TxtTargetId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblTargetId
            // 
            this.LblTargetId.AutoSize = true;
            this.LblTargetId.Location = new System.Drawing.Point(742, 472);
            this.LblTargetId.Name = "LblTargetId";
            this.LblTargetId.Size = new System.Drawing.Size(53, 13);
            this.LblTargetId.TabIndex = 45;
            this.LblTargetId.Text = "Target Id:";
            // 
            // LblCharacterZ
            // 
            this.LblCharacterZ.AutoSize = true;
            this.LblCharacterZ.Location = new System.Drawing.Point(409, 472);
            this.LblCharacterZ.Name = "LblCharacterZ";
            this.LblCharacterZ.Size = new System.Drawing.Size(17, 13);
            this.LblCharacterZ.TabIndex = 44;
            this.LblCharacterZ.Text = "Z:";
            // 
            // TxtCharacterZ
            // 
            this.TxtCharacterZ.Location = new System.Drawing.Point(427, 469);
            this.TxtCharacterZ.Name = "TxtCharacterZ";
            this.TxtCharacterZ.ReadOnly = true;
            this.TxtCharacterZ.Size = new System.Drawing.Size(31, 20);
            this.TxtCharacterZ.TabIndex = 43;
            this.TxtCharacterZ.Text = "0000";
            // 
            // LblCharacterY
            // 
            this.LblCharacterY.AutoSize = true;
            this.LblCharacterY.Location = new System.Drawing.Point(354, 472);
            this.LblCharacterY.Name = "LblCharacterY";
            this.LblCharacterY.Size = new System.Drawing.Size(17, 13);
            this.LblCharacterY.TabIndex = 42;
            this.LblCharacterY.Text = "Y:";
            // 
            // TxtCharacterY
            // 
            this.TxtCharacterY.Location = new System.Drawing.Point(372, 469);
            this.TxtCharacterY.Name = "TxtCharacterY";
            this.TxtCharacterY.ReadOnly = true;
            this.TxtCharacterY.Size = new System.Drawing.Size(31, 20);
            this.TxtCharacterY.TabIndex = 41;
            this.TxtCharacterY.Text = "0000";
            // 
            // LblCharacterX
            // 
            this.LblCharacterX.AutoSize = true;
            this.LblCharacterX.Location = new System.Drawing.Point(299, 472);
            this.LblCharacterX.Name = "LblCharacterX";
            this.LblCharacterX.Size = new System.Drawing.Size(17, 13);
            this.LblCharacterX.TabIndex = 40;
            this.LblCharacterX.Text = "X:";
            // 
            // TxtCharacterX
            // 
            this.TxtCharacterX.Location = new System.Drawing.Point(317, 469);
            this.TxtCharacterX.Name = "TxtCharacterX";
            this.TxtCharacterX.ReadOnly = true;
            this.TxtCharacterX.Size = new System.Drawing.Size(31, 20);
            this.TxtCharacterX.TabIndex = 39;
            this.TxtCharacterX.Text = "0000";
            // 
            // TxtCharacterId
            // 
            this.TxtCharacterId.Location = new System.Drawing.Point(258, 469);
            this.TxtCharacterId.Name = "TxtCharacterId";
            this.TxtCharacterId.ReadOnly = true;
            this.TxtCharacterId.Size = new System.Drawing.Size(35, 20);
            this.TxtCharacterId.TabIndex = 38;
            this.TxtCharacterId.Text = "0000";
            this.TxtCharacterId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblCharacterId
            // 
            this.LblCharacterId.AutoSize = true;
            this.LblCharacterId.Location = new System.Drawing.Point(189, 472);
            this.LblCharacterId.Name = "LblCharacterId";
            this.LblCharacterId.Size = new System.Drawing.Size(68, 13);
            this.LblCharacterId.TabIndex = 37;
            this.LblCharacterId.Text = "Character Id:";
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.SystemColors.Window;
            this.BtnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClear.Location = new System.Drawing.Point(1018, 18);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(53, 23);
            this.BtnClear.TabIndex = 32;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = false;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // TxtPacket
            // 
            this.TxtPacket.Location = new System.Drawing.Point(191, 353);
            this.TxtPacket.MaxLength = 9999999;
            this.TxtPacket.Multiline = true;
            this.TxtPacket.Name = "TxtPacket";
            this.TxtPacket.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TxtPacket.Size = new System.Drawing.Size(803, 110);
            this.TxtPacket.TabIndex = 31;
            // 
            // ChkSelectAll
            // 
            this.ChkSelectAll.AutoSize = true;
            this.ChkSelectAll.Location = new System.Drawing.Point(169, 38);
            this.ChkSelectAll.Name = "ChkSelectAll";
            this.ChkSelectAll.Size = new System.Drawing.Size(15, 14);
            this.ChkSelectAll.TabIndex = 30;
            this.ChkSelectAll.UseVisualStyleBackColor = true;
            this.ChkSelectAll.CheckedChanged += new System.EventHandler(this.ChkSelectAll_CheckedChanged);
            // 
            // LvPackets
            // 
            this.LvPackets.BackColor = System.Drawing.SystemColors.Window;
            this.LvPackets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ClPacketTime,
            this.ClPacketHex,
            this.ClPacketType,
            this.ClPacketCode});
            this.LvPackets.ForeColor = System.Drawing.Color.Black;
            this.LvPackets.FullRowSelect = true;
            this.LvPackets.GridLines = true;
            this.LvPackets.HideSelection = false;
            this.LvPackets.Location = new System.Drawing.Point(191, 13);
            this.LvPackets.Name = "LvPackets";
            this.LvPackets.Size = new System.Drawing.Size(883, 334);
            this.LvPackets.TabIndex = 26;
            this.LvPackets.UseCompatibleStateImageBehavior = false;
            this.LvPackets.View = System.Windows.Forms.View.Details;
            this.LvPackets.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LvPackets_MouseClick);
            this.LvPackets.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LvPackets_MouseDoubleClick);
            // 
            // ClPacketTime
            // 
            this.ClPacketTime.Text = "Time";
            // 
            // ClPacketHex
            // 
            this.ClPacketHex.Text = "Hex";
            this.ClPacketHex.Width = 47;
            // 
            // ClPacketType
            // 
            this.ClPacketType.Text = "Type";
            this.ClPacketType.Width = 54;
            // 
            // ClPacketCode
            // 
            this.ClPacketCode.Text = "Code";
            this.ClPacketCode.Width = 662;
            // 
            // ChkLstPackets
            // 
            this.ChkLstPackets.FormattingEnabled = true;
            this.ChkLstPackets.Location = new System.Drawing.Point(6, 35);
            this.ChkLstPackets.Name = "ChkLstPackets";
            this.ChkLstPackets.Size = new System.Drawing.Size(179, 454);
            this.ChkLstPackets.TabIndex = 14;
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 521);
            this.Controls.Add(this.GbLogger);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Logger";
            this.Text = "Logger";
            this.Load += new System.EventHandler(this.Logger_Load);
            this.GbLogger.ResumeLayout(false);
            this.GbLogger.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbLogger;
        private System.Windows.Forms.CheckBox ChkReadSend;
        private System.Windows.Forms.CheckBox ChkReadReceive;
        private System.Windows.Forms.Button BtnSendAll;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Label LblTargetZ;
        private System.Windows.Forms.TextBox TxtTargetZ;
        private System.Windows.Forms.Label LblTargetY;
        private System.Windows.Forms.TextBox TxtTargetY;
        private System.Windows.Forms.Label LblTargetX;
        private System.Windows.Forms.TextBox TxtTargetX;
        private System.Windows.Forms.TextBox TxtTargetId;
        private System.Windows.Forms.Label LblTargetId;
        private System.Windows.Forms.Label LblCharacterZ;
        private System.Windows.Forms.TextBox TxtCharacterZ;
        private System.Windows.Forms.Label LblCharacterY;
        private System.Windows.Forms.TextBox TxtCharacterY;
        private System.Windows.Forms.Label LblCharacterX;
        private System.Windows.Forms.TextBox TxtCharacterX;
        private System.Windows.Forms.TextBox TxtCharacterId;
        private System.Windows.Forms.Label LblCharacterId;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.TextBox TxtPacket;
        private System.Windows.Forms.CheckBox ChkSelectAll;
        internal System.Windows.Forms.ListView LvPackets;
        private System.Windows.Forms.ColumnHeader ClPacketTime;
        internal System.Windows.Forms.ColumnHeader ClPacketHex;
        private System.Windows.Forms.ColumnHeader ClPacketType;
        internal System.Windows.Forms.ColumnHeader ClPacketCode;
        internal System.Windows.Forms.CheckedListBox ChkLstPackets;
    }
}