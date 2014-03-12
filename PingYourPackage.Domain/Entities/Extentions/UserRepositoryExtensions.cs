namespace PingYourPackage.Domain.Entities.Extentions
{
    using System;
    using System.Linq;
    using PingYourPackage.Domain.Entities.Core;

    public static class UserRepositoryExtensions
    {
        public static User GetSingleByUsername(
        this IEntityRepository<User> userRepository, string username)
        {
            return userRepository.GetAll().FirstOrDefault(x => x.Name == username);
        }
    }
}
