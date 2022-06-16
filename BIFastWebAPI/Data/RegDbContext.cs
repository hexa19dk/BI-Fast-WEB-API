using BIFastWebAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Data
{
    public class RegDbContext : DbContext
    {
        public RegDbContext() : base("DevSaveRegis")
        {
        }

        public DbSet<RegistrationData> RegistrationDatas { get; set; }


    }
}