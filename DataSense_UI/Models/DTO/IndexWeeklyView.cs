using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class IndexWeeklyView
    {
        public int id { get; set; }

        public int dataSetIndexId { get; set; }

        public string daysToRun { get; set; }

        public int hourToRun { get; set; }
    }
    public class IndexWeeklySchedulePost
    {
        //public int dataSetIndexId { get; set; }

        public string daysToRun { get; set; }

        public int hourToRun { get; set; }
    }
    public class IndexWeeklySchedulePatch
    {
        public int dataSetIndexId { get; set; }

        public string daysToRun { get; set; }

        public int hourToRun { get; set; }
    }
}