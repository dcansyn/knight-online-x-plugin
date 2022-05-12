namespace KO.UI
{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.GbLocalization = new System.Windows.Forms.GroupBox();
            this.PbGeneral = new System.Windows.Forms.ProgressBar();
            this.TxtColumns = new System.Windows.Forms.TextBox();
            this.LblColumns = new System.Windows.Forms.Label();
            this.BtnCopy = new System.Windows.Forms.Button();
            this.BtnTargetPath = new System.Windows.Forms.Button();
            this.BtnLocalePath = new System.Windows.Forms.Button();
            this.DgvTable = new System.Windows.Forms.DataGridView();
            this.LstTables = new System.Windows.Forms.ListBox();
            this.GbLocalization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTable)).BeginInit();
            this.SuspendLayout();
            // 
            // GbLocalization
            // 
            this.GbLocalization.Controls.Add(this.PbGeneral);
            this.GbLocalization.Controls.Add(this.TxtColumns);
            this.GbLocalization.Controls.Add(this.LblColumns);
            this.GbLocalization.Controls.Add(this.BtnCopy);
            this.GbLocalization.Controls.Add(this.BtnTargetPath);
            this.GbLocalization.Controls.Add(this.BtnLocalePath);
            this.GbLocalization.Controls.Add(this.DgvTable);
            this.GbLocalization.Controls.Add(this.LstTables);
            this.GbLocalization.Location = new System.Drawing.Point(12, 12);
            this.GbLocalization.Name = "GbLocalization";
            this.GbLocalization.Size = new System.Drawing.Size(1218, 585);
            this.GbLocalization.TabIndex = 1;
            this.GbLocalization.TabStop = false;
            this.GbLocalization.Text = "Table Editor";
            // 
            // PbGeneral
            // 
            this.PbGeneral.Location = new System.Drawing.Point(6, 558);
            this.PbGeneral.Name = "PbGeneral";
            this.PbGeneral.Size = new System.Drawing.Size(1206, 21);
            this.PbGeneral.TabIndex = 10;
            // 
            // TxtColumns
            // 
            this.TxtColumns.Location = new System.Drawing.Point(59, 491);
            this.TxtColumns.Name = "TxtColumns";
            this.TxtColumns.Size = new System.Drawing.Size(166, 20);
            this.TxtColumns.TabIndex = 8;
            this.TxtColumns.TextChanged += new System.EventHandler(this.TxtColumns_TextChanged);
            // 
            // LblColumns
            // 
            this.LblColumns.AutoSize = true;
            this.LblColumns.Location = new System.Drawing.Point(6, 494);
            this.LblColumns.Name = "LblColumns";
            this.LblColumns.Size = new System.Drawing.Size(53, 13);
            this.LblColumns.TabIndex = 9;
            this.LblColumns.Text = "Columns :";
            // 
            // BtnCopy
            // 
            this.BtnCopy.Location = new System.Drawing.Point(6, 517);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(219, 35);
            this.BtnCopy.TabIndex = 7;
            this.BtnCopy.Text = "Copy";
            this.BtnCopy.UseVisualStyleBackColor = true;
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // BtnTargetPath
            // 
            this.BtnTargetPath.Location = new System.Drawing.Point(120, 19);
            this.BtnTargetPath.Name = "BtnTargetPath";
            this.BtnTargetPath.Size = new System.Drawing.Size(105, 35);
            this.BtnTargetPath.TabIndex = 6;
            this.BtnTargetPath.Text = "Target";
            this.BtnTargetPath.UseVisualStyleBackColor = true;
            this.BtnTargetPath.Click += new System.EventHandler(this.BtnTargetPath_Click);
            // 
            // BtnLocalePath
            // 
            this.BtnLocalePath.Location = new System.Drawing.Point(9, 19);
            this.BtnLocalePath.Name = "BtnLocalePath";
            this.BtnLocalePath.Size = new System.Drawing.Size(105, 35);
            this.BtnLocalePath.TabIndex = 5;
            this.BtnLocalePath.Text = "Locale";
            this.BtnLocalePath.UseVisualStyleBackColor = true;
            this.BtnLocalePath.Click += new System.EventHandler(this.BtnLocalePath_Click);
            // 
            // DgvTable
            // 
            this.DgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvTable.Location = new System.Drawing.Point(231, 19);
            this.DgvTable.Name = "DgvTable";
            this.DgvTable.Size = new System.Drawing.Size(981, 533);
            this.DgvTable.TabIndex = 1;
            // 
            // LstTables
            // 
            this.LstTables.FormattingEnabled = true;
            this.LstTables.Location = new System.Drawing.Point(9, 60);
            this.LstTables.Name = "LstTables";
            this.LstTables.Size = new System.Drawing.Size(216, 420);
            this.LstTables.TabIndex = 0;
            this.LstTables.SelectedIndexChanged += new System.EventHandler(this.LstTables_SelectedIndexChanged);
            this.LstTables.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LstTables_MouseDoubleClick);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 609);
            this.Controls.Add(this.GbLocalization);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Editor";
            this.Text = "Editor";
            this.Load += new System.EventHandler(this.Editor_Load);
            this.GbLocalization.ResumeLayout(false);
            this.GbLocalization.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbLocalization;
        private System.Windows.Forms.ProgressBar PbGeneral;
        private System.Windows.Forms.TextBox TxtColumns;
        private System.Windows.Forms.Label LblColumns;
        private System.Windows.Forms.Button BtnCopy;
        private System.Windows.Forms.Button BtnTargetPath;
        private System.Windows.Forms.Button BtnLocalePath;
        private System.Windows.Forms.DataGridView DgvTable;
        private System.Windows.Forms.ListBox LstTables;
    }
}