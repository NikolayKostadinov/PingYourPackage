namespace PingYourPackage.API.Model.Dtos {
    
    using System;
    using WebApiDoodle.Net.Http.Client.Model;
    
    public class ShipmentTypeDto : IDto {

        public Guid Key { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}