namespace PingYourPackage.Domain.Entities.Extentions
{
    using PingYourPackage.Domain.Entities.Core;
    using System.Linq;

    public static class RoleRepositoryExtensions
    {
        public static Role GetSingleByRoleName(
        this IEntityRepository<Role> roleRepository, string roleName)
        {
            return roleRepository.GetAll().FirstOrDefault(x => x.Name == roleName);
        }
    }
}
