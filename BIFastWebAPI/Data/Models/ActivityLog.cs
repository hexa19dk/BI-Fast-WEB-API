using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Data.Models
{
    public class ActivityLog
    {
        public int Id { get; set; }
        public string Channel { get; set; }
        public string EndPoint { get; set; }
        public string BizMsgIdr { get; set; }
        public string OrigTranRefNUM { get; set; }
        public string Type { get; set; }
        public string CIF { get; set; }
        public string ReqMessage { get; set; }
        public string RespMessage { get; set; }
        public string Status { get; set; }
        public DateTime? LogDate { get; set; }
        public DateTime ReqDate { get; set; }
        public DateTime RespDate { get; set; }
    }
}