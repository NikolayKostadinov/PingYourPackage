namespace PingYourPackage.Domain.Entities {

    using System;
    using System.Linq;
    using PingYourPackage.Domain.Entities.Core;

    public static class UserInRoleRepositoryExtensions {

        public static bool IsUserInRole(
            this IEntityRepository<UserInRole> userInRoleRepository, 
            Guid userKey, 
            Guid roleKey) {

            var userInRole = userInRoleRepository.GetAll()
                .FirstOrDefault(
                    x => x.UserKey == userKey && x.RoleKey == roleKey);

            return (userInRole != null);
        }
    }
}
