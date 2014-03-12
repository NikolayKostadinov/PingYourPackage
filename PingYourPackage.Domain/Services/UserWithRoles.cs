namespace PingYourPackage.Domain.Services
{
    using PingYourPackage.Domain.Entities;
    using System.Collections.Generic;

    public class UserWithRoles
    {
        public User User { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}