using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class PollerHealthView
    {
        public int processStartID { get; set; }
        public string pollerKey { get; set; }
        public DateTime processStart { get; set; }
        public DateTime processLastHealthCheck { get; set; }
        public string cpuUsage { get; set; }
        public string ramUsage { get; set; }
    }
}