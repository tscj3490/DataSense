using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DataSetIndexView
    {
        public int id { get; set; }
        public int dataSetId { get; set; }
        public string dataSetColumns { get; set; }

        public int? limitRecords { get; set; }

        public int? shingleSize { get; set; }

        public int? rowsPerBand { get; set; }

        public char procType { get; set; }

        public string outputFileDir { get; set; }

        public bool active { get; set; }

        public string nodeUrl { get; set; }

        public int? createdBy { get; set; }

        public int recordCount { get; set; }

        public int noOfDatabases { get; set;  }

        public int noOfServers { get; set;  }

        public Nullable<DateTime> createdOn { get; set; }

        public int? modifiedBy { get; set; }

        public Nullable<DateTime> modifiedOn { get; set; }

        public string status { get; set;  }

    }
    public class DataSetIndexPatch
    {
        //public int id { get; set; }
        public int dataSetId { get; set; }
        public string dataSetColumns { get; set; }

        public int? limitRecords { get; set; }

        [Required]
        public int? shingleSize { get; set; }

        [Required]
        public int? rowsPerBand { get; set; }
        public char procType { get; set; }
        public string nodeUrl { get; set; }
        public bool active { get; set; }
    }
    public class DataSetIndexPostView
    {
        public int dataSetId { get; set; }
        public string dataSetColumns { get; set; }

        public int? limitRecords { get; set; }

        [Required]
        public int? shingleSize { get; set; }

        [Required]
        public int? rowsPerBand { get; set; }

        public string procType { get; set; }

        public string nodeUrl { get; set; }
    }
    public class DataSetIndexPatchView
    {
        public int id { get; set; }
        public int dataSetId { get; set; }
        public string dataSetColumns { get; set; }

        public int? limitRecords { get; set; }

        public int? shingleSize { get; set; }

        public int? rowsPerBand { get; set; }

        public char procType { get; set; }

        public string outputFileDir { get; set; }

        public bool active { get; set; }

        public string nodeUrl { get; set; }
        
        public string status { get; set; }
        
    }
    public class DataSetIndexStatusView
    {
        public StatusView status { get; set; }

        public string fileName { get; set; }
    }

    public class StatusView
    {
        public int id { get; set; }
        public string name { get; set; }

    }

}