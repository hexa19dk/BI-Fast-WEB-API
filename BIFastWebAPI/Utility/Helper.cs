using BIFastWebAPI.Data;
using BIFastWebAPI.Data.Models;
using BIFastWebAPI.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Utility
{
    public class Helper
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        string st = ""; // status on response
        
        public bool Ck(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                return true;
            }
            return false;
        }
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

        //public int GenerateSn(string lastID)
        //{
        //    string ss = "";
        //    var ToInt = int.Parse(_db.ActivityLogs.Select(i => i.Id).Count().ToString()) + 1;
        //    lastID = _db.ActivityLogs.Select(x => x.Id).Any() ? ToInt.ToString() : null;

        //    if (String.IsNullOrEmpty(lastID))
        //    {
        //        var numInt = int.Parse(lastID) + 1;
        //        var LastNo = numInt.ToString();
        //        ss = LastNo.PadLeft(8, '0');
        //    }
        //    else
        //    {
        //        ss = lastID.PadLeft(8, '0');
        //    }
        //}

        public string swCh(string Ch) //Channel
        {
            switch (Ch)
            {
                case "01": //Internet Banking
                    Ch = "01";
                    break;
                case "02": //Mobile Banking
                    Ch = "02";
                    break;
                case "03": //Over the Counter
                    Ch = "03";
                    break;
                case "99": //Other
                    Ch = "90";
                    break;

                default:
                    break;
            }
            return Ch;
        }

        public string swOt(string Ot) //Operation Type
        {
            switch (Ot)
            {
                case "NEWR": //New Registration
                    Ot = "NEWR";
                    break;
                case "AMND": //Update/Modify
                    Ot = "AMND";
                    break;
                case "DEAC": //Deregistration
                    Ot = "DEAC";
                    break;
                case "SUSP": //Suspend
                    Ot = "SUSP";
                    break;
                case "ACTV": //Activate
                    Ot = "ACTV";
                    break;
                case "PORT": //Porting
                    Ot = "PORT";
                    break;
                default:
                    break;
            }
            return Ot;
        }

        public string swPt(string Pt) //Proxy Type
        {
            switch (Pt)
            {
                case "01": //mobile phone
                    Pt = "01";
                    break;
                case "02": //email
                    Pt = "02";
                    break;
                case "03": //IPT ID
                    Pt = "03";
                    break;
                default:
                    break;
            }
            return Pt;
        }

        public string swAt(string At) // Account Type
        {
            switch (At)
            {
                case "CACC": //Current account 
                    At = "CACC";
                    break;
                case "SVGS": //Savings account 
                    At = "SVGS";
                    break;
                case "LOAN": //Loan
                    At = "LOAN";
                    break;
                case "CCRD": //Credit card 
                    At = "CCRD";
                    break;
                case "UESB": //E-Money
                    At = "UESB";
                    break;
                case "OTHR": //None of the above
                    At = "OTHR";
                    break;
                default:
                    break;
            }
            return At;
        }

        public string swSIDType(string SIDType) // Secondary ID Type
        {
            switch (SIDType)
            {
                case "01": //National ID Number 
                    SIDType = "01";
                    break;
                case "02": //Passport number
                    SIDType = "02";
                    break;
                default:
                    break;
            }
            return SIDType;
        }

        public string swCt(string Ct) // Customer Type
        {
            switch (Ct)
            {
                case "01": //Individual
                    Ct = "01";
                    break;
                case "02": //Corporate
                    Ct = "02";
                    break;
                case "03": //Goverment
                    Ct = "03";
                    break;
                case "04": //Remittance
                    Ct = "04";
                    break;
                case "99": //Others
                    Ct = "99";
                    break;
                default:
                    break;
            }
            return Ct;
        }

        public string swCRs(string CRs) // Customer Resident Status
        {
            switch (CRs)
            {

                case "01": //Residental
                    CRs = "01";
                    break;
                case "02": //Non Residental
                    CRs = "02";
                    break;
                default:
                    break;
            }
            return CRs;
        }

        public string swOri(string Ori) // Customer Resident Status
        {
            switch (Ori)
            {

                case "O": //OFI,
                    Ori = "O";
                    break;
                case "H": //CI HUB
                    Ori = "H";
                    break;
                case "R": //RFI
                    Ori = "R";
                    break;
                default:
                    break;
            }
            return Ori;
        }

        public string PurposeType(string PsTp) //Purpose Type 
        {
            switch (PsTp)
            {
                case "Investment":
                    PsTp = "01";
                    break;
                case "Transfer of Wealth":
                    PsTp = "02";
                    break;
                case "Purchase":
                    PsTp = "03";
                    break;
                case "Others":
                    PsTp = "99";
                    break;
            }

            return PsTp;
        }

        public string CreditorAccountType(string Cat) //Creditor Account Type
        {
            switch (Cat)
            {
                case "Current Account":
                    Cat = "CACC";
                    break;
                case "Savings Account":
                    Cat = "SVGS";
                    break;
                case "Loan":
                    Cat = "LOAN";
                    break;
                case "Credit Card":
                    Cat = "CCRD";
                    break;
                case "E-Money":
                    Cat = "UESB";
                    break;
                case "None of the above":
                    Cat = "OTHR";
                    break;
            }

            return Cat;
        }

        public string CreditorType(string CrTp) //Creditor Type 
        {
            switch (CrTp)
            {
                case "Individual":
                    CrTp = "01";
                    break;
                case "Corporate":
                    CrTp = "02";
                    break;
                case "Government":
                    CrTp = "03";
                    break;
                case "Remittance":
                    CrTp = "04";
                    break;
                case "Others":
                    CrTp = "99";
                    break;
            }

            return CrTp;
        }

        public string DebitorAccountType(string Dat) //Debitor Account Type
        {
            switch (Dat)
            {
                case "Current Account":
                    Dat = "CACC"
                        ;
                    break;
                case "Savings Account":
                    Dat = "SVGS";
                    break;
                case "Loan":
                    Dat = "LOAN";
                    break;
                case "Credit Card":
                    Dat = "CCRD";
                    break;
                case "E-Money":
                    Dat = "UESB";
                    break;
                case "None of the above":
                    Dat = "OTHR";
                    break;
            }

            return Dat;
        }

        public string DebitorType(string Dt)
        {
            switch (Dt)
            {
                case "Individual":
                    Dt = "01";
                    break;
                case "Corporate":
                    Dt = "02";
                    break;
                case "Government":
                    Dt = "03";
                    break;
                case "Remittance":
                    Dt = "04";
                    break;
                case "Others":
                    Dt = "99";
                    break;
            }
            return Dt;
        }


        #region CreditTransfer Mapping Request
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
    }
}