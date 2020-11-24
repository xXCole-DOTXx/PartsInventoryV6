namespace PartsInventoryV6
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory")]
    public partial class Inventory
    {
        [StringLength(255)]
        public string DESCRIPTION { get; set; }

        [StringLength(255)]
        public string OLD_NUMBER { get; set; }

        [StringLength(255)]
        public string NEW_NUMBER { get; set; }

        [StringLength(255)]
        public string UNIT_OF_ISSUE { get; set; }

        [StringLength(255)]
        public string SYS_CODE { get; set; }

        public int ID { get; set; }
    }
}
