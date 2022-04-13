using BIFastWebAPI.Models;
using System.Web.Services;

namespace BIFastWebAPI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    
    public class BFService : System.Web.Services.WebService
    {       

        [WebMethod(MessageName = "Transaction", Description = "Transaction ATM & HP")]
        public ViewModelTransaction Transaction(ViewModelTransaction data)
        {
            ViewModelTransaction vmTrx = new ViewModelTransaction();
            vmTrx = data;

            return vmTrx;
        }

    }
}
