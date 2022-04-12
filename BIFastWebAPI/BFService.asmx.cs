using BIFastWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            ViewModelTransaction vmTf = new ViewModelTransaction();
            vmTf = data;

            return vmTf;
        }

    }
}
