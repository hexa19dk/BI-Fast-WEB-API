using BIFastWebAPI.Models;
using System.Web.Services;
using BIFastWebAPI.Controllers;
using BIFastWebAPI.Utility;

namespace BIFastWebAPI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class BFService : System.Web.Services.WebService
    {
        FunctionUtility ut = new FunctionUtility();

        [WebMethod(MessageName = "Transaction", Description = "Transaction ATM & HP")]
        public RespCreditTrfAll Transaction(ViewModelTransaction vmTrx)
        {
            RespCreditTrfAll resp = new RespCreditTrfAll();

            resp = ut.CreditTransferAll(vmTrx);

            return resp;
        }

    }
}
