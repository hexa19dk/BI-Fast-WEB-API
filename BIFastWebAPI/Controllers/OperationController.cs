using BIFastWebAPI.Models;
using BIFastWebAPI.Utility;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Web.Http;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using BIFastWebAPI.Data.Models;
using BIFastWebAPI.Data;
//using System.Text.Json;

namespace BIFastWebAPI.Controllers
{
    public class OperationController : ApiController
    {

        private readonly Helper hp = new Helper();
        string st = "", chan;
        
        ApplicationDbContext _db = new ApplicationDbContext();

        #region AliasManagement
        [HttpPost]
        [Route("jsonAPI/prxy001")]
        public IHttpActionResult AliasManagement([FromBody] ReqAliasManagement reqAM)
        {
            RespAliasManagement respAM = new RespAliasManagement();
            RespRejectAliasManagement rejAM = new RespRejectAliasManagement();
            RespErrAliasManagement errAM = new RespErrAliasManagement();
            string jsonRequest = JsonConvert.SerializeObject(reqAM), jsonResponse, num = reqAM.TranRefNUM, idr = reqAM.BizMsgIdr;
            jsonResponse = hp.GenerateReq(reqAM, "http://10.99.0.72:8355/jsonAPI/prxy001");
            

            if ( hp.Ck(reqAM.SendingSystemBIC) && hp.Ck(reqAM.ReceivingSystemBIC) &&  hp.Ck(reqAM.BizMsgIdr) && hp.Ck(reqAM.CreationDateTime) && hp.Ck(reqAM.TranRefNUM) && hp.Ck(reqAM.MsgCreationDate) && hp.Ck(reqAM.SendingParticipantID) && hp.Ck(reqAM.MsgSenderAccountId) && hp.Ck(reqAM.OperationType) && hp.Ck(reqAM.ProxyType) && hp.Ck(reqAM.ProxyValue) && hp.Ck(reqAM.ProxyBankID) && hp.Ck(reqAM.AccountID) && hp.Ck(reqAM.AccountType) && hp.Ck(reqAM.SecondaryIDType) && hp.Ck(reqAM.SecondaryIDValue) && hp.Ck(reqAM.SendingSystemBIC))
            {

                //success
                respAM = JsonConvert.DeserializeObject<RespAliasManagement>(jsonResponse);
                st = "Success";
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(respAM);

                
            }
            else if(jsonResponse.Contains("ErrorLocation"))
            {
                //error
                st = "Error";
                errAM = JsonConvert.DeserializeObject<RespErrAliasManagement>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(errAM);

            }
            else
            {
                //reject
                st = "Reject";
                rejAM = JsonConvert.DeserializeObject<RespRejectAliasManagement>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(rejAM);
               
            }
           

        }
        #endregion

        #region AliasResolution
        [HttpPost]
        [Route("jsonAPI/prxy003")]
        public IHttpActionResult AliasResolution([FromBody] ReqAliasResolution reqAR)
        {
            RespAliasResolution respAR = new RespAliasResolution();
            RespRejectAliasResolution rejAR = new RespRejectAliasResolution();
            RespErrAliasResolution errAR = new RespErrAliasResolution();
            string jsonRequest  = JsonConvert.SerializeObject(reqAR), jsonResponse, num = reqAR.TranRefNUM, idr = reqAR.BizMsgIdr;
            jsonResponse = hp.GenerateReq(reqAR, "http://10.99.0.72:8355/jsonAPI/prxy003");
         

            if (hp.Ck(reqAR.SendingSystemBIC) && hp.Ck(reqAR.ReceivingSystemBIC) && hp.Ck(reqAR.BizMsgIdr) && hp.Ck(reqAR.MsgDefIdr) && hp.Ck(reqAR.CreationDateTime) && hp.Ck(reqAR.TranRefNUM) && hp.Ck(reqAR.MsgCreationDate) && hp.Ck(reqAR.SendingParticipantID) && hp.Ck(reqAR.MsgSenderAccountId) && hp.Ck(reqAR.AlisaResolutionLookup) && hp.Ck(reqAR.UniqueRequestID) && hp.Ck(reqAR.ProxyType) && hp.Ck(reqAR.ProxyValue))
            {
                //success
                st = "Success";
                respAR = JsonConvert.DeserializeObject<RespAliasResolution>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(respAR);

            }
            else if (errAR.ErrorLocation != null)
            {
                //error
                st = "Error";
                errAR = JsonConvert.DeserializeObject<RespErrAliasResolution>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(errAR);
               
            }
            else
            {
                //reject
                st = "Reject";
                rejAR = JsonConvert.DeserializeObject<RespRejectAliasResolution>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(rejAR);

            }
        }
        #endregion

        #region AliasRegistrationInquiry
        [HttpPost]
        [Route("jsonAPI/prxy005")]
        public IHttpActionResult AliasRegistrationInquiry([FromBody] ReqAliasRegInquiry reqARI)
        {
            RespAliasRegInquiry respARI = new RespAliasRegInquiry();
            RespRejectAliasRegInquiry rejARI = new RespRejectAliasRegInquiry();
            RespErrAliasRegInquiry errARI = new RespErrAliasRegInquiry();
            string jsonRequest, jsonResponse;
            //jsonResponse = hp.GenerateReq(reqARI, "localhost:44350/jsonAPI/prxy005");
            jsonResponse = hp.GenerateReq(reqARI, "http://172.18.99.30:4343/jsonAPI/prxy005");
        


            if (hp.Ck(reqARI.SendingSystemBIC) && hp.Ck(reqARI.ReceivingSystemBIC) && hp.Ck(reqARI.BizMsgIdr) && hp.Ck(reqARI.MsgDefIdr) && hp.Ck(reqARI.CreationDateTime) && hp.Ck(reqARI.TranRefNUM) && hp.Ck(reqARI.MsgCreationDate) && hp.Ck(reqARI.SendingParticipantID) && hp.Ck(reqARI.MsgSenderAccountId))
            {
                //success
                respARI = JsonConvert.DeserializeObject<RespAliasRegInquiry>(jsonResponse);
                return Ok(respARI);
            }
            else if (errARI.ErrorLocation != null)
            {
                //error
                errARI = JsonConvert.DeserializeObject<RespErrAliasRegInquiry>(jsonResponse);
                return Ok(errARI);
            }
            else
            {
                //error
                rejARI = JsonConvert.DeserializeObject<RespRejectAliasRegInquiry>(jsonResponse);
                return Ok(rejARI);
            }
        }
        #endregion

        #region AliasNotification
        [HttpPost]
        [Route("jsonAPI/prxy901")]
        public IHttpActionResult AliasNotification([FromBody] ReqAliasNotification reqAN)
        {

            string jsonRequest,jsonResponse;
            //jsonResponse = hp.GenerateReq(reqAN, "localhost:44350/jsonAPI/prxy901");
            jsonResponse = hp.GenerateReq(reqAN, "http://172.18.99.30:4343/jsonAPI/prxy901");

            if (hp.Ck(reqAN.SendingSystemBIC) && hp.Ck(reqAN.ReceivingSystemBIC) && hp.Ck(reqAN.BizMsgIdr) && hp.Ck(reqAN.MsgDefIdr) && hp.Ck(reqAN.CreationDateTime) && hp.Ck(reqAN.TranRefNUM) && hp.Ck(reqAN.MsgCreationDate) && hp.Ck(reqAN.SendingParticipantID) && hp.Ck(reqAN.OrigUniqueRequestID) && hp.Ck(reqAN.ProxyType) && hp.Ck(reqAN.ProxyValue) && hp.Ck(reqAN.OriginRegistrationID) && hp.Ck(reqAN.OriginDisplayName) && hp.Ck(reqAN.OriginProxyBankID) && hp.Ck(reqAN.OriginAccountID) && hp.Ck(reqAN.OriginAccountType) && hp.Ck(reqAN.NewRegistrationID) && hp.Ck(reqAN.NewDisplayName) && hp.Ck(reqAN.NewProxyBankID) && hp.Ck(reqAN.NewAccountID) && hp.Ck(reqAN.NewAccountType))
            {
                //sukses
                return Ok();
            }
            //else if (true)
            //{
            //    //reject
            //    return Ok();
            //}
            else
            {
                //error
                return InternalServerError();
            }

        }
        #endregion

        #region SystemNotification

        [HttpPost]
        [Route("jsonAPI/adm004")]
        public IHttpActionResult SystemNotification([FromBody] ReqAliasSysNotification reqSN)
        {
            RespAliasSysNotification respSN = new RespAliasSysNotification();
            string jsonRequest, jsonResponse;
            //jsonResponse = hp.GenerateReq(reqSN, "localhost:44350/jsonAPI/adm004");
            jsonResponse = hp.GenerateReq(reqSN, "http://172.18.99.30:4343/jsonAPI/adm004");

            if (hp.Ck(reqSN.SendingSystemBIC) && hp.Ck(reqSN.ReceivingSystemBIC) && hp.Ck(reqSN.BizMsgIdr) && hp.Ck(reqSN.MsgDefIdr) && hp.Ck(reqSN.CreationDateTime) && hp.Ck(reqSN.EventCode))
            {
                //success
                respSN = JsonConvert.DeserializeObject<RespAliasSysNotification>(jsonResponse);
                return Ok(respSN);
            }
            else
            {
                //error
                return InternalServerError();
            }
        }
        #endregion
    }
}
