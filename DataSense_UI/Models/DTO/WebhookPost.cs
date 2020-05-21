using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class WebhookPost
    {   
        [Required]
        public int DataSetIndexExpID { get; set; }
        [Required]
        public string HookURL { get; set; }
        [Required]
        public string HookName { get; set; }

        public string HookAuthorization { get; set; }
    }
    public class WebhookPatch
    {
        public int id { get; set; }
        [Required]
        public int DataSetIndexExpID { get; set; }
        [Required]
        public string HookURL { get; set; }
        [Required]
        public string HookName { get; set; }
        public string HookAuthorization { get; set; }
    }
}