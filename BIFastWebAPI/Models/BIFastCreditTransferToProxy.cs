using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Models
{
    public class ReqCreditTransferToProxy
    {
        public string EndToEndId { get; set; }
        public string MsgDefIdr { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string PurposeType { get; set; }
        public string PaymentInformation { get; set; }
        public string SendingParticipantID { get; set; }
        public string DebitorAccountNo { get; set; }
        public string DebitorAccountType { get; set; }
        public string DebitorAccountName { get; set; }
        public string DebitorType { get; set; }
        public string DebitorID { get; set; }
        public string DebitorResidentStatus { get; set; }
        public string DebitorTownName { get; set; }
        public string RecipentParticipantID { get; set; }
        public string CreditorAccountNo { get; set; }
        public string CreditorAccountType { get; set; }
        public string CreditorAccountName { get; set; }
        public string ProxyType { get; set; }
        public string ProxyValue { get; set; }
        public string CreditorType { get; set; }
        public string CreditorID { get; set; }
        public string CreditorResidentStatus { get; set; }
        public string CreditorTownName { get; set; }
    }

    public class RespAllCreditProxy
    {
        public string MsgDefIdr { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string OriginalMsgdefIdr { get; set; }
        public string OrigEndToEndId { get; set; }
        public string OrigTranRefNUM { get; set; }
        public string TransactionStatus { get; set; }
        public string ReasonCode { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecipentParticipantID { get; set; }
        public string CreditorAccountNo { get; set; }
        public string CreditorAccountType { get; set; }
        public string CreditorAccountName { get; set; }
        public string CreditorType { get; set; }
        public string CreditorID { get; set; }
        public string CreditorResidentStatus { get; set; }
        public string CreditorTownName { get; set; }
        public string SendingSystemBIC { get; set; }
        public string ReceivingSystemBIC { get; set; }
        public string BizMsgIdr { get; set; }
        public string CreationDateTime { get; set; }
        public string Reference { get; set; }
        public string RejectReason { get; set; }
        public string RejectDateTime { get; set; }
        public string ErrorLocation { get; set; }
        public string ReasonDesc { get; set; }
        public string ResponseType { get; set; }
    }

    public class RespCrediTransferToProxy
    {
        public string MsgDefIdr { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string OriginalMsgdefIdr { get; set; }
        public string OrigEndToEndId { get; set; }
        public string OrigTranRefNUM { get; set; }
        public string TransactionStatus { get; set; }
        public string ReasonCode { get; set; }
        public string AdditionalInfo { get; set; }
        public string RecipentParticipantID { get; set; }
        public string CreditorAccountNo { get; set; }
        public string CreditorAccountType { get; set; }
        public string CreditorAccountName { get; set; }
        public string CreditorType { get; set; }
        public string CreditorID { get; set; }
        public string CreditorResidentStatus { get; set; }
        public string CreditorTownName { get; set; }
    }

    public class RespRejectCreditTransferToProxy
    {
        public string MsgDefIdr { get; set; }
        public string TranRefNUM { get; set; }
        public string MsgCreationDate { get; set; }
        public string OriginalMsgdefIdr { get; set; }
        public string OrigEndToEndId { get; set; }
        public string OrigTranRefNUM { get; set; }
        public string TransactionStatus { get; set; }
        public string ReasonCode { get; set; }
        public string CreditorAccountNo { get; set; }
    }

    public class RespErrCreditTransferToProxy
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

    public class ViewModelProxy
    {
        public string ChannelType { get; set; }
        public string Sequence { get; set; }
        public string Amount { get; set; }
        public string PurposeType { get; set; }
        public string DebitorAccountNo { get; set; }
        public string DebitorAccountType { get; set; }
        public string DebitorAccountName { get; set; }
        public string DebitorType { get; set; }
        public string DebitorID { get; set; }
        public string DebitorResidentStatus { get; set; }
        public string CreditorAccountNo { get; set; }
        public string CreditorAccountType { get; set; }
        public string CreditorAccountName { get; set; }
        public string CreditorType { get; set; }
        public string CreditorID { get; set; }
        public string CreditorResidentStatus { get; set; }
        public string PaymentInformation { get; set; }
        public string ProxyValue { get; set; }
        public string SendingParticipantID { get; set; }
        public string RecipentParticipantID { get; set; }
    }
}