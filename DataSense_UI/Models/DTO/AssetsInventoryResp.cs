using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class AssetsInventoryResp
    {
        public int dataSetId { get; set; }
        public string masterEnv { get; set; }
        public List<AssetsInventoryChild> childEnv { get; set; }
    }

    public class AssetsInventoryChild
    {
        public int dataSetIndexId { get; set; }

        public string childEnv { get; set; }

        public int totalAssets { get; set; }

        public int serverCount { get; set; }

        public int activeServers { get; set; }

        public int nonActiveServers { get; set; }

        public int dbCount { get; set; }

        public int activeDb { get; set; }

        public int nonActiveDb { get; set; }
    }
}