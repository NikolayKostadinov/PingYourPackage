namespace PingYourPackage.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using PingYourPackage.Domain.Entities.Core;

    public class ShipmentType : IEntity
    {
        [Key]
        public Guid Key { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Shipment> Shipments { get; set; }

        public ShipmentType()
        {
            Shipments = new HashSet<Shipment>();
        }
    }
}
