using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Models
{
    public class Input
    {
        public string OperationType { get; set; }
    }

public class ReqAccountEnquiry
{
    public string EndToEndId { get; set; }
    public string MsgDefIdr { get; set; }
    public string TranRefNUM { get; set; }
    public string MsgCreationDate { get; set; }
    public string RecipentParticipantID { get; set; }
    public string CreditorAccountNo { get; set; }
    public string Amount { get; set; }
    public string Currency { get; set; }
    public string PurposeType { get; set; }
}

public class RespAccEnquiry
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
    public string CreditorAccountType { get; set; }
    public string CreditorAccountName { get; set; }
    public string CreditorType { get; set; }
    public string CreditorID { get; set; }
    public string CreditorResidentStatus { get; set; }
    public string CreditorTownName { get; set; }
}

public class RespRejectAccEnquiry
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

public class RespErrAccEnquiry
{
    public string SendingSystemBIC { get; set; }
    public string ReceivingSystemBIC { get; set; }
    public string BizMsgIdr { get; set; }
    public string MsgDefIdr { get; set; }
    public string Reference { get; set; }
    public string RejectReason { get; set; }
    public string RejectDateTime { get; set; }
    public string ErrorLocation { get; set; }
    public string ReasonDesc { get; set; }
}
}