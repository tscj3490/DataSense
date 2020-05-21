using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DashboardView
    {
        public int noOfTiers { get; set; }
        public int noOfServers { get; set; }
        public int noOfDatabases { get; set; }
        public string currentlyScanning { get; set; }
        public int piiTotalFound { get; set; }
        public List<PIIServerRecordView> servers { get; set; }
        public List<PIIServerRecordView> databases { get; set; }

        public List<PollerHealthView> pollers { get; set;  }

    }
    public class PIIServerRecordView
    {
        public int dataSetIndexCredId { get; set; }
        public string computerName { get; set; }
        public string domainName { get; set; }
        public int piiFound { get; set; }
    }
}