namespace PingYourPackage.Domain.Entities
{
    using System;
    using System.Linq;
    using PingYourPackage.Domain.Entities.Core;

    public static class ShipmentTypeRepositoryExtensions
    {
        public static ShipmentType GetSingleByName(
            this IEntityRepository<ShipmentType> shipmentTypeRepository,
            string name)
        {

            return shipmentTypeRepository
                .FindBy(x => x.Name == name).FirstOrDefault();
        }
    }
}
