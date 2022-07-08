using BIFastWebAPI.Models;
using BIFastWebAPI.Utility;
using Newtonsoft.Json;
using System;
using System.Web.Http;
using System.Globalization;
using BIFastWebAPI.Data;


namespace BIFastWebAPI.Controllers
{
    public class FinancialController : ApiController
    {
        Helper Hp = new Helper();
        string chan, st = "" ; DateTime cd; object a;
        ApplicationDbContext db = new ApplicationDbContext();

        #region AccountEnquiry
        [HttpPost]
        [Route("jsonAPI/AccountEnquiry")]
        public IHttpActionResult AccountEnquiry(ViewModelAccount vmAcc)
        {
            a = Hp.AccountEnquiry(vmAcc);
            return Ok(a);
        }
        #endregion

        #region CreditTransfer
        [HttpPost]
        [Route("jsonAPI/CreditTransfer")]
        public IHttpActionResult CreditTransfer(ViewModelTransaction VmTrx)
        {
            a = Hp.CreditTransferAll(VmTrx);
            return Ok(a);
        }
        #endregion

        #region CreditTransferToProxy
        [HttpPost]
        [Route("jsonAPI/CreditTransferToProxy")]
        public IHttpActionResult CreditTransferToProxy([FromBody] ViewModelProxy vmProx)
        {
            a = Hp.CreditToProxy(vmProx);
            return Ok(a);
        }
        #endregion

        #region ReversalCreditTransfer
        [HttpPost]
        [Route("jsonAPI/ReversalCreditTransfer")]
        public IHttpActionResult ReversalCreditTransfer([FromBody] ViewModelReversal vmRev)
        {
            a = Hp.Reversal(vmRev);
            return Ok(a);
        }
        #endregion

        #region PaymentStatus
        [HttpPost]
        [Route("jsonAPI/PaymentStatus")]
        public IHttpActionResult PaymentStatus(VmPayStat vmSt)
        {
            a = Hp.PaymentSt(vmSt);
            return Ok(a);            
        }
        #endregion
    }
}
