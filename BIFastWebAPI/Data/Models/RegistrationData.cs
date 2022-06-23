using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Data.Models
{
    public class RegistrationData
    {
        [Key]
        public string RegistrationID { get; set; }
        public string CIF { get; set; }
        public string KTP { get; set; }
        public string ProxyValue { get; set; }
        public string ProxyType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}