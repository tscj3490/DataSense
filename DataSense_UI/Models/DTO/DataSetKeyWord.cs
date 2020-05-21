using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DataSetKeyWord
    {
        public int id { get; set; }

        public int dataSetIndexId { get; set; }

        public string keyWord { get; set; }

        public int createdBy { get; set; }

        public Nullable<DateTime> createdOn { get; set; }

        public int? modifiedBy { get; set; }

        public Nullable<DateTime> modifiedOn { get; set; }
    }
    public class DataSetKeyWordPatch
    {
        public int? keyWordId { get; set; }

        public string keyWord { get; set; }
        public int dataSetIndexId { get; set; }
    }

    public class DataSetKeywordPost
    {
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Data Set Index must be numeric")]
        public int dataSetIndexId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Keyword must be at least 2 characters")]
        public string keyWord { get; set; }
    }

}