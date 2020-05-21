using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class MachineNotificationView
    {
     
        public int id { get; set;  }
        public int dataSetIndexCredId { get; set;  }
        public string emailTo { get; set;  }
        public int createdBy { get; set;  }
        public Nullable<DateTime> createdOn { get; set; }
        
        public Nullable<DateTime> modifiedOn { get; set;  }
        
        public int? modifiedBy { get; set; }

    }
}