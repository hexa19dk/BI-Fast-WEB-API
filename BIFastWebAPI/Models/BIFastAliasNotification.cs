using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Models
{
    public class ReqAliasNotification
    { 
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string SendingParticipantID { get; set; }
        public string OrigUniqueRequestID { get; set; }
        public string ProxyType { get; set; }
        public string ProxyValue { get; set; }
        public string OriginRegistrationID { get; set; }
        public string OriginDisplayName { get; set; }
        public string OriginProxyBankID { get; set; }
        public string OriginAccountID { get; set; }
        public string OriginAccountType { get; set; }
        public string OriginAccountName { get; set; }
        public string OriginCustomerType { get; set; }
        public string OriginCustomerID { get; set; }
        public string OriginCustomerResidentStatus { get; set; }
        public string OriginCustomerTownName { get; set; }
        public string NewRegistrationID { get; set; }
        public string NewDisplayName { get; set; }
        public string NewProxyBankID { get; set; }
        public string NewAccountID { get; set; }
        public string NewAccountType { get; set; }
        public string NewAccountName { get; set; }
        public string NewCustomerType { get; set; }
        public string NewCustomerID { get; set; }
        public string NewCustomerResidentStatus { get; set; }
        public string NewCustomerTownName { get; set; }

    }

}