using KO.Core.Helpers.Message;
using KO.Core.Helpers.Storage;
using KO.Core.Models.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KO.UI.Extensions
{
    public static class SettingsExtensions
    {
        public static bool SaveSettings(this Control formControl)
        {
            try
            {
                var settingsFile = Path.Combine(Environment.CurrentDirectory, "Data", "Settings", $"{formControl.GetType().Name.Replace("Form", "")}.json");

                var data = JsonConvert.SerializeObject(ReadControls(formControl));
                StorageHelper.Write(settingsFile, data);
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.Send(ex);
                return false;
            }
        }

        public static bool ReadSettings(this Control formControl)
        {
            try
            {
                var settingsFile = Path.Combine(Environment.CurrentDirectory, "Data", "Settings", $"{formControl.GetType().Name.Replace("Form", "")}.json");

                if (!StorageHelper.Exists(settingsFile))
                    return true;

                var data = JsonConvert.DeserializeObject<List<FormSettingModel>>(StorageHelper.Read(settingsFile));
                CollectControls(formControl, data);
                return true;
            }
            catch (Exception ex)
            {
                MessageHelper.Send(ex);
                return false;
            }
        }

        private static List<FormSettingModel> ReadControls(this Control formControl)
        {
            var items = new List<FormSettingModel>();
            var controls = formControl.Controls.OfType<Control>().ToList();

            foreach (var control in controls)
            {
                var data = new FormSettingModel()
                {
                    Name = control.Name,
                    Type = control.GetType().Name,
                    TabIndex = control.TabIndex,
                    Value = new List<FormSettingValueModel>()
                };

                switch (control)
                {
                    case TextBox textBox:
                        data.Value.Add(new FormSettingValueModel()
                        {
                            Data = textBox.Text
                        });
                        break;

                    case ComboBox comboBox:
                        data.Value.AddRange(comboBox.Items
                            .Cast<object>()
                            .Select((x, i) => new FormSettingValueModel()
                            {
                                Index = i,
                                Data = x.ToString(),
                                Selected = i == comboBox.SelectedIndex
                            }).ToList());
                        break;

                    case CheckBox checkBox:
                        data.Value.Add(new FormSettingValueModel()
                        {
                            Data = checkBox.Checked
                        });
                        break;

                    case RadioButton radioButton:
                        data.Value.Add(new FormSettingValueModel()
                        {
                            Data = radioButton.Checked
                        });
                        break;

                    case TrackBar trackBar:
                        data.Value.Add(new FormSettingValueModel()
                        {
                            Data = trackBar.Value
                        });
                        break;

                    case CheckedListBox checkedListBox:
                        data.Value.AddRange(checkedListBox.Items
                            .Cast<object>()
                            .Select((x, i) => new FormSettingValueModel()
                            {
                                Index = i,
                                Data = x.ToString(),
                                Checked = checkedListBox.GetItemChecked(i)
                            }).ToList());
                        break;

                    case ListBox listBox:
                        data.Value.AddRange(listBox.Items
                            .Cast<object>()
                            .Select((x, i) => new FormSettingValueModel()
                            {
                                Index = i,
                                Data = x.ToString(),
                                Selected = i == listBox.SelectedIndex
                            }).ToList());
                        break;

                    case ListView listView:
                        data.Value.AddRange(listView.Items
                            .Cast<ListViewItem>()
                            .Select((x, i) => new FormSettingValueModel()
                            {
                                Index = i,
                                Data = x.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(s => s.Text).ToList(),
                                Checked = x.Checked,
                                Selected = x.Selected
                            }).ToList());
                        break;

                    case TreeView treeView:
                        data.Value.AddRange(GetTreeViewSettings(treeView.Nodes));
                        break;
                }

                if (control.HasChildren)
                    data.Children = control.ReadControls();

                items.Add(data);
            }

            return items;
        }

        private static void CollectControls(this Control formControl, List<FormSettingModel> data)
        {
            var controls = formControl.Controls.OfType<Control>().ToList();

            foreach (var control in controls)
            {
                var setting = data.FirstOrDefault(x => x.Name == control.Name && x.Type == control.GetType().Name);
                if (setting == null)
                    continue;

                switch (control)
                {
                    case TextBox textBox:
                        textBox.Text = setting.Value[0].Data.ToString();
                        break;

                    case ComboBox comboBox:
                        if (control.AccessibleName == "Generate")
                        {
                            var comboBoxSelected = setting.Value.FirstOrDefault(x => x.Selected);
                            if (comboBox.Items.Count > 0)
                                comboBox.SelectedIndex = comboBoxSelected != null && comboBox.Items.Count > comboBoxSelected.Index ? comboBoxSelected.Index : 0;
                        }
                        else
                        {
                            comboBox.Items.Clear();
                            foreach (var item in setting.Value)
                            {
                                comboBox.Items.Add(item.Data);

                                if (item.Selected)
                                    comboBox.SelectedIndex = item.Index <= 0 ? 0 : item.Index;
                            }
                        }
                        break;

                    case CheckBox checkBox:
                        if (control.AccessibleName != "Generate")
                            checkBox.Checked = Convert.ToBoolean(setting.Value[0].Data);
                        break;

                    case RadioButton radioButton:
                        radioButton.Checked = Convert.ToBoolean(setting.Value[0].Data);
                        break;

                    case TrackBar trackBar:
                        trackBar.Value = Convert.ToInt32(setting.Value[0].Data);
                        break;

                    case CheckedListBox checkedListBox:
                        if (control.AccessibleName == "Generate")
                        {
                            foreach (var item in setting.Value)
                            {
                                if (checkedListBox.Items.Count < item.Index) continue;

                                if (item.Checked)
                                    checkedListBox.SetItemChecked(item.Index, item.Checked);

                                if (item.Selected)
                                    checkedListBox.SelectedIndex = item.Index <= 0 ? 0 : item.Index;
                            }
                        }
                        else
                        {
                            checkedListBox.Items.Clear();

                            foreach (var item in setting.Value)
                            {
                                checkedListBox.Items.Add(item.Data);

                                if (item.Checked)
                                    checkedListBox.SetItemChecked(item.Index, item.Checked);

                                if (item.Selected)
                                    checkedListBox.SelectedIndex = item.Index <= 0 ? 0 : item.Index;
                            }
                        }
                        break;

                    case ListBox listBox:
                        if (control.AccessibleName == "Generate")
                        {
                            foreach (var item in setting.Value)
                            {
                                if (listBox.Items.Count < item.Index) continue;

                                if (item.Selected)
                                    listBox.SelectedIndex = item.Index <= 0 ? 0 : item.Index;
                            }
                        }
                        else
                        {
                            listBox.Items.Clear();

                            foreach (var item in setting.Value)
                            {
                                listBox.Items.Add(item.Data);

                                if (item.Selected)
                                    listBox.SelectedIndex = item.Index <= 0 ? 0 : item.Index;
                            }
                        }
                        break;

                    case ListView listView:
                        if (control.AccessibleName == "Generate")
                        {
                            for (int i = 0; i < setting.Value.Count; i++)
                            {
                                var value = setting.Value[i];

                                if (listView.Items.Count < value.Index) continue;

                                if (value.Checked)
                                    listView.Items[i].Checked = value.Checked;

                                if (value.Selected)
                                    listView.Items[i].Selected = value.Selected;
                            }
                        }
                        else
                        {
                            listView.Items.Clear();

                            for (int i = 0; i < setting.Value.Count; i++)
                            {
                                var value = setting.Value[i];

                                listView.Items.Add(new ListViewItem(((JArray)value.Data).Select(x => x.ToString()).ToArray()));

                                value.Index = i;

                                if (value.Checked)
                                    listView.Items[i].Checked = value.Checked;

                                if (value.Selected)
                                    listView.Items[i].Selected = value.Selected;
                            }
                        }
                        break;

                    case TreeView treeView:
                        treeView.CollectTreeView(setting.Value, control.AccessibleName == "Generate");
                        break;
                }

                if (control.HasChildren && setting.Children?.Count > 0)
                    control.CollectControls(setting.Children);
            }
        }

        private static void CollectTreeView(this TreeView treeView, List<FormSettingValueModel> values, bool generate = false)
        {
            if (generate)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    var item = values[i];
                    treeView.Nodes[i].Checked = item.Checked;

                    if (item.Children?.Count > 0)
                        treeView.CollectTreeView(item.Children, treeView.AccessibleName == "Generate");
                }
            }
            else
            {
                treeView.Nodes.Clear();
                treeView.Nodes.AddRange(AddTreeNode(values));
            }
        }

        private static List<FormSettingValueModel> GetTreeViewSettings(TreeNodeCollection nodes)
        {
            var result = new List<FormSettingValueModel>();

            foreach (TreeNode node in nodes)
                result.Add(new FormSettingValueModel()
                {
                    Index = node.Index,
                    Data = node.Text,
                    Checked = node.Checked,
                    Children = node.Nodes.Count > 0 ? GetTreeViewSettings(node.Nodes) : null
                });

            return result;
        }

        private static TreeNode[] AddTreeNode(List<FormSettingValueModel> values)
        {
            var nodes = new List<TreeNode>();
            for (int i = 0; i < values.Count; i++)
            {
                var item = values[i];
                var node = new TreeNode(item.Data.ToString())
                {
                    Checked = item.Checked
                };

                if (item.Children?.Count > 0)
                    node.Nodes.AddRange(AddTreeNode(item.Children));

                nodes.Add(node);
            }
            return nodes.ToArray();
        }
    }
}
