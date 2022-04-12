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

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(MessageName = "Transaction", Description = "Transaction ATM & HP")]
        public string Transaction(string input)
        {
            string rekSbr, BankDest, NoTuj, NoRekTuj, NomTf, Desk;
            string result = "";



            return "testing";
        }

    }
}
