using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class RegDataType
    {
        public int id { get; set; }
        public int dataSetIndexCredId { get; set; }
        public string regularExpression { get; set; }
        public string dataTypeDesc { get; set; }
        public bool active { get; set; }
        public Nullable<DateTime> createdOn { get; set; }
        public int? createdBy { get; set; }
        public Nullable<DateTime> modifiedOn { get; set; }
        public int? modifiedBy { get; set; }
    }
    public class predefinedDataTypeView
    {
        public int id { get; set; }
        public int credID { get; set; }

        public string dataTypeDesc { get; set; }

        public bool active { get; set; }
    }
   public class DataSetIndexCredRegPredPost
    {
        public string typeDesc { get; set; }

        public bool active { get; set; }
    }
    public class RegDataTypePost
    {
        [Required]
        public string regularExpression { get; set; }

        [Required]
        public string dataTypeDesc { get; set; }
    }

    public class RegDataTypePatch
    {
        [Required]
        public string regularExpression { get; set; }

        [Required]
        public string dataTypeDesc { get; set; }
        public bool active { get; set; }
    }
}