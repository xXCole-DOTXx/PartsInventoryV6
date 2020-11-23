using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace PartsInventoryV6
{
    public partial class EquipmentContext : DbContext
    {
        public EquipmentContext()
            : base("name=EquipmentContext")
        {
        }

        public virtual DbSet<Inventory> Inventories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
