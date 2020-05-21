using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class PatternRecViewSvc
    {
        public int id { get; set; }

        public int dataSetIndexCredId { get; set; }
        public PatternRegEx patternRegEx { get; set; }
        public string fileName { get; set; }

        public string previewText { get; set; }

        public Nullable<DateTime> createdOn { get; set; }
    }
    public class PatternRegEx
    {
        public int id { get; set; }

        public string dataTypeDesc { get; set; }
    }
    public class PageViewPost
    {
        public int pageNo { get; set; }

        public int noOfRecords { get; set; }
    }

    public class Reviewed
    {
        public int quarantineId { get; set; }
        public int credId { get; set; }
        public string fileName { get; set; }
        public int createdby { get; set; }
        public string createdByUser { get; set; }
        public DateTime createdOn { get; set; }
        public Nullable<int> modifiedBy { get; set; }
        public Nullable<DateTime> modifiedOn { get; set; }
    }

    public class MarkReviewsPost
    {
        public string fileName { get; set; }

    }
}