using KO.Core.Helpers.Message;
using KO.Infrastructure.Services.Tables;
using KO.UI.Helpers;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KO.UI
{
    public partial class Editor : Form
    {
        private string LocalePath = null;
        private string TargetPath = null;
        private DataTable LocaleData = null;
        private DataTable TargetData = null;

        private readonly TableService _tableService;
        public Editor()
        {
            InitializeComponent();

            _tableService = new TableService();
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            PbGeneral.Maximum = 100;
        }

        private void BtnLocalePath_Click(object sender, EventArgs e)
        {
            var path = GameHelper.GetGameDirectoryPath();

            if (string.IsNullOrEmpty(path))
                return;

            LocalePath = Path.Combine(path, "Data");
        }

        private void BtnTargetPath_Click(object sender, EventArgs e)
        {
            var path = GameHelper.GetGameDirectoryPath();

            if (string.IsNullOrEmpty(path))
                return;

            TargetPath = Path.Combine(path, "Data");

            if (Directory.Exists(TargetPath))
                LstTables.DataSource = _tableService.GetTables(TargetPath);
        }

        private void LstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstTables.Enabled = false;
            if (!string.IsNullOrEmpty(TargetPath) || !string.IsNullOrEmpty(LstTables.Text))
                TargetData = _tableService.GetTable(Path.Combine(TargetPath, LstTables.Text));

            TxtColumns.Text = "";
            if (TargetData.Columns.Cast<DataColumn>().Any(x => x.DataType.Name == "String"))
            {
                TxtColumns.Text = string.Join(",", TargetData.Columns
                    .Cast<DataColumn>()
                    .Where(x => x.DataType.Name == "String").Select(x => x.Ordinal).ToList());
            }

            DgvTable.DataSource = TargetData;
            LstTables.Enabled = true;
        }

        private void TxtColumns_TextChanged(object sender, EventArgs e)
        {
            TxtColumns.Text = "";
        }

        private void LstTables_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnCopy_Click(sender, e);
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            BtnCopy.Enabled = false;
            if (!string.IsNullOrEmpty(LocalePath) || !string.IsNullOrEmpty(LstTables.Text))
                LocaleData = _tableService.GetTable(Path.Combine(LocalePath, LstTables.Text.Replace("_jp", "_us")));

            if (LocaleData == null)
            {
                MessageHelper.Send($"Table not found: {LstTables.Text}");
                BtnCopy.Enabled = true;
                return;
            }

            var mergeColumnIndexes = TxtColumns.Text
            .Split(',')
            .Select(x => Convert.ToInt32(x))
            .ToArray();

            for (int i = 0; i < TargetData.Rows.Count; i++)
            {
                var row = TargetData.Rows[i];
                var result = LocaleData.Rows.Cast<DataRow>().FirstOrDefault(x => x[0].ToString() == row[0].ToString());
                foreach (var index in mergeColumnIndexes)
                {
                    if (result != null)
                        row[index] = result[index];

                    PbGeneral.Value = PbGeneral.Value < PbGeneral.Maximum ? PbGeneral.Value + 1 : 0;

                    System.Windows.Forms.Application.DoEvents();
                }
            }

            _tableService.SaveTable(TargetData, Path.Combine(TargetPath, LstTables.Text));
            DgvTable.DataSource = TargetData;
            PbGeneral.Value = 100;
            BtnCopy.Enabled = true;
        }
    }
}
