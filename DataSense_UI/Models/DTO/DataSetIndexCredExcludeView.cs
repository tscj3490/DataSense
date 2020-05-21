using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DataSetIndexCredExcludeView
    {
        public int id { get; set; }

        public int credId { get; set; }

        public string directoryExclude { get; set; }

        public Nullable<DateTime> createdOn { get; set; }

        public int? createdBy { get; set; }

        public string createdByUser { get; set; }

        public Nullable<DateTime> modifiedOn { get; set; }

        public int? modifiedBy { get; set; }
    }
    public class DataSetIndexCredExcludeDirPost
    {

        [Required]
        public int credId { get; set; }
        [Required]
        public string directoryExclude { get; set; }
    }
    public class DataSetIndexCredExcludeDirPatch
    {

        [Required]
        [MinLength(2, ErrorMessage = "Path to Exclude must be at least 2 characters")]
        public string directoryExclude { get; set; }
    }
}