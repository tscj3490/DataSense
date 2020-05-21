using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace DataSense_UI.Models.DTO
{
    public class SearchPost
    {
        [Required]
        public string MatchString { get; set; }
        [RegularExpression("^[0-9]*$", ErrorMessage = "Match Factor must be numeric")]
        public double Likelihood { get; set; }
        public int dataSetIndexId { get; set; }
        public int pageNo { get; set; }
        public int noOfRecords { get; set; }
    }

    public class PatternSearchPost
    {

        public string matchPattern { get; set; }

        public int dataSetIndexId { get; set; }

    }

    public class MatchPairView
    {
       
        public double JaccardIndex { get; set; }
        public ShingleDataView MatchingRecord { get; set; }

    }

    public class LocationView
    {
        public string location { get; set; }

        public ShingleDataView patternRecord { get; set; }
    }

    public class ShingleDataView
    {
        public int RecordID { get; set; }
        public int DataSetID { get; set; }
        public string RefID { get; set; }
        public string IndexColumn { get; set; }

    }



}