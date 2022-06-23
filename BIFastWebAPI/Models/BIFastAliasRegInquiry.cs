using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Models
{
    public class ReqAliasRegInquiry
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
        public string RegistrationID { get; set; }
        public string SecondaryIDType { get; set; }
        public string SecondaryIDValue { get; set; }

    }

    public class UserRegInVM
    {
        public string RegistrationID { get; set; }
        public string DisplayName { get; set; }
        public string ProxyType { get; set; }
        public string ProxyRecordStatus { get; set; }
        public string AccountBIC { get; set; }
        public string AccountID { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
    }


    public class AccountData 
    {
        public string RegistrationID { get; set; }
        public string DisplayName { get; set; }
        public string ProxyType { get; set; }
        public string ProxyValue { get; set; }
        public string ProxyRecordStatus { get; set; }
        public string AccountBIC { get; set; }
        public string AccountID { get; set; }
        public string AccountType { get; set; }
        public string AccountName { get; set; }
    }

    public class RespAliasRegInquiry 
    {

        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string RecipentParticipantID { get; set; }
        public string OrigUniqueRequestID { get; set; }
        public string OriginalMsgdefIdr { get; set; }
        public string OrigMsgCreationDate { get; set; }
        public string ProxyInqRespStatusCode { get; set; }
        public string StatusReasonCode { get; set; }
        public AccountData[] RspnData { get; set; }
    }

    public class RespRejectAliasRegInquiry { 
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string Reference { get; set; }
        public string RejectReason { get; set; }
        public string RejectDateTime { get; set; }

    }

    public class RespErrAliasRegInquiry
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

    public class AliasRegInquiryResponses
    {
        public string ResponseType { get; set; }
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string MsgDefIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string RecipentParticipantID { get; set; }
        public string OrigUniqueRequestID { get; set; }
        public string OriginalMsgdefIdr { get; set; }
        public string OrigMsgCreationDate { get; set; }
        public string ProxyInqRespStatusCode { get; set; }
        public string StatusReasonCode { get; set; }
        public AccountData[] RspnData { get; set; }
        public string Reference { get; set; }
        public string RejectReason { get; set; }
        public string RejectDateTime { get; set; }
        public string ErrorLocation { get; set; }
        public string ReasonDesc { get; set; }
    }

    public class AliasRegInquiryVM
    {
        public string CIF { get; set; }
        public string ChannelType { get; set; }
        public string MsgSenderAccountId { get; set; }
        public string SecondaryIDType { get; set; }
        public string SecondaryIDValue { get; set; }
        public string Channel { get; set; }
    }
}