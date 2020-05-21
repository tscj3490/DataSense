using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class DownloadLatestFileResult
    {
        public byte[] Stuff { get; set; }
        public string FileName { get; set; }
    }
}