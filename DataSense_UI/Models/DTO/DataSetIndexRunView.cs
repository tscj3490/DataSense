using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DataSetIndexRunView
    {
        public int id { get; set; }
        public DateTime createdOn { get; set; }
        public int? createdBy { get; set; }
    }
    public class DataSetIndexRunDetailView
    {
        public int id { get; set; }
        public DateTime createdOn { get; set; }
        public int? createdBy { get; set; }

        public IList<StatusLog> status { get; set; }

        public IList<IndexLog> indexLog { get; set; }
    }
    public class StatusLog
    {
        public int id { get; set; }

        public string name { get; set; }

        public DateTime createdOn { get; set; }
    }

    public class IndexLog
    {
        public int id { get; set; }

        public string desc { get; set; }

        public DateTime createdOn { get; set; }
    }
}