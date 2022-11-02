using BIFastWebAPI.Models;
using System.Web.Services;
using BIFastWebAPI.Utility;
using System.Collections.Generic;
using BIFastWebAPI.Data;
using System.Linq;
using BIFastWebAPI.Data.Models;
using System.Xml.Serialization;

namespace BIFastWebAPI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class BFService : System.Web.Services.WebService
    {
        Helper hp = new Helper();
        ApplicationDbContext _db = new ApplicationDbContext();

        #region TransactionService
        [WebMethod(MessageName = "AccountInquiry", Description = "Account Inquiry")]
        [XmlInclude(typeof(RespAccEnquiry))]
        [XmlInclude(typeof(RespRejectAccEnquiry))]
        [XmlInclude(typeof(RespErrAccEnquiry))]
        [XmlInclude(typeof(RespAllAccount))]
        public object Inquiry(ViewModelAccount vmAcc)
        {
            return hp.AccountEnquiry(vmAcc);
        }


        [WebMethod(MessageName = "CreditTransfer", Description = "for CreditTransfer")]
        [XmlInclude(typeof(RespCreditTransfer))]
        [XmlInclude(typeof(RejectCreditTransfer))]
        [XmlInclude(typeof(ErrorCreditTransfer))]
        [XmlInclude(typeof(RespCreditTrfAll))]
        public object Transaction(ViewModelTransaction vmTrx)
        {
           return hp.CreditTransferAll(vmTrx);
        }


        [WebMethod(MessageName = "CreditProxy", Description = "Credit Transfer To Proxy")]
        [XmlInclude(typeof(RespCrediTransferToProxy))]
        [XmlInclude(typeof(RespRejectCreditTransferToProxy))]
        [XmlInclude(typeof(RespErrCreditTransferToProxy))]
        [XmlInclude(typeof(RespAllCreditProxy))]
        public object TransactionProxy(ViewModelProxy vmProx)
        {
            return hp.CreditToProxy(vmProx);
        }

        #endregion

        #region NonTransactionService

        [WebMethod(MessageName ="Alias_Management" , Description ="Alias Management")]
        [XmlInclude(typeof(RespAliasManagement))]
        [XmlInclude(typeof(RespRejectAliasManagement))]
        [XmlInclude(typeof(RespErrAliasManagement))]
        public object AliasManagement(AliasManagementVM data)
        {
            return hp.AliasManagement(data);
        }


        [WebMethod(MessageName = "Alias_Resolution", Description = "Alias Resolution")]
        [XmlInclude(typeof(RespAliasResolution))]
        [XmlInclude(typeof(RespErrAliasResolution))]
        [XmlInclude(typeof(RespRejectAliasResolution))]
        public object AliasResolution(AliasResolutionVM data)
        {
            return hp.AliasResolution(data);
        }

        [WebMethod(MessageName = "Alias_Registration_inquiry", Description = "Alias Registration Inquiry")]
        [XmlInclude(typeof(RespAliasRegInquiry))]
        [XmlInclude(typeof(RespRejectAliasRegInquiry))]
        [XmlInclude(typeof(RespErrAliasRegInquiry))]
        public object AliasRegInquiry(AliasRegInquiryVM data)
        {
            return hp.AliasRegInquiry(data);
        }


        //[WebMethod(MessageName = "Get_RegID", Description = "Get Registration ID")]
        //public string RegistrationData GetRegID(string pv, string CIF, string KTP, string Norek)
        //{
        //    RegistrationData data = hp.GetRegID(pv, CIF, KTP, Norek);
        //    return data;
        //}
        #endregion

    }
}
