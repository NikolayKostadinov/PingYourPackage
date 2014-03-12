namespace PingYourPackage.Domain.Entities.Extentions
{
    using System;
    using System.Linq;
    using PingYourPackage.Domain.Entities;
    using PingYourPackage.Domain.Entities.Core;
    
    public static class ShipmentStateRepositoryExtensions {

        public static IQueryable<ShipmentState> GetAllByShipmentKey(
            this IEntityRepository<ShipmentState> shipmentStateRepository, Guid shipmentKey) {

            return shipmentStateRepository.GetAll()
                .Where(x => x.ShipmentKey == shipmentKey);
        }
    }
}