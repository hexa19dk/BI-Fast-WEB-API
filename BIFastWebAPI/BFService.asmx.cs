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
        Helper hp = new Helper();

        [WebMethod(MessageName = "CreditTransfer", Description = "for CreditTransfer and CreditTransferToProxy")]
        public RespCreditTrfAll Transaction(ViewModelTransaction vmTrx)
        {
            RespCreditTrfAll resp = new RespCreditTrfAll();
            resp = hp.CreditTransferAll(vmTrx);
            return resp;
        }

    }
}
