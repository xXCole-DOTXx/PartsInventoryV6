using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using CovidV3.Models;

namespace PartsInventoryV6
{
    public partial class EquipmentContext : DbContext
    {
        public CovidAppContext()
            : base("name=CovidAppContext")
        {
        }

        public DbSet<Case_Log> Case_Log { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
