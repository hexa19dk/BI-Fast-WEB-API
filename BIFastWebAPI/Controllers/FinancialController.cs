using BIFastWebAPI.Models;
using BIFastWebAPI.Utility;
using Newtonsoft.Json;
using System;
using System.Web.Http;
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
        public IHttpActionResult AccountEnquiry(ViewModelAccount vmAcc)
        {
            RespAllAccount resp = new RespAllAccount();
            resp = Hp.AccountEnquiry(vmAcc);
            return Ok(resp);
        }
        #endregion

        #region CreditTransfer
        [HttpPost]
        [Route("jsonAPI/CreditTransfer")]
        public IHttpActionResult CreditTransfer(ViewModelTransaction VmTrx)
        {
            RespCreditTrfAll respall = new RespCreditTrfAll();
            respall = Hp.CreditTransferAll(VmTrx);
            return Ok(respall);
        }
        #endregion

        #region CreditTransferToProxy
        [HttpPost]
        [Route("jsonAPI/CreditTransferToProxy")]
        public IHttpActionResult CreditTransferToProxy([FromBody] ViewModelProxy vmProx)
        {
            RespAllCreditProxy respAll = new RespAllCreditProxy();
            respAll = Hp.CreditToProxy(vmProx);
            return Ok(respAll);
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
