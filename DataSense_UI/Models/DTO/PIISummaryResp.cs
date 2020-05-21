using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class PIISummaryResp
    {
        public int dataSetId { get; set; }
        public int dataSetIndexId { get; set; }
        public int dataSetIndexCredId { get; set; }
        public string machineName { get; set; }
        public string domainName { get; set; }
        public string databaseType { get; set; }
        public bool isDatabase { get; set; }
        public List<PIISummaryChild> dataTypeList { get; set; }
    }

    public class PIISummaryChild
    {
        public string dataTypeDesc { get; set; }

        public int piiCount { get; set; }
    }
}