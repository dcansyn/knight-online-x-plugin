using System.ComponentModel.DataAnnotations;

namespace KO.Core.Constants.Query
{
    public enum ConditionType
    {
        /// <summary>
        /// AND
        /// </summary>
        [Display(Name = "AND")]
        And,

        /// <summary>
        /// OR
        /// </summary>
        [Display(Name = "OR")]
        Or
    }
}
