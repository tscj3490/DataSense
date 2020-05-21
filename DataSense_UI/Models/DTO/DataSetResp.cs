using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DataSetResp
    {
        public int id { get; set; }

        public string dataSetName { get; set; }
        
        public string dataSetLookUpTable { get; set; }


        public bool active { get; set; }

        public DateTime? createdOn { get; set; }

        public int? createdBy { get; set; }

        public string createdByUser { get; set;  }

        public DateTime? modifiedOn { get; set; }


        public int? modifiedBy { get; set; }

        public string modifiedByUser { get; set;  }

    }

    public class DataSetPatch
    {
        public int datasetId { get; set; }
        public string dataSetName { get; set; }

        public string dataSetLookUpTable { get; set; }

        public bool active { get; set; }

    }
    public class DataSetPost
    {
        public string dataSetName { get; set; }

        public string dataSetLookUpTable { get; set; }

        public bool active { get; set; } = true;
    }
    public class DataSetPostView
    {
        public string dataSetName { get; set; }

        public string dataSetLookUpTable { get; set; }
        public bool active { get; set; } = true;

    }

    public class DataSetPatchView
    {
        public int id { get; set; }
        public string dataSetName { get; set; }

        public string dataSetLookUpTable { get; set; }

        public bool active { get; set; }

        public string modifiedByUser { get; set; }
        public DateTime? modifiedOn { get; set; }
    }

}