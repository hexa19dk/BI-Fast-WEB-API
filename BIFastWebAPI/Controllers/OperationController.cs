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
using System.Globalization;
//using System.Text.Json;

namespace BIFastWebAPI.Controllers
{
    public class OperationController : ApiController
    {

        private readonly Helper hp = new Helper();
        string st = "", ss = "",chan, Date = DateTime.Now.ToString("yyyyMMdd");
        object a;
        ApplicationDbContext _db = new ApplicationDbContext();



        #region AliasManagement
       

        [HttpPost]
        [Route("jsonAPI/prxy001")]
        public IHttpActionResult AliasManagement([FromBody] AliasManagementVM data)
        {
            a = hp.AliasManagement(data);
            return Ok(a);
        }
        #endregion

        #region AliasResolution
        [HttpPost]
        [Route("jsonAPI/prxy003")]
        public IHttpActionResult AliasResolution([FromBody] AliasResolutionVM data)
        {
            a = hp.AliasResolution(data);
            return Ok(a);
        }
        #endregion

        #region AliasRegistrationInquiry
        [HttpPost]
        [Route("jsonAPI/prxy005")]
        public IHttpActionResult AliasRegistrationInquiry([FromBody] AliasRegInquiryVM data)
        {
            a = hp.AliasRegInquiry(data);
            return Ok(a);
        }
        #endregion

        #region AliasNotification
        [HttpPost]
        [Route("jsonAPI/prxy901")]
        public IHttpActionResult AliasNotification([FromBody] ReqAliasNotification reqAN)
        {
            string jsonRequest = JsonConvert.SerializeObject(reqAN);
            string jsonResponse = "ALIAS_NOTIFICATION";

            
            if (hp.Ck(reqAN.SendingSystemBIC) && hp.Ck(reqAN.ReceivingSystemBIC) && hp.Ck(reqAN.BizMsgIdr) && hp.Ck(reqAN.MsgDefIdr) && hp.Ck(reqAN.CreationDateTime) && hp.Ck(reqAN.TranRefNUM) && hp.Ck(reqAN.MsgCreationDate) && hp.Ck(reqAN.SendingParticipantID) && hp.Ck(reqAN.OrigUniqueRequestID) && hp.Ck(reqAN.ProxyType) && hp.Ck(reqAN.ProxyValue) && hp.Ck(reqAN.OriginRegistrationID) && hp.Ck(reqAN.OriginDisplayName) && hp.Ck(reqAN.OriginProxyBankID) && hp.Ck(reqAN.OriginAccountID) && hp.Ck(reqAN.OriginAccountType) && hp.Ck(reqAN.NewRegistrationID) && hp.Ck(reqAN.NewDisplayName) && hp.Ck(reqAN.NewProxyBankID) && hp.Ck(reqAN.NewAccountID) && hp.Ck(reqAN.NewAccountType))
            {
                hp.SaveLog("SVIP", "SVIP", null, reqAN.ProxyValue, reqAN.TranRefNUM, reqAN.BizMsgIdr, jsonRequest, jsonResponse, "Sukses", DateTime.Parse(reqAN.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Now);
                return Ok();
            }
            
            else
            {
                //error
                hp.SaveLog("SVIP", "SVIP", null, reqAN.ProxyValue, reqAN.TranRefNUM, reqAN.BizMsgIdr, null, null, "Error", DateTime.Parse(reqAN.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Now);
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

        #region GetRegistrationID

        [Route("jsonAPI/getReqID")]
        public IHttpActionResult GetRegID(string pv, string CIF, string KTP, string Norek)
        {
            var regId = hp.GetRegID(pv, CIF, KTP, Norek);
            if (regId != null)
            {
                return Ok(regId);
            }
            else
            {
                return Ok("RegId Tidak ditemukan");
            }
            
        }

        #endregion

        #region GetBankMaster

        [Route("jsonAPI/getBankMaster")]

        public IHttpActionResult GetBankMaster()
        {
            List<BankMaster> list = hp.GetAllBankMaster();
            if (list == null)
            {
                return Ok("Tidak ada List Bank");
            }
            else
            {
                return Ok(list);
            }

        }

        #endregion
    }

}
