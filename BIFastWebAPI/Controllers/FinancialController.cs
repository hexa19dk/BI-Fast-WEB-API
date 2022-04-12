using BIFastWebAPI.Models;
using BIFastWebAPI.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestSharp;
using AngleSharp.Io;
using System.Globalization;
using BIFastWebAPI.Data;

namespace BIFastWebAPI.Controllers
{
    public class FinancialController : ApiController
    {
        Helper Hp = new Helper();
        string chan, st = "" ;
        DateTime cd;
        ApplicationDbContext db = new ApplicationDbContext();

        #region AccountEnquiry
        [HttpPost]
        [Route("jsonAPI/AccountEnquiry")]
        public IHttpActionResult AccountEnquiry([FromBody] ReqAccountEnquiry reqAcc)
        {
            //ReqAccountEnquiry reqAcc = new ReqAccountEnquiry();
            RespAccEnquiry respAcc = new RespAccEnquiry();
            RespRejectAccEnquiry rejcAcc = new RespRejectAccEnquiry();
            RespErrAccEnquiry errAcc = new RespErrAccEnquiry();

            string jsonRequest = JsonConvert.SerializeObject(reqAcc), idr = reqAcc.EndToEndId, num = reqAcc.TranRefNUM;
            string jsonResponse = Hp.GenerateReq(reqAcc, "http://10.99.0.72:8355/jsonAPI/AccountEnquiry");
            

            if (Hp.Ck(reqAcc.EndToEndId) && Hp.Ck(reqAcc.MsgDefIdr) && Hp.Ck(reqAcc.TranRefNUM) && Hp.Ck(reqAcc.RecipentParticipantID) && Hp.Ck(reqAcc.CreditorAccountNo) && Hp.Ck(reqAcc.Amount) && Hp.Ck(reqAcc.Currency) && Hp.Ck(reqAcc.MsgCreationDate) && jsonResponse.Contains("pacs.002.001.10"))
            {
                respAcc = JsonConvert.DeserializeObject<RespAccEnquiry>(jsonResponse);
                st = "Success";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(respAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(respAcc);
            } 
            else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
            {
                errAcc = JsonConvert.DeserializeObject<RespErrAccEnquiry>(jsonResponse);
                st = "Error";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(errAcc.CreationDateTime, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errAcc.RejectDateTime, null, DateTimeStyles.RoundtripKind));
                return Ok(errAcc);
            }
            else
            {
                rejcAcc = JsonConvert.DeserializeObject<RespRejectAccEnquiry>(jsonResponse);
                if (reqAcc.MsgCreationDate != null)
                {
                    cd = DateTime.Parse(reqAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind);
                }
                else
                {
                    cd = DateTime.Now;
                }
                st = "Reject";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, cd, DateTime.Parse(rejcAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(rejcAcc);
            }
        }
        #endregion

        #region CreditTransfer
        [HttpPost]
        [Route("jsonAPI/CreditTransfer")]
        public IHttpActionResult CreditTransfer([FromBody] ViewModelTransaction VmTrx)
        {
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
            req.PaymentInformation = VmTrx.PaymentInformation;
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

            string jsonRequest = JsonConvert.SerializeObject(req), idr = req.EndToEndId, num = req.TranRefNUM;
            string jsonResponse = Hp.GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/CreditTransfer");

            if (Hp.Ck(req.EndToEndId) && Hp.Ck(req.MsgDefIdr) && Hp.Ck(req.TranRefNUM) && Hp.Ck(req.RecipentParticipantID) && Hp.Ck(req.CreditorAccountNo) && Hp.Ck(req.Amount) && Hp.Ck(req.Currency) && Hp.Ck(req.MsgCreationDate) && Hp.Ck(req.PurposeType) && Hp.Ck(req.SendingParticipantID) && Hp.Ck(req.DebitorAccountNo) && Hp.Ck(req.DebitorAccountType) && Hp.Ck(req.DebitorAccountName) && Hp.Ck(req.DebitorID) && Hp.Ck(req.RecipentParticipantID) && Hp.Ck(req.CreditorAccountNo) && Hp.Ck(req.CreditorAccountName) && jsonResponse.Contains("pacs.002.001.10"))
            {
                resp = JsonConvert.DeserializeObject<RespCreditTransfer>(jsonResponse);
                st = "Success";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(resp.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(resp);
            }
            else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
            {
                errCt = JsonConvert.DeserializeObject<ErrorCreditTransfer>(jsonResponse);
                st = "Error";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errCt.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                return Ok(errCt);
            }
            else
            {
                rejCt = JsonConvert.DeserializeObject<RejectCreditTransfer>(jsonResponse);
                st = "Reject";
                Hp.SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejCt.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(rejCt);
            }
        }
        #endregion

        #region CreditTransferToProxy
        [HttpPost]
        [Route("jsonAPI/CreditTransferToProxy")]
        public IHttpActionResult CreditTransferToProxy([FromBody] ReqCreditTransferToProxy reqCtPrx)
        {
            //ReqCreditTransferToProxy reqCtPrx = new ReqCreditTransferToProxy();
            RespCrediTransferToProxy resCtPrx = new RespCrediTransferToProxy();
            RespRejectCreditTransferToProxy rejCtPrx = new RespRejectCreditTransferToProxy();
            RespErrCreditTransferToProxy errCtPrx = new RespErrCreditTransferToProxy();

            string jsonRequest = JsonConvert.SerializeObject(reqCtPrx), idr = reqCtPrx.EndToEndId, num = reqCtPrx.TranRefNUM;
            string jsonResponse = Hp.GenerateReq(reqCtPrx, "http://10.99.0.72:8355/jsonAPI/CreditTransferToProxy");

            if (Hp.Ck(reqCtPrx.EndToEndId) && Hp.Ck(reqCtPrx.MsgDefIdr) && Hp.Ck(reqCtPrx.TranRefNUM) && Hp.Ck(reqCtPrx.RecipentParticipantID) && Hp.Ck(reqCtPrx.CreditorAccountNo) && Hp.Ck(reqCtPrx.Amount) && Hp.Ck(reqCtPrx.Currency) && Hp.Ck(reqCtPrx.MsgCreationDate) && Hp.Ck(reqCtPrx.PurposeType) && Hp.Ck(reqCtPrx.SendingParticipantID) && Hp.Ck(reqCtPrx.DebitorAccountNo) && Hp.Ck(reqCtPrx.DebitorAccountType) && Hp.Ck(reqCtPrx.DebitorAccountName) && Hp.Ck(reqCtPrx.DebitorID) && Hp.Ck(reqCtPrx.RecipentParticipantID) && Hp.Ck(reqCtPrx.CreditorAccountNo) && Hp.Ck(reqCtPrx.CreditorAccountName) && jsonResponse.Contains("Pacs.008.001.08"))
            {
                resCtPrx = JsonConvert.DeserializeObject<RespCrediTransferToProxy>(jsonResponse);
                st = "Success";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(resCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(resCtPrx);
            }
            else if(jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
            {
                errCtPrx = JsonConvert.DeserializeObject<RespErrCreditTransferToProxy>(jsonResponse);
                st = "Error";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errCtPrx.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                return Ok(errCtPrx);
            }
            else
            {
                rejCtPrx = JsonConvert.DeserializeObject<RespRejectCreditTransferToProxy>(jsonResponse);
                st = "Reject";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(rejCtPrx);
            }
        }
        #endregion

        #region ReversalCreditTransfer
        [HttpPost]
        [Route("jsonAPI/ReversalCreditTransfer")]
        public IHttpActionResult ReversalCreditTransfer([FromBody] ReqReversalCreditTransfer req)
        {
            //ReqReversalCreditTransfer req = new ReqReversalCreditTransfer();
            RespReversalCreditTransfer res = new RespReversalCreditTransfer();
            RespRejectRCT rej = new RespRejectRCT();
            RespErrRCT err = new RespErrRCT();

            string jsonRequest = JsonConvert.SerializeObject(req), idr = req.EndToEndId, num = req.TranRefNUM;
            string jsonResponse = Hp.GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/ReversalCreditTransfer");

            if (Hp.Ck(req.EndToEndId) && Hp.Ck(req.MsgDefIdr) && Hp.Ck(req.TranRefNUM) && Hp.Ck(req.RecipentParticipantID) && Hp.Ck(req.CreditorAccountNo) && Hp.Ck(req.Amount) && Hp.Ck(req.Currency) && Hp.Ck(req.PurposeType) && Hp.Ck(req.SendingParticipantID) && Hp.Ck(req.DebitorAccountNo) && Hp.Ck(req.DebitorAccountType) && Hp.Ck(req.DebitorAccountName) && Hp.Ck(req.DebitorID) && Hp.Ck(req.RecipentParticipantID) && Hp.Ck(req.CreditorAccountNo) && Hp.Ck(req.CreditorAccountName) && jsonResponse.Contains("pacs.008.001.08"))
            {
                res = JsonConvert.DeserializeObject<RespReversalCreditTransfer>(jsonResponse);
                st = "Success";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(res.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(res);
            }
            else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
            {
                err = JsonConvert.DeserializeObject<RespErrRCT>(jsonResponse);
                st = "Error";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(err.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                return Ok(err);
            }
            else
            {
                rej = JsonConvert.DeserializeObject<RespRejectRCT>(jsonResponse);
                st = "Reject";
                Hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rej.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(rej);
            }
        }
        #endregion

        #region PaymentStatus
        [HttpPost]
        [Route("jsonAPI/PaymentStatus")]
        public IHttpActionResult PaymentStatus([FromBody] ReqPaymentStatus req)
        {
            //ReqPaymentStatus req = new ReqPaymentStatus();
            RespPaymentStatus res = new RespPaymentStatus();
            RespRejectPaymentStatus rej = new RespRejectPaymentStatus();
            RespErrPaymentStatus err = new RespErrPaymentStatus();

            string jsonRequest = JsonConvert.SerializeObject(req), idr = req.OrigEndToEndId, num = req.TranRefNUM;
            string jsonResponse = Hp.GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/PaymentStatus");

            if (Hp.Ck(req.TranRefNUM) && Hp.Ck(req.OrigEndToEndId) && jsonResponse.Contains("pacs.002.001.10"))
            {
                res = JsonConvert.DeserializeObject<RespPaymentStatus>(jsonResponse);
                st = "Success";
                Hp.SaveLog(chan, num, res.MsgDefIdr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(res.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(res);
            }
            else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
            {
                err = JsonConvert.DeserializeObject<RespErrPaymentStatus>(jsonResponse);
                st = "Error";
                Hp.SaveLog(chan, num, err.BizMsgIdr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(err.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                return Ok(err);
            }
            else
            {
                rej = JsonConvert.DeserializeObject<RespRejectPaymentStatus>(jsonResponse);
                st = "Reject";
                Hp.SaveLog(chan, num, rej.MsgDefIdr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rej.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                return Ok(rej);
            }          
        }
        #endregion
    }
}
