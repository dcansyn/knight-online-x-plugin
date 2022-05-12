using KO.Core.Constants.Query;
using KO.Core.Extensions;

namespace KO.Core.Models.Query
{
    public class Parameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public OperatorType OperatorType { get; set; }
        public EnumModel Operator => OperatorType.Get();
        public ConditionType ConditionType { get; set; }
        public EnumModel Condition => ConditionType.Get();

        public Parameter(string name, object value, OperatorType operatorType = OperatorType.Equal, ConditionType conditionType = ConditionType.And)
        {
            Name = name;
            Value = value;
            OperatorType = operatorType;
            ConditionType = conditionType;
        }
    }
}
