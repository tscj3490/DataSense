using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class MachineStatusView
    {
        public int machineCredStatusID { get; set; }
        public int dataSetIndexCredID { get; set; }

        public DateTime scanStarted { get; set; }

        public Nullable<DateTime> scanEnded { get; set; }


    }
}