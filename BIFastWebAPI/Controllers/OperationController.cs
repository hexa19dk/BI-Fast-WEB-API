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
        string st = "", ss = "",chan, Date = DateTime.Now.ToString("yyyyMMdd");   
        ApplicationDbContext _db = new ApplicationDbContext();


        #region AliasManagement
        [HttpPost]
        [Route("jsonAPI/prxy001")]
        public IHttpActionResult AliasManagement([FromBody] AliasManagementVM data)
        {
            ReqAliasManagement reqAM = new ReqAliasManagement();
            RespAliasManagement respAM = new RespAliasManagement();
            RespRejectAliasManagement rejAM = new RespRejectAliasManagement();
            RespErrAliasManagement errAM = new RespErrAliasManagement();

            var ToInt = int.Parse(_db.ActivityLogs.Select(i => i.Id).Count().ToString()) + 1;
            var lastID = _db.ActivityLogs.Select(x => x.Id).Any() ? ToInt.ToString() : null;

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

                reqAM.SendingSystemBIC = data.SBIC;
                reqAM.ReceivingSystemBIC = data.RBIC;
                reqAM.BizMsgIdr = Date + data.SBIC + data.TransactionType + data.Originator + data.ChannelType + ss;
                reqAM.MsgDefIdr = data.MsgDefIdr;
                reqAM.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.ssZ");
                reqAM.TranRefNUM = Date + data.SBIC + data.TransactionType + ss;
                reqAM.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sss");
                reqAM.SendingParticipantID = data.SendingParticipantID;
                reqAM.MsgSenderAccountId = data.MsgSenderAccountId;
                reqAM.OperationType = data.OperationType;
                reqAM.ProxyType = data.ProxyType;
                reqAM.ProxyValue = data.ProxyValue;
                reqAM.RegistrationID = data.RegistrationID;
                reqAM.DisplayName = data.DisplayName;
                reqAM.ProxyBankID = data.ProxyBankID;
                reqAM.AccountID = data.AccountID;
                reqAM.AccountType = data.AccountType;
                reqAM.AccountName = data.AccountName;
                reqAM.SecondaryIDType = data.SecondaryIDType;
                reqAM.SecondaryIDValue = data.SecondaryIDValue;
                reqAM.RegistrationStatus = data.RegistrationStatus;
                reqAM.CustomerType = data.CustomerType;
                reqAM.CustomerID = data.CustomerID;
                reqAM.CustomerResidentStatus = data.CustomerResidentStatus;
                reqAM.CustomerTownName = data.CustomerTownName;

            }
            catch (Exception)
            {
                throw;
            }

            string jsonRequest = JsonConvert.SerializeObject(reqAM), jsonResponse, num = reqAM.TranRefNUM, idr = reqAM.BizMsgIdr;
            jsonResponse = hp.GenerateReq(reqAM, "http://10.99.0.72:8355/jsonAPI/prxy001");

            
            

            if ( hp.Ck(reqAM.SendingSystemBIC) && hp.Ck(reqAM.ReceivingSystemBIC) &&  hp.Ck(reqAM.BizMsgIdr) && hp.Ck(reqAM.CreationDateTime) && hp.Ck(reqAM.TranRefNUM) && hp.Ck(reqAM.MsgCreationDate) && hp.Ck(reqAM.SendingParticipantID) && hp.Ck(reqAM.MsgSenderAccountId) && hp.Ck(reqAM.OperationType) && hp.Ck(reqAM.ProxyType) && hp.Ck(reqAM.ProxyValue) && hp.Ck(reqAM.ProxyBankID) && hp.Ck(reqAM.AccountID) && hp.Ck(reqAM.AccountType) && hp.Ck(reqAM.SecondaryIDType) && hp.Ck(reqAM.SecondaryIDValue) && hp.Ck(reqAM.SendingSystemBIC) && jsonResponse.Contains("prxy.002.001.01"))
            {

                //success
                respAM = JsonConvert.DeserializeObject<RespAliasManagement>(jsonResponse);
                st = "Success";
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(respAM);

                
            }
            else if(jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
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
        public IHttpActionResult AliasResolution([FromBody] AliasResolutionVM data)
        {
            ReqAliasResolution reqAR = new ReqAliasResolution();
            RespAliasResolution respAR = new RespAliasResolution();
            RespRejectAliasResolution rejAR = new RespRejectAliasResolution();
            RespErrAliasResolution errAR = new RespErrAliasResolution();

            var ToInt = int.Parse(_db.ActivityLogs.Select(i => i.Id).Count().ToString()) + 1;
            var lastID = _db.ActivityLogs.Select(x => x.Id).Any() ? ToInt.ToString() : null;

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

                reqAR.SendingSystemBIC = data.SBIC;
                reqAR.ReceivingSystemBIC = data.RBIC;
                reqAR.BizMsgIdr = Date + data.SBIC + data.TransactionType + data.Originator + data.ChannelType + ss;
                reqAR.MsgDefIdr = data.MsgDefIdr;
                reqAR.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.ssZ");
                reqAR.TranRefNUM = Date + data.SBIC + data.TransactionType + ss;
                reqAR.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sss");
                reqAR.SendingParticipantID = data.SendingParticipantID;
                reqAR.MsgSenderAccountId = data.MsgSenderAccountId;
                reqAR.AlisaResolutionLookup = data.AlisaResolutionLookup;
                reqAR.UniqueRequestID = Date + data.SBIC + data.Originator + ss;
                reqAR.ProxyType = data.ProxyType;
                reqAR.ProxyValue = data.ProxyValue;
            }
            catch (Exception)
            {

                throw;
            }

            string jsonRequest  = JsonConvert.SerializeObject(reqAR), jsonResponse, num = reqAR.TranRefNUM, idr = reqAR.BizMsgIdr;
            jsonResponse = hp.GenerateReq(reqAR, "http://10.99.0.72:8355/jsonAPI/prxy003");
         

            if (hp.Ck(reqAR.SendingSystemBIC) && hp.Ck(reqAR.ReceivingSystemBIC) && hp.Ck(reqAR.BizMsgIdr) && hp.Ck(reqAR.MsgDefIdr) && hp.Ck(reqAR.CreationDateTime) && hp.Ck(reqAR.TranRefNUM) && hp.Ck(reqAR.MsgCreationDate) && hp.Ck(reqAR.SendingParticipantID) && hp.Ck(reqAR.MsgSenderAccountId) && hp.Ck(reqAR.AlisaResolutionLookup) && hp.Ck(reqAR.UniqueRequestID) && hp.Ck(reqAR.ProxyType) && hp.Ck(reqAR.ProxyValue) && jsonResponse.Contains("prxy.004.001.01"))
            {
                //success
                st = "Success";
                respAR = JsonConvert.DeserializeObject<RespAliasResolution>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(respAR);

            }
            else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
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
        public IHttpActionResult AliasRegistrationInquiry([FromBody] AliasRegInquiryVM data)
        {
            ReqAliasRegInquiry reqARI = new ReqAliasRegInquiry();
            RespAliasRegInquiry respARI = new RespAliasRegInquiry();
            RespRejectAliasRegInquiry rejARI = new RespRejectAliasRegInquiry();
            RespErrAliasRegInquiry errARI = new RespErrAliasRegInquiry();

            var ToInt = int.Parse(_db.ActivityLogs.Select(i => i.Id).Count().ToString()) + 1;
            var lastID = _db.ActivityLogs.Select(x => x.Id).Any() ? ToInt.ToString() : null;

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

                reqARI.SendingSystemBIC = data.SBIC;
                reqARI.ReceivingSystemBIC = data.RBIC;
                reqARI.BizMsgIdr = Date + data.SBIC + data.TransactionType + data.Originator + data.ChannelType + ss;
                reqARI.MsgDefIdr = data.MsgDefIdr;
                reqARI.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.ssZ");
                reqARI.TranRefNUM = Date + data.SBIC + data.TransactionType + ss;
                reqARI.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sss");
                reqARI.SendingParticipantID = data.SendingParticipantID;
                reqARI.MsgSenderAccountId = data.MsgSenderAccountId;
                reqARI.RegistrationID = data.RegistrationID;
                reqARI.SecondaryIDType = data.SecondaryIDType;
                reqARI.SecondaryIDValue = data.SecondaryIDValue;
            }
            catch (Exception)
            {

                throw;
            }

            string jsonRequest = JsonConvert.SerializeObject(reqARI), jsonResponse, num = reqARI.TranRefNUM, idr = reqARI.BizMsgIdr; ;
            jsonResponse = hp.GenerateReq(reqARI, "http://10.99.0.72:8355/jsonAPI/prxy005");
            //jsonResponse = hp.GenerateReq(reqARI, "http://172.18.99.30:4343/jsonAPI/prxy005");



            if (hp.Ck(reqARI.SendingSystemBIC) && hp.Ck(reqARI.ReceivingSystemBIC) && hp.Ck(reqARI.BizMsgIdr) && hp.Ck(reqARI.MsgDefIdr) && hp.Ck(reqARI.CreationDateTime) && hp.Ck(reqARI.TranRefNUM) && hp.Ck(reqARI.MsgCreationDate) && hp.Ck(reqARI.SendingParticipantID) && hp.Ck(reqARI.MsgSenderAccountId) && jsonResponse.Contains("prxy.006.001.01"))
            {
                //success
                st = "Success";
                respARI = JsonConvert.DeserializeObject<RespAliasRegInquiry>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(respARI);
            }
            else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
            {
                //error
                st = "Error";
                errARI = JsonConvert.DeserializeObject<RespErrAliasRegInquiry>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                return Ok(errARI);
            }
            else
            {
                //reject
                st = "Reject";
                rejARI = JsonConvert.DeserializeObject<RespRejectAliasRegInquiry>(jsonResponse);
                hp.SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
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
            jsonResponse = hp.GenerateReq(reqAN, "https://localhost:44350/jsonAPI/prxy901");

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
