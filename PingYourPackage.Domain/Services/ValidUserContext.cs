namespace PingYourPackage.Domain.Services
{
    using System;
    using System.Linq;
    using System.Security.Principal;

    public class ValidUserContext
    {

        public IPrincipal Principal { get; set; }
        public UserWithRoles User { get; set; }

        public bool IsValid()
        {

            return Principal != null;
        }
    }
}