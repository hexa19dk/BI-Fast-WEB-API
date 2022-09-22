using BIFastWebAPI.Data;
using BIFastWebAPI.Data.Models;
using BIFastWebAPI.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Serialization;
using System.Xml;

namespace BIFastWebAPI.Utility
{
    public class Helper
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        RegDbContext _dbr = new RegDbContext();
        string st = "", ss = "", chan, tt = "", Date = DateTime.Now.ToString("yyyyMMdd"), rID = "";
        object rrr;
        Random r = new Random();

        #region Check Isnull?
        public bool Ck(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Get Sequence


        public string GetSeq()
        {
            ////string sss = GenerateGet("http://www.randomnumberapi.com/api/v1.0/random?min=0&max=99999999&count=1").Remove(0,1);
            ////string ssa = sss.Remove(sss.Length - 1, 1);

            //lock (r)
            //{
            //var zeroDate = DateTime.Now;
            //int uniqueId = (int)(zeroDate.Ticks / 10000);
            ////int rInt = r.Next(0, 99999999);
            //    ss = uniqueId.ToString().PadLeft(8, '0');

            //    return ss;
            //}
            ////string ss = ssa.PadLeft(8, '0');
            ///
            Random r = new Random(Guid.NewGuid().GetHashCode());
            string ss = r.Next(0, 99999999).ToString().PadLeft(8, '0');
            return ss;
        }   
        #endregion

        #region store to Registration DB
        public void SaveRegID(string regID, string CIF, string KTP, string Norek, string proxyValue, string proxyType, string ch)
        {
            var dt = new RegistrationData();
            dt.RegistrationID = regID;
            dt.CIF = CIF;
            dt.KTP = KTP;
            dt.NoRek = Norek;
            dt.ProxyValue = proxyValue;
            dt.ProxyType = proxyType;
            dt.Channel = ch;

            dt.CreatedDate = DateTime.Now;
            RegistrationData dtb = _dbr.RegistrationDatas.FirstOrDefault(
                   m => m.RegistrationID == regID);

            if (dtb == null)
            {
                _dbr.RegistrationDatas.Add(dt);
                _dbr.SaveChanges();
            }
            else
            {
                return;
            }
        }
        #endregion

        #region GetRegId

        public string GetRegID(string pv, string CIF, string KTP, string Norek)
        {
            var rd = _dbr.RegistrationDatas.FirstOrDefault(o => o.ProxyValue == pv && o.CIF == CIF && o.KTP == KTP && o.NoRek == Norek);
            return rd.RegistrationID;
        }

        public string GetRegIDKTP(string CIF, string KTP, string Norek)
        {
            var rdk = _dbr.RegistrationDatas.FirstOrDefault(o => o.CIF == CIF && o.KTP == KTP && o.NoRek == Norek);
            if (rdk != null)
            {
                return rdk.RegistrationID;
            }
            return "";
        }



        #endregion

        public List<BankMaster> GetAllBankMaster()
        {
            var list = _dbr.BankMasters.ToList();
            return list;
        }


        #region Savelog
        public void SaveLog(string CIF, string chan, string act, string pv, string num, string idr, object jsonRequest, object jsonResponse, string st, DateTime reqModel, DateTime respModel)
        {
            var log = new ActivityLog();
            log.Channel = chan;
            log.AMActivity = act;
            log.ProxyValue = pv;
            log.OrigTranRefNUM = num;
            log.BizMsgIdr = idr;
            log.EndPoint = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            log.Type = "POST";
            log.CIF = CIF;
            log.ReqMessage = jsonRequest.ToString();
            log.RespMessage = jsonResponse.ToString();
            log.Status = st;
            log.LogDate = DateTime.Now;
            log.ReqDate = reqModel;
            log.RespDate = respModel;
            _db.ActivityLogs.Add(log);
            _db.SaveChanges();
        }
        #endregion

        #region GenerateRequest
        public string GenerateReq(object reqModel, string link)
        {
            string jsonRequest = JsonConvert.SerializeObject(reqModel);
            var client = new RestClient(link);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", jsonRequest, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string jsonResponse = response.Content;
            return jsonResponse;
        }

        public string GenerateGet(string link)
        {
            var client = new RestClient(link);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string res = response.Content;
            return res;

        }
        #endregion


        #region Non Transaction Function Helper

        #region Alias Management
        public Object AliasManagement(AliasManagementVM data)
        {
            AliasManagementResponses respAll = new AliasManagementResponses();
            ReqAliasManagement reqAM = new ReqAliasManagement();
            RespAliasManagement respAM = new RespAliasManagement();
            RespRejectAliasManagement rejAM = new RespRejectAliasManagement();
            RespErrAliasManagement errAM = new RespErrAliasManagement();

            string ss = GetSeq();
            try
            {
                if (data.OperationType == "NEWR")
                {
                    tt = "710";
                    rID = "";
                }
                else
                {
                    tt = "720";
                    rID = GetRegID(data.ProxyValue, data.CIF, data.SecondaryIDValue, data.MsgSenderAccountId);
                }

                reqAM.SendingSystemBIC = "AGTBIDJA";
                reqAM.ReceivingSystemBIC = "FASTIDJA";
                reqAM.BizMsgIdr = Date + "AGTBIDJA" + tt + "O" + data.ChannelType + ss;
                reqAM.MsgDefIdr = "prxy.001.001.01";
                reqAM.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.ssZ");
                reqAM.TranRefNUM = Date + "AGTBIDJA" + tt + ss;
                reqAM.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sss");
                reqAM.SendingParticipantID = "AGTBIDJA";
                reqAM.MsgSenderAccountId = data.MsgSenderAccountId;
                reqAM.OperationType = data.OperationType;
                reqAM.ProxyType = data.ProxyType;
                reqAM.ProxyValue = data.ProxyValue;
                reqAM.RegistrationID = rID;
                reqAM.DisplayName = data.DisplayName;
                reqAM.ProxyBankID = "AGTBIDJA";
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


                string jsonRequest = JsonConvert.SerializeObject(reqAM), jsonResponse, num = reqAM.TranRefNUM, idr = reqAM.BizMsgIdr;
                //jsonResponse = GenerateReq(reqAM, "http://10.99.0.72:8355/jsonAPI/prxy001");
                jsonResponse = GenerateReq(reqAM, "http://10.99.0.3:8355/jsonAPI/prxy001");
                //jsonResponse = GenerateReq(reqAM, "http://10.99.48.46:8355/jsonAPI/prxy001");

                respAll = JsonConvert.DeserializeObject<AliasManagementResponses>(jsonResponse);

                if (respAll.MsgDefIdr == "prxy.002.001.01" && respAll.StatusReasonCode == "U000")
                {
                    //success
                    respAM = JsonConvert.DeserializeObject<RespAliasManagement>(jsonResponse);
                    st = "Success";
                    SaveLog(data.CIF, data.Channel, data.OperationType, data.ProxyValue, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    SaveRegID(respAll.RegistrationID, data.CIF, reqAM.SecondaryIDValue, reqAM.MsgSenderAccountId, reqAM.ProxyValue, reqAM.ProxyType, data.Channel);
                    rrr = respAM;

                }

                else if (respAll.MsgDefIdr == "admi.002.001.01" && respAll.ErrorLocation != null)
                {
                    //error
                    st = "Error";
                    errAM = JsonConvert.DeserializeObject<RespErrAliasManagement>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, data.OperationType, data.ProxyValue, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = errAM;

                }
                else
                {
                    //reject
                    st = "Reject";
                    rejAM = JsonConvert.DeserializeObject<RespRejectAliasManagement>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, data.OperationType, data.ProxyValue, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = rejAM;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }
            return rrr;
        }
        #endregion

        #region Alias Resolution

        //U807
        public Object AliasResolution(AliasResolutionVM data)
        {
            AliasResolutionResponses respAll = new AliasResolutionResponses();
            ReqAliasResolution reqAR = new ReqAliasResolution();
            RespAliasResolution respAR = new RespAliasResolution();
            RespRejectAliasResolution rejAR = new RespRejectAliasResolution();
            RespErrAliasResolution errAR = new RespErrAliasResolution();

            string ss = GetSeq();

            try
            {
                reqAR.SendingSystemBIC = "AGTBIDJA";
                reqAR.ReceivingSystemBIC = "FASTIDJA";
                reqAR.BizMsgIdr = Date + "AGTBIDJA610O" + data.ChannelType + ss;
                reqAR.MsgDefIdr = "prxy.003.001.01";
                reqAR.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.ssZ");
                reqAR.TranRefNUM = Date + "AGTBIDJA610" + ss;
                reqAR.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sss");
                reqAR.SendingParticipantID = "AGTBIDJA";
                reqAR.MsgSenderAccountId = data.MsgSenderAccountId;
                reqAR.AlisaResolutionLookup = data.AlisaResolutionLookup;
                reqAR.UniqueRequestID = Date + "AGTBIDJAO" + ss;
                reqAR.ProxyType = data.ProxyType;
                reqAR.ProxyValue = data.ProxyValue;

                string jsonRequest = JsonConvert.SerializeObject(reqAR), jsonResponse, num = reqAR.TranRefNUM, idr = reqAR.BizMsgIdr;
                //jsonResponse = GenerateReq(reqAR, "http://10.99.0.72:8355/jsonAPI/prxy003");
                jsonResponse = GenerateReq(reqAR, "http://10.99.0.3:8355/jsonAPI/prxy003");
                //jsonResponse = GenerateReq(reqAR, "http://10.99.48.46:8355/jsonAPI/prxy003");

                respAll = JsonConvert.DeserializeObject<AliasResolutionResponses>(jsonResponse);

                if (respAll.MsgDefIdr == "prxy.004.001.01" && respAll.StatusReasonCode == "U000")
                {
                    //success
                    st = "Success";
                    respAR = JsonConvert.DeserializeObject<RespAliasResolution>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = respAR;
                }
                else if (respAll.ErrorLocation != null || respAll.MsgDefIdr == "admi.002.001.01")
                {
                    //error
                    st = "Error";
                    errAR = JsonConvert.DeserializeObject<RespErrAliasResolution>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = errAR;
                }
                else
                {
                    //reject
                    st = "Reject";
                    rejAR = JsonConvert.DeserializeObject<RespRejectAliasResolution>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = rejAR;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }
            return rrr;
        }

        #endregion

        #region Alias Registration inquiry
        public Object AliasRegInquiry(AliasRegInquiryVM data)
        {
            AliasRegInquiryResponses respAll = new AliasRegInquiryResponses();
            ReqAliasRegInquiry reqARI = new ReqAliasRegInquiry();
            RespAliasRegInquiry respARI = new RespAliasRegInquiry();
            RespRejectAliasRegInquiry rejARI = new RespRejectAliasRegInquiry();
            RespErrAliasRegInquiry errARI = new RespErrAliasRegInquiry();

            string ss = GetSeq();
            rID = GetRegIDKTP(data.CIF, data.SecondaryIDValue, data.MsgSenderAccountId);


            try
            {

                reqARI.SendingSystemBIC = "AGTBIDJA";
                reqARI.ReceivingSystemBIC = "FASTIDJA";
                reqARI.BizMsgIdr = Date + "AGTBIDJA620O" + data.ChannelType + ss;
                reqARI.MsgDefIdr = "prxy.005.001.01";
                reqARI.CreationDateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.ssZ");
                reqARI.TranRefNUM = Date + "AGTBIDJA620" + ss;
                reqARI.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sss");
                reqARI.SendingParticipantID = "AGTBIDJA";
                reqARI.MsgSenderAccountId = data.MsgSenderAccountId;
                reqARI.RegistrationID = rID;
                reqARI.SecondaryIDType = data.SecondaryIDType;
                reqARI.SecondaryIDValue = data.SecondaryIDValue;

                string jsonRequest = JsonConvert.SerializeObject(reqARI), jsonResponse, num = reqARI.TranRefNUM, idr = reqARI.BizMsgIdr; ;
                //jsonResponse = GenerateReq(reqARI, "http://10.99.0.72:8355/jsonAPI/prxy005");
                jsonResponse = GenerateReq(reqARI, "http://10.99.0.3:8355/jsonAPI/prxy005");
                //jsonResponse = GenerateReq(reqARI, "http://10.99.48.46:8355/jsonAPI/prxy005");
                respAll = JsonConvert.DeserializeObject<AliasRegInquiryResponses>(jsonResponse);

                if (respAll.MsgDefIdr == "prxy.006.001.01" && respAll.StatusReasonCode == "U000")
                {
                    //success
                    st = "Success";
                    respARI = JsonConvert.DeserializeObject<RespAliasRegInquiry>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = respARI;
                }
                else if (respAll.ErrorLocation != null || respAll.BizMsgIdr == "admi.002.001.01")
                {
                    //error
                    st = "Error";
                    errARI = JsonConvert.DeserializeObject<RespErrAliasRegInquiry>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = errARI;
                }
                else
                {
                    //reject
                    st = "Reject";
                    rejARI = JsonConvert.DeserializeObject<RespRejectAliasRegInquiry>(jsonResponse);
                    SaveLog(data.CIF, data.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    rrr = rejARI;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }
            return rrr;
        }
        #endregion

        #region Alias Notification
        //code
        #endregion

        #region System Notification
        //code
        #endregion


        #endregion

        #region Transaction Function Helper

        #region Account Enquiry
        public Object AccountEnquiry(ViewModelAccount vmAcc)
        {
            RespAllAccount respAll = new RespAllAccount();
            ReqAccountEnquiry reqAcc = new ReqAccountEnquiry();
            RespAccEnquiry respAcc = new RespAccEnquiry();
            RespRejectAccEnquiry rejcAcc = new RespRejectAccEnquiry();
            RespErrAccEnquiry errAcc = new RespErrAccEnquiry();
            string ss = GetSeq();

            try
            {
                //reqAcc.EndToEndId = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "510" + "O" + vmAcc.ChannelType + ss;
                reqAcc.EndToEndId = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "510" + "O" + vmAcc.ChannelType + ss;
                reqAcc.MsgDefIdr = "pacs.008.001.08";
                reqAcc.TranRefNUM = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "510" + ss;
                reqAcc.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.sss");
                reqAcc.RecipentParticipantID = vmAcc.RecipentParticipantID;
                reqAcc.CreditorAccountNo = vmAcc.CreditorAccountNo;
                reqAcc.Amount = vmAcc.Amount + ".00";
                reqAcc.Currency = "IDR";
                reqAcc.PurposeType = vmAcc.PurposeType;

                string jsonRequest = JsonConvert.SerializeObject(reqAcc), idr = reqAcc.EndToEndId, num = reqAcc.TranRefNUM;
                //string jsonResponse = GenerateReq(reqAcc, "http://10.99.0.72:8355/jsonAPI/AccountEnquiry");
                string jsonResponse = GenerateReq(reqAcc, "http://10.99.0.3:8355/jsonAPI/AccountEnquiry");
                ////string jsonResponse = GenerateReq(reqAcc, "http://10.99.48.46:8355/jsonAPI/AccountEnquiry");

                respAll = JsonConvert.DeserializeObject<RespAllAccount>(jsonResponse);

                if (respAll.MsgDefIdr == "pacs.002.001.10" && respAll.ReasonCode == "U000")
                {
                    respAcc = JsonConvert.DeserializeObject<RespAccEnquiry>(jsonResponse);
                    st = "Success";
                    SaveLog(vmAcc.CIF, vmAcc.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(respAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind));

                    rrr = respAcc;
                }
                else if (respAll.MsgDefIdr == "admi.002.001.01" && respAll.ErrorLocation != null)
                {
                    errAcc = JsonConvert.DeserializeObject<RespErrAccEnquiry>(jsonResponse);
                    st = "Error";
                    SaveLog(vmAcc.CIF, vmAcc.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errAcc.RejectDateTime, null, DateTimeStyles.RoundtripKind));
                    rrr = errAcc;
                }
                else
                {
                    rejcAcc = JsonConvert.DeserializeObject<RespRejectAccEnquiry>(jsonResponse);
                    st = "Reject";

                    SaveLog(vmAcc.CIF, vmAcc.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejcAcc.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = rejcAcc;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Request" + ex.Message);
            }

            return rrr;
        }
        #endregion

        #region Credit Transfer
        public Object CreditTransferAll(ViewModelTransaction VmTrx)
        {
            RespCreditTrfAll respall = new RespCreditTrfAll();
            ReqCreditTransfer req = new ReqCreditTransfer();
            RespCreditTransfer resp = new RespCreditTransfer();
            RejectCreditTransfer rejCt = new RejectCreditTransfer();
            ErrorCreditTransfer errCt = new ErrorCreditTransfer();
            string ss = GetSeq();

            try
            {
                req.EndToEndId = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "010" + "O" + VmTrx.ChannelType + ss;
                req.MsgDefIdr = "pacs.008.001.08";
                req.TranRefNUM = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "010" + ss;
                req.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.sss");
                req.Amount = VmTrx.Amount + ".00";
                req.Currency = "IDR";
                req.PurposeType = VmTrx.PurposeType;
                req.PaymentInformation = "TR0 BIFAST - " + VmTrx.PaymentInformation;
                //req.PaymentInformation = VmTrx.PaymentInformation;
                req.SendingParticipantID = VmTrx.SendingParticipantID;
                req.DebitorAccountNo = VmTrx.DebitorAccountNo;
                req.DebitorAccountType = VmTrx.DebitorAccountType;
                req.DebitorAccountName = VmTrx.DebitorAccountName;
                req.DebitorType = VmTrx.DebitorType;
                req.DebitorID = VmTrx.DebitorID;
                req.DebitorResidentStatus = VmTrx.DebitorResidentStatus;
                req.DebitorTownName = "0300";
                req.RecipentParticipantID = VmTrx.RecipentParticipantID;
                req.CreditorAccountNo = VmTrx.CreditorAccountNo;
                req.CreditorAccountType = VmTrx.CreditorAccountType;
                req.CreditorAccountName = VmTrx.CreditorAccountName;
                req.CreditorType = VmTrx.CreditorType;
                req.CreditorID = VmTrx.CreditorID;
                req.CreditorResidentStatus = VmTrx.CreditorResidentStatus;
                req.CreditorTownName = "0300";

                string jsonRequest = JsonConvert.SerializeObject(req), idr = req.EndToEndId, num = req.TranRefNUM;
                //string jsonResponse = GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/CreditTransfer");
                //string jsonResponse = GenerateReq(req, "http://10.99.0.3:8355/jsonAPI/CreditTransfer");
                string jsonResponse = GenerateReq(req, "http://10.99.48.46:8355/jsonAPI/CreditTransfer");
                respall = JsonConvert.DeserializeObject<RespCreditTrfAll>(jsonResponse);

                if (respall.MsgDefIdr == "pacs.002.001.10" && respall.ReasonCode == "U000")
                {
                    resp = JsonConvert.DeserializeObject<RespCreditTransfer>(jsonResponse);
                    st = "Success";
                    SaveLog(VmTrx.CIF, VmTrx.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(resp.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = resp;
                }
                else if (respall.MsgDefIdr == "admi.002.001.01")
                {
                    errCt = JsonConvert.DeserializeObject<ErrorCreditTransfer>(jsonResponse);
                    st = "Error";
                    SaveLog(VmTrx.CIF, VmTrx.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errCt.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                    rrr = errCt;
                }
                else
                {
                    rejCt = JsonConvert.DeserializeObject<RejectCreditTransfer>(jsonResponse);
                    st = "Reject";
                    SaveLog(VmTrx.CIF, VmTrx.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejCt.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = rejCt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Request" + ex.Message);
            }

            return rrr;
        }
        #endregion

        #region Credit Transfer To Proxy
        public Object CreditToProxy(ViewModelProxy vmProx)
        {
            ReqCreditTransferToProxy reqCtPrx = new ReqCreditTransferToProxy();
            RespCrediTransferToProxy resCtPrx = new RespCrediTransferToProxy();
            RespRejectCreditTransferToProxy rejCtPrx = new RespRejectCreditTransferToProxy();
            RespErrCreditTransferToProxy errCtPrx = new RespErrCreditTransferToProxy();
            RespAllCreditProxy respAll = new RespAllCreditProxy();
            string ss = GetSeq();

            try
            {
                reqCtPrx.EndToEndId = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "110" + "O" + vmProx.ChannelType + ss;
                reqCtPrx.MsgDefIdr = "pacs.008.001.08";
                reqCtPrx.TranRefNUM = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "110" + ss;
                reqCtPrx.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.sss");
                reqCtPrx.Amount = vmProx.Amount + ".00";
                reqCtPrx.Currency = "IDR";
                reqCtPrx.PurposeType = vmProx.PurposeType;
                reqCtPrx.PaymentInformation = vmProx.PaymentInformation;
                reqCtPrx.SendingParticipantID = vmProx.SendingParticipantID;
                reqCtPrx.DebitorAccountNo = vmProx.DebitorAccountNo;
                reqCtPrx.DebitorAccountType = vmProx.DebitorAccountType;
                reqCtPrx.DebitorAccountName = vmProx.DebitorAccountName;
                reqCtPrx.DebitorType = vmProx.DebitorType;
                reqCtPrx.DebitorID = vmProx.DebitorID;
                reqCtPrx.DebitorResidentStatus = vmProx.DebitorResidentStatus;
                reqCtPrx.DebitorTownName = "0300";
                reqCtPrx.RecipentParticipantID = vmProx.RecipentParticipantID;
                reqCtPrx.CreditorAccountNo = vmProx.CreditorAccountNo;
                reqCtPrx.CreditorAccountType = vmProx.CreditorAccountType;
                reqCtPrx.CreditorAccountName = vmProx.CreditorAccountName;
                reqCtPrx.ProxyValue = vmProx.ProxyValue;
                if (vmProx.ProxyValue.Contains("@"))
                {
                    reqCtPrx.ProxyType = "02";
                }
                else
                {
                    reqCtPrx.ProxyType = "01";
                }
                reqCtPrx.CreditorType = vmProx.CreditorType;
                reqCtPrx.CreditorID = vmProx.CreditorID;
                reqCtPrx.CreditorResidentStatus = vmProx.CreditorResidentStatus;
                reqCtPrx.CreditorTownName = "0300";

                string jsonRequest = JsonConvert.SerializeObject(reqCtPrx), idr = reqCtPrx.EndToEndId, num = reqCtPrx.TranRefNUM;
                //string jsonResponse = GenerateReq(reqCtPrx, "http://10.99.0.72:8355/jsonAPI/CreditTransferToProxy");
                //string jsonResponse = GenerateReq(reqCtPrx, "http://10.99.0.3:8355/jsonAPI/CreditTransferToProxy");
                string jsonResponse = GenerateReq(reqCtPrx, "http://10.99.48.46:8355/jsonAPI/CreditTransferToProxy");
                respAll = JsonConvert.DeserializeObject<RespAllCreditProxy>(jsonResponse);

                if (respAll.MsgDefIdr == "pacs.002.001.10" && respAll.ReasonCode == "U000")
                {
                    resCtPrx = JsonConvert.DeserializeObject<RespCrediTransferToProxy>(jsonResponse);
                    st = "Success";
                    SaveLog(vmProx.CIF, vmProx.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(resCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = resCtPrx;
                }
                else if (respAll.MsgDefIdr == "admi.002.001.01")
                {
                    errCtPrx = JsonConvert.DeserializeObject<RespErrCreditTransferToProxy>(jsonResponse);
                    st = "Error";
                    SaveLog(vmProx.CIF, vmProx.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errCtPrx.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                    rrr = errCtPrx;
                }
                else
                {
                    rejCtPrx = JsonConvert.DeserializeObject<RespRejectCreditTransferToProxy>(jsonResponse);
                    st = "Reject";
                    SaveLog(vmProx.CIF, vmProx.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejCtPrx.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = rejCtPrx;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Request" + ex.Message);
            }

            return rrr;
        }
        #endregion

        #region Reversal Credit Transfer
        public Object Reversal(ViewModelReversal vmRev)
        {
            ReqReversalCreditTransfer req = new ReqReversalCreditTransfer();
            RespReversalCreditTransfer res = new RespReversalCreditTransfer();
            RespRejectRCT rej = new RespRejectRCT();
            RespErrRCT err = new RespErrRCT();
            RespAllReversal respAll = new RespAllReversal();
            string ss = GetSeq();
            try
            {
                req.EndToEndId = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "010" + "O" + vmRev.ChannelType + ss;
                req.MsgDefIdr = "pacs.008.001.08";
                req.TranRefNUM = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "010" + ss;
                req.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.sss");
                req.Amount = vmRev.Amount + ".00";
                req.Currency = "IDR";
                req.PurposeType = vmRev.PurposeType;
                req.PaymentInformation = vmRev.PaymentInformation;
                req.SendingParticipantID = "AGTBIDJA";
                req.DebitorAccountNo = vmRev.DebitorAccountNo;
                req.DebitorAccountType = vmRev.DebitorAccountType;
                req.DebitorAccountName = vmRev.DebitorAccountName;
                req.DebitorType = vmRev.DebitorType;
                req.DebitorID = vmRev.DebitorID;
                req.DebitorResidentStatus = vmRev.DebitorResidentStatus;
                req.DebitorTownName = "0300";
                req.RecipentParticipantID = vmRev.RecipentParticipantID;
                req.CreditorAccountNo = vmRev.CreditorAccountNo;
                req.CreditorAccountType = vmRev.CreditorAccountType;
                req.CreditorAccountName = vmRev.CreditorAccountName;
                req.CreditorType = vmRev.CreditorType;
                req.CreditorID = vmRev.CreditorID;
                req.CreditorResidentStatus = vmRev.CreditorResidentStatus;
                req.CreditorTownName = "0300";
                req.PaymentInformation = vmRev.PaymentInformation;
                req.RltdEndToEndId = vmRev.RltdEndToEndId;

                string jsonRequest = JsonConvert.SerializeObject(req), idr = req.EndToEndId, num = req.TranRefNUM;
                //string jsonResponse = Hp.GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/ReversalCreditTransfer");
                string jsonResponse = GenerateReq(req, "http://10.99.0.3:8355/jsonAPI/ReversalCreditTransfer");
                respAll = JsonConvert.DeserializeObject<RespAllReversal>(jsonResponse);

                if (respAll.MsgDefIdr == "pacs.002.001.10" && respAll.ReasonCode == "U000")
                {
                    res = JsonConvert.DeserializeObject<RespReversalCreditTransfer>(jsonResponse);
                    st = "Success";
                    SaveLog(vmRev.CIF, vmRev.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(res.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = res;
                }
                else if (respAll.MsgDefIdr == "admi.002.001.01")
                {
                    err = JsonConvert.DeserializeObject<RespErrRCT>(jsonResponse);
                    st = "Error";
                    SaveLog(vmRev.CIF, vmRev.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(err.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                    rrr = err;
                }
                else
                {
                    rej = JsonConvert.DeserializeObject<RespRejectRCT>(jsonResponse);
                    st = "Reject";
                    SaveLog(vmRev.CIF, vmRev.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rej.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = rej;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Request" + ex.Message);
            }

            return rrr;
        }
        #endregion

        #region Payment Status
        public Object PaymentSt(VmPayStat vmSt)
        {
            ReqPaymentStatus req = new ReqPaymentStatus();
            RespPaymentStatus res = new RespPaymentStatus();
            RespRejectPaymentStatus rej = new RespRejectPaymentStatus();
            RespErrPaymentStatus err = new RespErrPaymentStatus();
            RespAllPaymentStatus respAll = new RespAllPaymentStatus();
            string ss = GetSeq();

            try
            {
                //req.TranRefNUM = vmSt.TranRefNUM; // inputan dari request CT
                req.TranRefNUM = DateTime.Now.ToString("yyyyMMdd") + "AGTBIDJA" + "000" + ss;
                req.MsgCreationDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:MM:ss.sss");
                req.OrigEndToEndId = vmSt.OrigEndToEndId; // inputan dari request CT

                string jsonRequest = JsonConvert.SerializeObject(req), idr = req.OrigEndToEndId, num = req.TranRefNUM;
                //string jsonResponse = GenerateReq(req, "http://10.99.0.3:8355/jsonAPI/PaymentStatus");
                string jsonResponse = GenerateReq(req, "http://10.99.48.46:8355/jsonAPI/PaymentStatus");
                respAll = JsonConvert.DeserializeObject<RespAllPaymentStatus>(jsonResponse);

                if (respAll.MsgDefIdr == "pacs.002.001.10" && respAll.ReasonCode == "U000")
                {
                    res = JsonConvert.DeserializeObject<RespPaymentStatus>(jsonResponse);
                    st = "Success";
                    SaveLog(vmSt.CIF, vmSt.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(res.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = res;
                }
                else if (respAll.MsgDefIdr == "admi.002.001.01")
                {
                    err = JsonConvert.DeserializeObject<RespErrPaymentStatus>(jsonResponse);
                    st = "Error";
                    SaveLog(vmSt.CIF, vmSt.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(err.CreationDateTime, null, DateTimeStyles.RoundtripKind));
                    rrr = err;
                }
                else
                {
                    rej = JsonConvert.DeserializeObject<RespRejectPaymentStatus>(jsonResponse);
                    st = "Reject";
                    SaveLog(vmSt.CIF, vmSt.Channel, null, null, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rej.MsgCreationDate, null, DateTimeStyles.RoundtripKind));
                    rrr = rej;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Request" + ex.Message);
            }

            return rrr;
        }
        #endregion

        #endregion
    }
}