using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Models
{

    public class EventData
    {
        public string TENANT_ID { get; set; }
        public string BANK_ID { get; set; }
        public string EVENT_ID { get; set; }
        public string EVENT_CODE { get; set; }
        public string BANK_STATUS { get; set; }
        public string EVENT_TIME { get; set; }
        public string EMIT_TIME { get; set; }
        public string NET_SETTLEMENT_AMOUNT { get; set; }
    }

    public class ReqAliasSysNotification
    {
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string EventCode { get; set; }
        public List<string> EvtParam { get; set; }
        public string EventDesc { get; set; }
        public string EventTime { get; set; }
    }

    public class RespAliasSysNotification
    {
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string msgID { get; set; }
        public string originalTranRefNum { get; set; }
        public string EventCode { get; set; }
        public List<string> EvtParam { get; set; }
        public string EventDesc { get; set; }
        public string EventTime { get; set; }
    }

    public class EvtParam
    {
        public string TENANT_ID { get; set; }
        public string BANK_ID { get; set; }
        public string EVENT_ID { get; set; }
        public string EVENT_CODE { get; set; }
        public string BANK_STATUS { get; set; }
        public string EMIT_TIME { get; set; }
        public string EMNET_SETTLEMENT_AMOUNTIT_TIME { get; set; }
    }

}