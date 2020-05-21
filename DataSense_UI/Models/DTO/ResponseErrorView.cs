using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataSense_UI.Models.DTO
{
    public class ResponseErrorView
    {
        public int errorCount { get; set; }

        public List<ResponseErrorItemView> responseErrors { get; set; }

        public int httpstatus { get; set; }
    }
    public class ResponseErrorItemView
    {
        public string ErrorCode { get; set; }

        public string Message { get; set; }
    }
    public class ResponseErrormessage
    {
        public string message { get; set; }
    }
}