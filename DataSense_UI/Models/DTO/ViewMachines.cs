using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class ViewMachines
    {
        public int id { get; set; }
        public int dataSetIndexId { get; set; }
        public string computerName { get; set; }
        public string domainName { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public bool isWmi { get; set; }
        public bool active { get; set; }
        public bool isDatabase { get; set; }

        public string databaseType { get; set; }
        public int piiCount { get; set; }

        public MachineStatusView machineStatus { get; set; }

        public Nullable<DateTime> createdOn { get; set; }
        public int? createdBy { get; set; }
        public Nullable<DateTime> modifiedOn { get; set; }
        public int? modifiedBy { get; set; }
    }

    public class ViewMachinesPost
    {

        [Required]
        public string computerName { get; set; }
        public string domainName { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public bool isWmi { get; set; }
        public bool isDatabase { get; set; }
        public string databaseType { get; set;  }
    }

    public class ViewMachinesPatch
    {
        
        public string computerName { get; set; }
        public string domainName { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public bool isWmi { get; set; } 
        public bool isDatabase { get; set; } 
        public bool active { get; set; } 




        public string databaseType { get; set; }
    }
}