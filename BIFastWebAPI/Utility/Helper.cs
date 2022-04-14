using BIFastWebAPI.Data;
using BIFastWebAPI.Data.Models;
using BIFastWebAPI.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Utility
{
    public class Helper
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        string st = "", ss = "", chan, Date = DateTime.Now.ToString("yyyyMMdd");

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

        #region Savelog
        public void SaveLog(string chan, string num, string idr, object jsonRequest, object jsonResponse, string st, DateTime reqModel, DateTime respModel)
        {
            var log = new ActivityLog();
            log.Channel = chan;
            log.OrigTranRefNUM = num;
            log.BizMsgIdr = idr;
            log.EndPoint = HttpContext.Current.Request.Url.AbsolutePath.ToString();
            log.Type = "POST";
            log.UserId = "Ngadmin";
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
        #endregion

        #region Non Transaction Function Helper

        #region Alias Management
        public AliasManagementResponses AliasManagement(AliasManagementVM data)
        {
            AliasManagementResponses respAll = new AliasManagementResponses();
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


                string jsonRequest = JsonConvert.SerializeObject(reqAM), jsonResponse, num = reqAM.TranRefNUM, idr = reqAM.BizMsgIdr;
                jsonResponse = GenerateReq(reqAM, "http://10.99.0.72:8355/jsonAPI/prxy001");


                if (Ck(reqAM.SendingSystemBIC) && Ck(reqAM.ReceivingSystemBIC) && Ck(reqAM.BizMsgIdr) && Ck(reqAM.CreationDateTime) && Ck(reqAM.TranRefNUM) && Ck(reqAM.MsgCreationDate) && Ck(reqAM.SendingParticipantID) && Ck(reqAM.MsgSenderAccountId) && Ck(reqAM.OperationType) && Ck(reqAM.ProxyType) && Ck(reqAM.ProxyValue) && Ck(reqAM.ProxyBankID) && Ck(reqAM.AccountID) && Ck(reqAM.AccountType) && Ck(reqAM.SecondaryIDType) && Ck(reqAM.SecondaryIDValue) && Ck(reqAM.SendingSystemBIC) && jsonResponse.Contains("prxy.002.001.01"))
                {

                    //success
                    respAM = JsonConvert.DeserializeObject<RespAliasManagement>(jsonResponse);
                    st = "Success";
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    //AutoMapper.Mapper.Map<RespAliasManagement, AliasManagementResponses>(respAM);
                    respAll.SendingSystemBIC = respAM.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = respAM.ReceivingSystemBIC;
                    respAll.BizMsgIdr = respAM.BizMsgIdr;
                    respAll.MsgDefIdr = respAM.MsgDefIdr;
                    respAll.CreationDateTime = respAM.CreationDateTime;
                    respAll.TranRefNUM = respAM.TranRefNUM;
                    respAll.MsgCreationDate = respAM.MsgCreationDate;
                    respAll.SendingParticipantID = respAM.SendingParticipantID;
                    respAll.OrigUniqueRequestID = respAM.OrigUniqueRequestID;
                    respAll.OriginalMsgdefIdr = respAM.OriginalMsgdefIdr;
                    respAll.OrigMsgCreationDate = respAM.OrigMsgCreationDate;
                    respAll.ProxyInqRespStatusCode = respAM.ProxyInqRespStatusCode;
                    respAll.StatusReasonCode = respAM.StatusReasonCode;
                    respAll.OperationType = respAM.OperationType;
                    respAll.RegistrationID = respAM.RegistrationID;
                    respAll.ProxyBankID = respAM.ProxyBankID;
                    respAll.CustomerType = respAM.CustomerType;
                    respAll.CustomerID = respAM.CustomerID;
                    respAll.CustomerResidentStatus = respAM.CustomerResidentStatus;
                    respAll.CustomerTownName = respAM.CustomerTownName;

                }
                else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
                {
                    //error
                    st = "Error";
                    errAM = JsonConvert.DeserializeObject<RespErrAliasManagement>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    //AutoMapper.Mapper.Map<AliasManagementResponses, RespErrAliasManagement>(respAll);
                    respAll.SendingSystemBIC = errAM.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = errAM.ReceivingSystemBIC;
                    respAll.BizMsgIdr = errAM.BizMsgIdr;
                    respAll.MsgDefIdr = errAM.MsgDefIdr;
                    respAll.CreationDateTime = errAM.CreationDateTime;
                    respAll.Reference = errAM.Reference;
                    respAll.RejectReason = errAM.RejectReason;
                    respAll.RejectDateTime = errAM.RejectDateTime;
                    respAll.ErrorLocation = errAM.ErrorLocation;
                    respAll.ReasonDesc = errAM.ReasonDesc;

                }
                else
                {
                    //reject
                    st = "Reject";
                    rejAM = JsonConvert.DeserializeObject<RespRejectAliasManagement>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejAM.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    //AutoMapper.Mapper.Map<AliasManagementResponses, RespRejectAliasManagement>(respAll);
                    respAll.SendingSystemBIC = rejAM.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = rejAM.ReceivingSystemBIC;
                    respAll.BizMsgIdr = rejAM.BizMsgIdr;
                    respAll.MsgDefIdr = rejAM.MsgDefIdr;
                    respAll.CreationDateTime = rejAM.CreationDateTime;
                    respAll.Reference = rejAM.Reference;
                    respAll.RejectReason = rejAM.RejectReason;
                    respAll.RejectDateTime = rejAM.RejectDateTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }
            return respAll;
        }
        #endregion

        #region Alias Resolution
        
        public AliasResolutionResponses AliasResolution(AliasResolutionVM data)
        {
            AliasResolutionResponses respAll = new AliasResolutionResponses();
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

                string jsonRequest = JsonConvert.SerializeObject(reqAR), jsonResponse, num = reqAR.TranRefNUM, idr = reqAR.BizMsgIdr;
                jsonResponse = GenerateReq(reqAR, "http://10.99.0.72:8355/jsonAPI/prxy003");


                if (Ck(reqAR.SendingSystemBIC) && Ck(reqAR.ReceivingSystemBIC) && Ck(reqAR.BizMsgIdr) && Ck(reqAR.MsgDefIdr) && Ck(reqAR.CreationDateTime) && Ck(reqAR.TranRefNUM) && Ck(reqAR.MsgCreationDate) && Ck(reqAR.SendingParticipantID) && Ck(reqAR.MsgSenderAccountId) && Ck(reqAR.AlisaResolutionLookup) && Ck(reqAR.UniqueRequestID) && Ck(reqAR.ProxyType) && Ck(reqAR.ProxyValue) && jsonResponse.Contains("prxy.004.001.01"))
                {
                    //success
                    st = "Success";
                    respAR = JsonConvert.DeserializeObject<RespAliasResolution>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    respAll.SendingSystemBIC = respAR.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = respAR.ReceivingSystemBIC;
                    respAll.BizMsgIdr = respAR.BizMsgIdr;
                    respAll.MsgDefIdr = respAR.MsgDefIdr;
                    respAll.CreationDateTime = respAR.CreationDateTime;
                    respAll.TranRefNUM = respAR.TranRefNUM;
                    respAll.MsgCreationDate = respAR.MsgCreationDate;
                    respAll.RecipientParticipantID = respAR.RecipientParticipantID;
                    respAll.UniqueRequestID = respAR.UniqueRequestID;
                    respAll.OrigUniqueRequestID = respAR.OrigUniqueRequestID;
                    respAll.OriginalMsgdefIdr = respAR.OriginalMsgdefIdr;
                    respAll.OrigMsgCreationDate = respAR.OrigMsgCreationDate;
                    respAll.OriginalProxyType = respAR.OriginalProxyType;
                    respAll.OriginalProxyValue = respAR.OriginalProxyValue;
                    respAll.ProxyInqRespStatusCode = respAR.ProxyInqRespStatusCode;
                    respAll.StatusReasonCode = respAR.StatusReasonCode;
                    respAll.ProxyType = respAR.ProxyType;
                    respAll.ProxyValue = respAR.ProxyValue;
                    respAll.RegistrationID = respAR.RegistrationID;
                    respAll.DisplayName = respAR.DisplayName;
                    respAll.ProxyBankID = respAR.ProxyBankID;
                    respAll.AccountID = respAR.AccountID;
                    respAll.AccountType = respAR.AccountType;
                    respAll.AccountName = respAR.AccountName;
                    respAll.CustomerType = respAR.CustomerType;
                    respAll.CustomerID = respAR.CustomerID;
                    respAll.CustomerResidentStatus = respAR.CustomerResidentStatus;
                    respAll.CustomerTownName = respAR.CustomerTownName;

                }
                else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
                {
                    //error
                    st = "Error";
                    errAR = JsonConvert.DeserializeObject<RespErrAliasResolution>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));

                    respAll.SendingSystemBIC = errAR.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = errAR.ReceivingSystemBIC;
                    respAll.BizMsgIdr = errAR.BizMsgIdr;
                    respAll.MsgDefIdr = errAR.MsgDefIdr;
                    respAll.CreationDateTime = errAR.CreationDateTime;
                    respAll.Reference = errAR.Reference;
                    respAll.RejectReason = errAR.RejectReason;
                    respAll.RejectDateTime = errAR.RejectDateTime;
                    respAll.ErrorLocation = errAR.ErrorLocation;
                    respAll.ReasonDesc = errAR.ReasonDesc;

                }
                else
                {
                    //reject
                    st = "Reject";
                    rejAR = JsonConvert.DeserializeObject<RespRejectAliasResolution>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejAR.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));

                    respAll.SendingSystemBIC = rejAR.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = rejAR.ReceivingSystemBIC;
                    respAll.BizMsgIdr = rejAR.BizMsgIdr;
                    respAll.MsgDefIdr = rejAR.MsgDefIdr;
                    respAll.CreationDateTime = rejAR.CreationDateTime;
                    respAll.Reference = rejAR.Reference;
                    respAll.RejectReason = rejAR.RejectReason;
                    respAll.RejectDateTime = rejAR.RejectDateTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }
            return respAll;
        }

        #endregion

        #region Alias Registration inquiry
        public AliasRegInquiryResponses AliasRegInquiry(AliasRegInquiryVM data)
        {
            AliasRegInquiryResponses respAll = new AliasRegInquiryResponses();
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

                string jsonRequest = JsonConvert.SerializeObject(reqARI), jsonResponse, num = reqARI.TranRefNUM, idr = reqARI.BizMsgIdr; ;
                jsonResponse = GenerateReq(reqARI, "http://10.99.0.72:8355/jsonAPI/prxy005");
                //jsonResponse = GenerateReq(reqARI, "http://172.18.99.30:4343/jsonAPI/prxy005");



                if (Ck(reqARI.SendingSystemBIC) && Ck(reqARI.ReceivingSystemBIC) && Ck(reqARI.BizMsgIdr) && Ck(reqARI.MsgDefIdr) && Ck(reqARI.CreationDateTime) && Ck(reqARI.TranRefNUM) && Ck(reqARI.MsgCreationDate) && Ck(reqARI.SendingParticipantID) && Ck(reqARI.MsgSenderAccountId) && jsonResponse.Contains("prxy.006.001.01"))
                {
                    //success
                    st = "Success";
                    respARI = JsonConvert.DeserializeObject<RespAliasRegInquiry>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(respARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));

                    respAll.SendingSystemBIC = respARI.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = respARI.ReceivingSystemBIC;
                    respAll.BizMsgIdr = respARI.BizMsgIdr;
                    respAll.MsgDefIdr = respARI.MsgDefIdr;
                    respAll.CreationDateTime = respARI.CreationDateTime;
                    respAll.TranRefNUM = respARI.TranRefNUM;
                    respAll.MsgCreationDate = respARI.MsgCreationDate;
                    respAll.RecipentParticipantID = respARI.RecipentParticipantID;
                    respAll.OrigUniqueRequestID = respARI.OrigUniqueRequestID;
                    respAll.OriginalMsgdefIdr = respARI.OriginalMsgdefIdr;
                    respAll.OrigMsgCreationDate = respARI.OrigMsgCreationDate;
                    respAll.ProxyInqRespStatusCode = respARI.ProxyInqRespStatusCode;
                    respAll.StatusReasonCode = respARI.StatusReasonCode;
                    respAll.RspnData = respARI.RspnData;
                    respAll.CustomerType = respARI.CustomerType;
                    respAll.CustomerID = respARI.CustomerID;
                    respAll.CustomerResidentStatus = respARI.CustomerResidentStatus;
                    respAll.CustomerTownName = respARI.CustomerTownName;

                }
                else if (jsonResponse.Contains("ErrorLocation") && jsonResponse.Contains("admi.002.001.01"))
                {
                    //error
                    st = "Error";
                    errARI = JsonConvert.DeserializeObject<RespErrAliasRegInquiry>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(errARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));
                    
                    respAll.SendingSystemBIC = errARI.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = errARI.ReceivingSystemBIC;
                    respAll.BizMsgIdr = errARI.BizMsgIdr;
                    respAll.MsgDefIdr = errARI.MsgDefIdr;
                    respAll.CreationDateTime = errARI.CreationDateTime;
                    respAll.Reference = errARI.Reference;
                    respAll.RejectReason = errARI.RejectReason;
                    respAll.RejectDateTime = errARI.RejectDateTime;
                    respAll.ErrorLocation = errARI.ErrorLocation;
                    respAll.ReasonDesc = errARI.ReasonDesc;
                }
                else
                {
                    //reject
                    st = "Reject";
                    rejARI = JsonConvert.DeserializeObject<RespRejectAliasRegInquiry>(jsonResponse);
                    SaveLog(chan, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(reqARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(rejARI.CreationDateTime, null, System.Globalization.DateTimeStyles.RoundtripKind));

                    respAll.SendingSystemBIC = rejARI.SendingSystemBIC;
                    respAll.ReceivingSystemBIC = rejARI.ReceivingSystemBIC;
                    respAll.BizMsgIdr = rejARI.BizMsgIdr;
                    respAll.MsgDefIdr = rejARI.MsgDefIdr;
                    respAll.CreationDateTime = rejARI.CreationDateTime;
                    respAll.Reference = rejARI.Reference;
                    respAll.RejectReason = rejARI.RejectReason;
                    respAll.RejectDateTime = rejARI.RejectDateTime;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred! Please try again." + ex.Message);
            }
            return respAll;
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
        //code
        #endregion

        #region Credit Transfer
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
                string jsonResponse = GenerateReq(req, "http://10.99.0.72:8355/jsonAPI/CreditTransfer");

                if (Ck(req.EndToEndId) && Ck(req.MsgDefIdr) && Ck(req.TranRefNUM) && Ck(req.RecipentParticipantID) && Ck(req.CreditorAccountNo) && Ck(req.Amount) && Ck(req.Currency) && Ck(req.MsgCreationDate) && Ck(req.PurposeType) && Ck(req.SendingParticipantID) && Ck(req.DebitorAccountNo) && Ck(req.DebitorAccountType) && Ck(req.DebitorAccountName) && Ck(req.DebitorID) && Ck(req.RecipentParticipantID) && Ck(req.CreditorAccountNo) && Ck(req.CreditorAccountName) && jsonResponse.Contains("pacs.002.001.10"))
                {
                    resp = JsonConvert.DeserializeObject<RespCreditTransfer>(jsonResponse);
                    st = "Success";

                    SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(respall.MsgCreationDate, null, DateTimeStyles.RoundtripKind));

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

                    SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(errCt.CreationDateTime, null, DateTimeStyles.RoundtripKind));

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

                    SaveLog(ct, num, idr, jsonRequest, jsonResponse, st, DateTime.Parse(req.MsgCreationDate, null, DateTimeStyles.RoundtripKind), DateTime.Parse(rejCt.MsgCreationDate, null, DateTimeStyles.RoundtripKind));

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
                Console.WriteLine("Bad Request" + ex.Message);
            }

            return respall;
        }
        #endregion

        #endregion

        #region Credit Transfer To Proxy
        //code
        #endregion

        #region Reversal Credit Transfer
        //code
        #endregion

        #region Payment Status
        //code
        #endregion

#endregion

    }
}