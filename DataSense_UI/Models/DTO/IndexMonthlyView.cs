using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class IndexMonthlyView
    {
        public int id { get; set; }

        public int dataSetIndexId { get; set; }

        public DateTime monthDay { get; set; }
    }
    public class IndexMonthlySchedulePost
    {
        public DateTime monthDay { get; set; }
    }
}