namespace PingYourPackage.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using PingYourPackage.Domain.Entities.Core;

    public class UserInRole : IEntity
    {
        [Key]
        public Guid Key { get; set; }

        public Guid UserKey { get; set; }

        public Guid RoleKey { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
