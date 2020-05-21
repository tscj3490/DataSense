using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DataSetIndexExpHook
    {
        public int id { get; set; }
        public int DataSetIndexExpID { get; set; }
        public string HookURL { get; set; }
        public string HookName { get; set; }
        public string HookAuthorization { get; set; }
    }
}