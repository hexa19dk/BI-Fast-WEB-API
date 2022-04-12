using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BIFastWebAPI.Data;
using BIFastWebAPI.Models;
using BIFastWebAPI.Controllers;

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
            ReqCreditTransfer req = new ReqCreditTransfer();
            RespCreditTransfer resp = new RespCreditTransfer();
            RejectCreditTransfer rejCt = new RejectCreditTransfer();
            ErrorCreditTransfer errCt = new ErrorCreditTransfer();
            RespCreditTrfAll respall = new RespCreditTrfAll();
            

            //respall = CreditTransferAll(VmTrx);


            return "testing";
        }

    }
}
