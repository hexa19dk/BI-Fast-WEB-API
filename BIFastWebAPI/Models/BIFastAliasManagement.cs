using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Models
{
    public class ReqAliasManagement
    {
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string SendingParticipantID { get; set; }
        public string MsgSenderAccountId { get; set; }
        public string OperationType { get; set; }
        public string ProxyType { get; set; }
        public string ProxyValue { get; set; }
        public string RegistrationID { get; set; }
        public string DisplayName { get; set; }
        public string ProxyBankID { get; set; }
        public string AccountID { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string SecondaryIDType { get; set; }
        public string SecondaryIDValue { get; set; }
        public string RegistrationStatus { get; set; }
        public string CustomerType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerResidentStatus { get; set; }
        public string CustomerTownName { get; set; }
    }



    public class RespAliasManagement
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
        public string OriginalMsgdefIdr { get; set; }
        public string OrigMsgCreationDate { get; set; }
        public string ProxyInqRespStatusCode { get; set; }
        public string StatusReasonCode { get; set; }
        public string OperationType { get; set; }
        public string RegistrationID { get; set; }
        public string ProxyBankID { get; set; }
        public string CustomerType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerResidentStatus { get; set; }
        public string CustomerTownName { get; set; }
        



    }

    public class RespRejectAliasManagement
    {
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string Reference { get; set; }
        public string RejectReason { get; set; }
        public string RejectDateTime { get; set; }

    }

    public class RespErrAliasManagement
    {
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string Reference { get; set; }
        public string RejectReason { get; set; }
        public string RejectDateTime { get; set; }
        public string ErrorLocation { get; set; }
        public string ReasonDesc { get; set; }
    }

    public class AliasManagementResponses
    {
        public string ResponseType { get; set; }
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string SendingParticipantID { get; set; }
        public string OrigUniqueRequestID { get; set; }
        public string OriginalMsgdefIdr { get; set; }
        public string OrigMsgCreationDate { get; set; }
        public string ProxyInqRespStatusCode { get; set; }
        public string StatusReasonCode { get; set; }
        public string OperationType { get; set; }
        public string RegistrationID { get; set; }
        public string ProxyBankID { get; set; }
        public string CustomerType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerResidentStatus { get; set; }
        public string CustomerTownName { get; set; }
        public string Reference { get; set; }
        public string RejectReason { get; set; }
        public string RejectDateTime { get; set; }
        public string ErrorLocation { get; set; }
        public string ReasonDesc { get; set; }
    }

    public class AliasManagementVM
    {
        public string CIF { get; set; }
        public string ChannelType { get; set; }
        public string MsgSenderAccountId { get; set; }
        public string OperationType { get; set; }
        public string ProxyType { get; set; }
        public string ProxyValue { get; set; }
        public string DisplayName { get; set; }
        public string AccountID { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
        public string SecondaryIDType { get; set; }
        public string SecondaryIDValue { get; set; }
        public string RegistrationStatus { get; set; }
        public string CustomerType { get; set; }
        public string CustomerID { get; set; }
        public string CustomerResidentStatus { get; set; }
        public string CustomerTownName { get; set; }
        public string Channel { get; set; }
    }
}