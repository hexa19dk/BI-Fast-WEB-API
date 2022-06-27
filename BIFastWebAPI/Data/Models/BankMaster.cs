using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Data.Models
{
    
    public class BankMaster
    {
        public int Id { get; set; }
        [StringLength(3)]
        public string BankCode { get; set; }
        [StringLength(8)]
        [Index(nameof(SwiftCode), IsUnique = true)]
        public string SwiftCode { get; set; }
        public string NamaBank { get; set; }
    }
}