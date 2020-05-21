using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class UserProfileResp
    {
        public int id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailAddress { get; set; }

        public bool active { get; set; }

        public int createdBy { get; set; }

        public DateTime createdOn { get; set; }

        public int? modifiedBy { get; set; }

        public Nullable<DateTime> modifiedOn { get; set; }

        public string createdByUser { get; set; }

        public API api { get; set; }
    }
    public class API
    {
        public string key { get; set; }
        public bool? active { get; set; }
    }

    public class UserProfilePatchView
    {
        public int id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailAddress { get; set; }

        public bool active { get; set; }

        public int? modifiedBy { get; set; }

        public Nullable<DateTime> modifiedOn { get; set; }

        public string modifiedByUser { get; set; }

    }

    public class UserProfilePatch
    {


        public string firstName { get; set; }

        public string lastName { get; set; }

        public bool active { get; set; }
    }

    public class UserProfileAdd
    {
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string emailAddress { get; set; }

        public bool active { get; set; }
        public string passWord { get; set; }
    }

    public class UserAPIActivePatch
    {
        //public string apiKey { get; set; }
        public bool? active { get; set; }
    }

}