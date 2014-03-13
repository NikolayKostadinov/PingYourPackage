namespace PingYourPackage.API.Test.Integration
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using Moq;
    using PingYourPackage.Domain.Services;

    internal static class ServicesMockHelper
    {

        internal static Mock<IMembershipService> GetInitialMembershipServiceMock()
        {

            var membershipServiceMock = new Mock<IMembershipService>();

            var users = new[] { 
                new { 
                    Name = Constants.ValidAdminUserName, 
                    Password = Constants.ValidAdminPassword, 
                    Roles = new[] { "Admin" } 
                },
                new { 
                    Name = Constants.ValidEmployeeUserName, 
                    Password = Constants.ValidEmployeePassword, 
                    Roles = new[] { "Employee" } 
                },
                new { 
                    Name = Constants.ValidAffiliateUserName, 
                    Password = Constants.ValidAffiliatePassword, 
                    Roles = new[] { "Affiliate" } 
                }
            }.ToDictionary(
                user => user.Name, user => user
            );

            membershipServiceMock.Setup(ms => ms.ValidateUser(
                It.IsAny<string>(), It.IsAny<string>())
            ).Returns<string, string>(
                (username, password) =>
                {

                    var user = users.FirstOrDefault(x => x.Key.Equals(
                        username, StringComparison.OrdinalIgnoreCase)).Value;

                    var validUserContext = (user != null)
                        ? new ValidUserContext
                        {
                            Principal = new GenericPrincipal(
                                new GenericIdentity(user.Name), user.Roles
                            )
                        } : new ValidUserContext();

                    return validUserContext;
                }
            );

            return membershipServiceMock;
        }
    }
}