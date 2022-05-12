using System.Collections.Generic;

namespace KO.Core.Models.Settings
{
    public class FormSettingModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int TabIndex { get; set; }
        public List<FormSettingValueModel> Value { get; set; }
        public List<FormSettingModel> Children { get; set; }
    }

    public class FormSettingValueModel
    {
        public int Index { get; set; }
        public object Data { get; set; }
        public bool Checked { get; set; }
        public bool Selected { get; set; }
        public List<FormSettingValueModel> Children { get; set; }
    }
}
