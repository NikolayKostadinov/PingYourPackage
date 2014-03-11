namespace PingYourPackage.Domain.Entities
{
    using System;
    using System.Linq;
  
    public enum ShipmentStatus
    {
        Ordered = 1,
        Scheduled = 2,
        InTransit = 3,
        Delivered = 4
    }
}
