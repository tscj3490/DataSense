using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
  
    public class IndexDailyView
    {
        public int id { get; set; }

        public int dataSetIndexId { get; set; }

        public int hourToRun { get; set; }
    }
    public class IndexDailySchedulePost
    {
        //public int id { get; set; }

        public int hourToRun { get; set; }

    }
 
    public class IndexDailySchedulePatch
    {
        //public int id { get; set; }

        public int hourToRun { get; set; }
    }

}