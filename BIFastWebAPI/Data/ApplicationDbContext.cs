using BIFastWebAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BIFastWebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {

        }

        public DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}