using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class IndexHourlyView
    {
        public int id { get; set; }

        public int dataSetIndexId { get; set; }

        public int hourlyOccurrence { get; set; }
    }
    public class IndexHourlySchedulePost
    {
        public int hourlyOccurrence { get; set; }

    }
    public class IndexHourlySchedulePatch
    {
        public int hourlyOccurrence { get; set; }
    }
}