using BIFastWebAPI.Models;
using System.Web.Services;
using BIFastWebAPI.Utility;
using System.Collections.Generic;
using BIFastWebAPI.Data;
using System.Linq;
using System.Collections;

namespace BIFastWebAPI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class BFService : System.Web.Services.WebService
    {
        Helper hp = new Helper();

        [WebMethod(MessageName = "GetBankMaster", Description = "Get Master Data")]
        public List<BankMaster> GetAllBank()
        {
            ApplicationDbContext _db = new ApplicationDbContext();

            IEnumerable data = _db.BankMasters.Select(b => b.KodeBank.ToString()).ToList();

            return (List<BankMaster>)data;
        }


        #region TransactionService
        [WebMethod(MessageName = "CreditTransfer", Description = "for CreditTransfer")]
        public RespCreditTrfAll Transaction(ViewModelTransaction vmTrx)
        {
            RespCreditTrfAll resp = new RespCreditTrfAll();
            resp = hp.CreditTransferAll(vmTrx);
            return resp;
        }

        [WebMethod(MessageName = "CreditProxy", Description = "Credit Transfer To Proxy")]
        public RespAllCreditProxy TransactionProxy(ViewModelProxy vmProx)
        {
            RespAllCreditProxy resp = new RespAllCreditProxy();
            resp = hp.CreditToProxy(vmProx);
            return resp;
        }

        [WebMethod(MessageName = "AccountInquiry", Description = "Account Inquiry")]
        public RespAllAccount Inquiry(ViewModelAccount vmAcc)
        {
            RespAllAccount resp = new RespAllAccount();
            resp = hp.AccountEnquiry(vmAcc);
            return resp;
        }
        #endregion

        #region NonTransactionService

        [WebMethod(MessageName ="Alias_Management" , Description ="Alias Management")]
        public AliasManagementResponses AliasManagement(AliasManagementVM data)
        {
            AliasManagementResponses respAll = new AliasManagementResponses();
            respAll = hp.AliasManagement(data);
            return respAll;
        }

        [WebMethod(MessageName = "Alias_Resolution", Description = "Alias Resolution")]
        public AliasResolutionResponses AliasResolution(AliasResolutionVM data)
        {
            AliasResolutionResponses respAll = new AliasResolutionResponses();
            respAll = hp.AliasResolution(data);
            return respAll;
        }

        [WebMethod(MessageName = "Alias_Registration_inquiry", Description = "Alias Registration Inquiry")]
        public AliasRegInquiryResponses AliasRegInquiry(AliasRegInquiryVM data)
        {
            AliasRegInquiryResponses respAll = new AliasRegInquiryResponses();
            respAll = hp.AliasRegInquiry(data);
            return respAll;
        }

        #endregion
    }
}
