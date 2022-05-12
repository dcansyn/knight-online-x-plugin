using System.ComponentModel.DataAnnotations;

namespace KO.Core.Constants.Query
{
    public enum OperatorType
    {
        /// <summary>
        /// =
        /// </summary>
        [Display(Name = "=")]
        Equal,

        /// <summary>
        /// &lt;&gt;
        /// </summary>
        [Display(Name = "<>")]
        NotEqual,

        /// <summary>
        /// &gt;
        /// </summary>
        [Display(Name = ">")]
        Greater,

        /// <summary>
        /// &lt;
        /// </summary>
        [Display(Name = "<")]
        Less,

        /// <summary>
        /// &gt;=
        /// </summary>
        [Display(Name = ">=")]
        GreaterEqual,

        /// <summary>
        /// &lt;=
        /// </summary>
        [Display(Name = "<=")]
        LessEqual,

        /// <summary>
        /// LIKE
        /// </summary>
        [Display(Name = "LIKE", GroupName = "Like")]
        Like,

        /// <summary>
        /// IN 
        /// </summary>
        [Display(Name = "IN")]
        In,

        /// <summary>
        /// IS NULL 
        /// </summary>
        [Display(Name = "IS NULL", GroupName = "NullSyntax")]
        IsNull,

        /// <summary>
        /// IS NOT NULL 
        /// </summary>
        [Display(Name = "IS NOT NULL", GroupName = "NullSyntax")]
        IsNotNull
    }
}
