using BIFastWebAPI.Data;
using BIFastWebAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BIFastWebAPI.Utility
{
    public class FunctionUtility
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private readonly Helper Hp = new Helper();
        string st = "";

        #region CreditTransfer
        public RespCreditTrfAll CreditTransferAll(ViewModelTransaction VmTrx)
        {
            RespCreditTrfAll respall = new RespCreditTrfAll();
            ReqCreditTransfer req = new ReqCreditTransfer();
            RespCreditTransfer resp = new RespCreditTransfer();
            RejectCreditTransfer rejCt = new RejectCreditTransfer();
            ErrorCreditTransfer errCt = new ErrorCreditTransfer();

            string Date = DateTime.Now.ToString("yyyyMMdd");
            string bic = "AGTBIDJA"; //BIC Code
            string TrxTp = "010"; // Transaction Type
            string ori = "O"; // Originator
            string ct = VmTrx.ChannelType; // Channel Type
            string ss = ""; // Serial Number

            var ToInt = int.Parse(db.ActivityLogs.Select(i => i.Id).Count().ToString()) + 1;
            var lastID = db.ActivityLogs.Select(x => x.Id).Any() ? ToInt.ToString() : null;

            try
            {
                if (String.IsNullOrEmpty(lastID))
                {
                    var numInt = int.Parse(lastID) + 1;
                    var LastNo = numInt.ToString();
                    ss = LastNo.PadLeft(8, '0');
                }
                else
                {
                    ss = lastID.PadLeft(8, '0');
                }

                req.EndToEndId = Date + bic + TrxTp + ori + ct + ss;
                req.MsgDefIdr = "pacs.008.001.08";
                req.TranRefNUM = Date + bic + TrxTp + ss;
                req.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.sss");
                req.Amount = VmTrx.Amount;
                req.Currency = "IDR";
                req.PurposeType = VmTrx.PurpposeType;
                req.PaymentInformation = "Payment for housing";
                req.SendingParticipantID = "AGTBIDJA";
                req.DebitorAccountNo = VmTrx.DebitorAccountNo;
                req.DebitorAccountType = VmTrx.DebitorAccountType;
                req.DebitorAccountName = VmTrx.DebitorAccountName;
                req.DebitorType = VmTrx.DebitorType;
                req.DebitorID = VmTrx.DebitorID;
                req.DebitorResidentStatus = "01";
                req.DebitorTownName = "0300";
                req.RecipentParticipantID = "BRINIDJA";
                req.CreditorAccountNo = VmTrx.CreditorAccountNo;
                req.CreditorAccountType = VmTrx.CreditorAccountType;
                req.CreditorAccountName = VmTrx.CreditorAccountName;
                req.CreditorType = VmTrx.CreditorType;
                req.CreditorID = VmTrx.CreditorID;
                req.CreditorResidentStatus = VmTrx.CreditorResidentStatus;
                req.CreditorTownName = "0300";
                req.PaymentInformation = VmTrx.PaymentInformation;

                string jsonRequest = JsonConvert.SerializeObject(req), idr = req.EndToEndId, num = req.TranRefNUM;
                string jsonResponse = Hp.GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/CreditTransfer");

                if (Hp.Ck(req.EndToEndId) && Hp.Ck(req.MsgDefIdr) && Hp.Ck(req.TranRefNUM) && Hp.Ck(req.RecipentParticipantID) && Hp.Ck(req.CreditorAccountNo) && Hp.Ck(req.Amount) && Hp.Ck(req.Currency) && Hp.Ck(req.MsgCreationDate) && Hp.Ck(req.PurposeType) && Hp.Ck(req.SendingParticipantID) && Hp.Ck(req.DebitorAccountNo) && Hp.Ck(req.DebitorAccountType) && Hp.Ck(req.DebitorAccountName) && Hp.Ck(req.DebitorID) && Hp.Ck(req.RecipentParticipantID) && Hp.Ck(req.CreditorAccountNo) && Hp.Ck(req.CreditorAccountName) && jsonResponse.Contains("pacs.002.001.10"))
                {
                    resp = JsonConvert.DeserializeObject<RespCreditTransfer>(jsonResponse);
                    st = "Success";

                    Hp.SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(resp.MsgCreationDate, null, DateTimeStyles.RoundtripKind));

                    respall.MsgDefIdr = resp.MsgDefIdr;
                    respall.TranRefNUM = resp.TranRefNUM;
                    respall.MsgCreationDate = resp.MsgCreationDate;
                    respall.OriginalMsgdefIdr = resp.OriginalMsgdefIdr;
                    respall.OrigEndToEndId = resp.OrigEndToEndId;
                    respall.OrigTranRefNUM = resp.OrigTranRefNUM;
                    respall.TransactionStatus = resp.TransactionStatus;
                    respall.ReasonCode = resp.ReasonCode;
                    respall.AdditionalInfo = resp.AdditionalInfo;
                    respall.CreditorAccountNo = resp.CreditorAccountNo;
                    respall.CreditorAccountType = resp.CreditorAccountType;
                    respall.CreditorAccountName = resp.CreditorAccountName;
                    respall.CreditorType = resp.CreditorType;
                    respall.CreditorID = resp.CreditorID;
                    respall.CreditorResidentStatus = resp.CreditorResidentStatus;
                    respall.CreditorTownName = resp.CreditorTownName;
                }
                else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
                {
                    errCt = JsonConvert.DeserializeObject<ErrorCreditTransfer>(jsonResponse);
                    st = "Error";

                    Hp.SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errCt.CreationDateTime, null, DateTimeStyles.RoundtripKind));

                    respall.SendingSystemBIC = errCt.SendingSystemBIC;
                    respall.ReceivingSystemBIC = errCt.ReceivingSystemBIC;
                    respall.BizMsgIdr = errCt.BizMsgIdr;
                    respall.MsgDefIdr = errCt.MsgDefIdr;
                    respall.CreationDateTime = errCt.CreationDateTime;
                    respall.Reference = errCt.Reference;
                    respall.RejectReason = errCt.RejectReason;
                    respall.RejectDateTime = errCt.RejectDateTime;
                    respall.ErrorLocation = errCt.ErrorLocation;
                    respall.ReasonDesc = errCt.ReasonDesc;
                }
                else
                {
                    rejCt = JsonConvert.DeserializeObject<RejectCreditTransfer>(jsonResponse);
                    st = "Reject";

                    Hp.SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejCt.MsgCreationDate, null, DateTimeStyles.RoundtripKind));

                    respall.MsgDefIdr = rejCt.MsgDefIdr;
                    respall.TranRefNUM = rejCt.TranRefNUM;
                    respall.CreationDateTime = rejCt.MsgCreationDate;
                    respall.OriginalMsgdefIdr = rejCt.OriginalMsgdefIdr;
                    respall.OrigEndToEndId = rejCt.OrigEndToEndId;
                    respall.OrigTranRefNUM = rejCt.OrigTranRefNUM;
                    respall.TransactionStatus = rejCt.TransactionStatus;
                    respall.ReasonCode = rejCt.ReasonCode;
                    respall.CreditorAccountNo = rejCt.CreditorAccountNo;
                }
            }
            catch (Exception ex)
            {
                //throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }

            return respall;
        }
        #endregion


    }


}