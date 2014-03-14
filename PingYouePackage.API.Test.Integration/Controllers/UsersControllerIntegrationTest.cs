﻿using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using PingYourPackage.API.Model.Dtos;
using PingYourPackage.API.Model.RequestModels;
using PingYourPackage.Domain.Entities;
using PingYourPackage.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace PingYourPackage.API.Test.Integration.Controllers
{

    public class UsersControllerIntegrationTest
    {

        public class GetUsers
        {

            [Fact, NullCurrentPrincipal]
            public Task
                Returns_200_And_Expected_Users_If_Request_Authorized()
            {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            "api/users", 1, 2),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                return IntegrationTestHelper
                    .TestForPaginatedDtoAsync<UserDto>(
                        config,
                        request,
                        expectedPageIndex: 1,
                        expectedTotalPageCount: 2,
                        expectedCurrentItemsCount: 2,
                        expectedTotalItemsCount: 3,
                        expectedHasNextPageResult: true,
                        expectedHasPreviousPageResult: false);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_PageIndex_Parameter_Is_Not_Correct()
            {

                // Arrange
                int pageIndexParam = 0,
                    pageSizeParam = 20;

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            "api/users",
                            pageIndexParam,
                            pageSizeParam),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var pageParamError = modelState["Page"] as string[];

                // Assert
                Assert.NotNull(pageParamError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_PageSize_Parameter_Is_Not_Correct()
            {

                // Arrange
                var pageIndexParam = 1;
                var pageSizeParam = 51;

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}?page={1}&take={2}",
                            "api/users",
                            pageIndexParam,
                            pageSizeParam),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var takeParamError = modelState["Take"] as string[];

                // Assert
                Assert.NotNull(takeParamError);
            }

            private static IMembershipService GetMembershipService()
            {

                var users = GetDummyUsers(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.GetUsers(
                        It.IsAny<int>(), It.IsAny<int>()
                    )
                ).Returns<int, int>((page, take) =>
                    users.AsQueryable()
                              .ToPaginatedList(page, take)
                );

                return mockMemSrv.Object;
            }
        }

        public class GetUser
        {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_Expected_User_If_Request_Authorized_And_User_Exists()
            {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/users", keys[1]),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var userDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<UserDto>(
                        config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(keys[1], userDto.Key);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_User_Does_Not_Exist()
            {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Get,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/users", Guid.NewGuid()),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            private static IMembershipService GetMembershipService(Guid[] keys)
            {

                var users = GetDummyUsers(keys);
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.GetUser(
                        It.Is<Guid>(
                            key => keys.Contains(key)
                        )
                    )
                ).Returns<Guid>(key =>
                    users.FirstOrDefault(x =>
                        x.User.Key == key
                    )
                );

                return mockMemSrv.Object;
            }
        }

        public class PostUser
        {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_201_And_User_If_Request_Authorized_And_Success()
            {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                // This is a valid user request to create new one
                var userRequestModel = new UserRequestModel
                {
                    Name = "FooBar",
                    Email = "FooBar@example.com",
                    Password = "123456789",
                    Roles = new[] { "Admin", "Employee" }
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/users"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<UserRequestModel>(
                    userRequestModel, new JsonMediaTypeFormatter());

                var userDto = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<UserDto>(
                        config, request, HttpStatusCode.Created);

                // Assert
                Assert.Equal(userRequestModel.Name, userDto.Name);
                Assert.Equal(userRequestModel.Email, userDto.Email);
                Assert.True(userDto.Roles.Any(x => x.Name.Equals(userRequestModel.Roles[0], StringComparison.OrdinalIgnoreCase)));
                Assert.True(userDto.Roles.Any(x => x.Name.Equals(userRequestModel.Roles[1], StringComparison.OrdinalIgnoreCase)));
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_409_If_Request_Authorized_But_Conflicted()
            {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                // This is not a valid user request to create new one
                var userRequestModel = new UserRequestModel
                {
                    Name = Constants.ValidAdminUserName,
                    Email = "FooBar@example.com",
                    Password = "123456789",
                    Roles = new[] { "Admin", "Employee" }
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/users"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<UserRequestModel>(
                    userRequestModel, new JsonMediaTypeFormatter());

                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid()
            {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                // This is not a valid user request to create new one
                var userRequestModel = new UserRequestModel
                {
                    Name = Constants.ValidAdminUserName,
                    Email = "FooBarexample.com",
                    Password = "1234",
                    Roles = new string[0]
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/users"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<UserRequestModel>(
                    userRequestModel, new JsonMediaTypeFormatter());

                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var passwordError = modelState["requestModel.Password"] as string[];
                var rolesError = modelState["requestModel.Roles"] as string[];
                var emailError = modelState["requestModel.Email"] as string[];

                // Assert
                Assert.NotNull(passwordError);
                Assert.NotNull(rolesError);
                Assert.NotNull(emailError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty()
            {

                // Arrange
                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService()));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Post,
                        uri: string.Format(
                            "https://localhost/{0}",
                            "api/users"),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var requestModelError = modelState["requestModel"] as string[];

                // Assert
                Assert.NotNull(requestModelError);
            }

            private static IMembershipService GetMembershipService()
            {

                CryptoService cryptoService = new CryptoService();
                var salt = cryptoService.GenerateSalt();

                var users = GetDummyUsers(new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                });
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.CreateUser(
                        It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string[]>()
                    )
                ).Returns<string, string, string, string[]>(
                    (username, email, password, roles) =>

                    new OperationResult<UserWithRoles>(true)
                    {
                        Entity = new UserWithRoles
                        {
                            User = new User
                            {
                                Key = Guid.NewGuid(),
                                Name = username,
                                Email = email,
                                Salt = salt,
                                HashedPassword = cryptoService.EncryptPassword(password, salt),
                                CreatedOn = DateTime.Now,
                                IsLocked = false
                            },
                            Roles = roles.Select(
                                roleName => new Role
                                {
                                    Key = Guid.NewGuid(),
                                    Name = roleName
                                }
                            )
                        }
                    }
                );

                mockMemSrv.Setup(ms => ms.CreateUser(
                        It.Is<string>(
                            userName =>
                                users.Any(x =>
                                    x.User.Name.Equals(
                                        userName, StringComparison.OrdinalIgnoreCase
                                    )
                                )
                        ),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string[]>()
                    )
                ).Returns(new OperationResult<UserWithRoles>(false));

                return mockMemSrv.Object;
            }
        }

        public class PutUser
        {

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_404_If_Request_Authorized_But_User_Does_Not_Exist()
            {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var invalidUserKey = Guid.NewGuid();

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                // This is a valid user request to update one
                var userRequestModel = new UserUpdateRequestModel
                {
                    Name = "FooBar",
                    Email = "FooBar@example.com",
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/users",
                            invalidUserKey),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<UserUpdateRequestModel>(
                    userRequestModel, new JsonMediaTypeFormatter());

                var response = await IntegrationTestHelper
                    .GetResponseAsync(config, request);

                // Assert
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Invalid()
            {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                // This is a not valid user request to update one
                var userRequestModel = new UserUpdateRequestModel
                {
                    Name = "ANameWhichIsMoreThan50CharsANameWhichIsMoreThan50Chars",
                    Email = "FooBarexample.com",
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/users",
                            keys[1]),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<UserUpdateRequestModel>(
                    userRequestModel, new JsonMediaTypeFormatter());

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var nameError = modelState["requestModel.Name"] as string[];
                var emailError = modelState["requestModel.Email"] as string[];

                // Assert
                Assert.NotNull(nameError);
                Assert.NotNull(emailError);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_200_And_User_If_Request_Authorized_But_Request_Is_Valid()
            {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                // This is a valid user request to update one
                var userRequestModel = new UserUpdateRequestModel
                {
                    Name = "FooBar",
                    Email = "FooBar@example.com",
                };

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/users",
                            keys[2]),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                request.Content = new ObjectContent<UserUpdateRequestModel>(
                    userRequestModel, new JsonMediaTypeFormatter());

                // Act
                var userDto = await IntegrationTestHelper.GetResponseMessageBodyAsync<UserDto>(config, request, HttpStatusCode.OK);

                // Assert
                Assert.Equal(userRequestModel.Name, userDto.Name);
                Assert.Equal(userRequestModel.Email, userDto.Email);
            }

            [Fact, NullCurrentPrincipal]
            public async Task
                Returns_400_If_Request_Authorized_But_Message_Body_Is_Empty()
            {

                // Arrange
                Guid[] keys = new[] { 
                    Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()
                };

                var config = IntegrationTestHelper
                    .GetInitialIntegrationTestConfig(
                        GetInitialServices(GetMembershipService(keys)));

                var request = HttpRequestMessageHelper
                    .ConstructRequest(
                        httpMethod: HttpMethod.Put,
                        uri: string.Format(
                            "https://localhost/{0}/{1}",
                            "api/users",
                            keys[1]),
                        mediaType: "application/json",
                        username: Constants.ValidAdminUserName,
                        password: Constants.ValidAdminPassword);

                // Act
                var httpError = await IntegrationTestHelper
                    .GetResponseMessageBodyAsync<HttpError>(
                        config, request, HttpStatusCode.BadRequest);

                var modelState = (HttpError)httpError["ModelState"];
                var requestModelError = modelState["requestModel"] as string[];

                // Assert
                Assert.NotNull(requestModelError);
            }

            private static IMembershipService GetMembershipService(Guid[] keys)
            {

                var users = GetDummyUsers(keys);
                var mockMemSrv = ServicesMockHelper
                    .GetInitialMembershipServiceMock();

                mockMemSrv.Setup(ms => ms.GetUser(
                        It.Is<Guid>(
                            key => keys.Contains(key)
                        )
                    )
                ).Returns<Guid>(key =>
                    users.FirstOrDefault(x =>
                        x.User.Key == key
                    )
                );

                mockMemSrv.Setup(ms => ms.UpdateUser(
                        It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()
                    )
                ).Returns<User, string, string>(
                    (user, username, email) =>
                    {

                        var roles = users
                            .FirstOrDefault(
                                x => x.User.Name.Equals(user.Name, StringComparison.OrdinalIgnoreCase)).Roles;

                        user.Name = username;
                        user.Email = email;
                        return new UserWithRoles
                        {
                            User = user,
                            Roles = roles
                        };
                    }
                );

                return mockMemSrv.Object;
            }
        }

        private static IContainer GetInitialServices(
            IMembershipService memSrv)
        {

            var builder = IntegrationTestHelper
                .GetEmptyContainerBuilder();

            builder.Register(c => memSrv)
                .As<IMembershipService>()
                .InstancePerApiRequest();

            return builder.Build();
        }

        private static IEnumerable<UserWithRoles> GetDummyUsers(Guid[] keys)
        {

            #region Dummy User List
            List<UserWithRoles> users = new List<UserWithRoles> { 
                new UserWithRoles { 
                    User = new User { 
                        Key = keys[0],
                        Name = Constants.ValidAdminUserName,
                        Email = "fooAdmin@example.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { 
                            Key = Guid.NewGuid(), Name = "Admin"
                        }
                    }
                },

                new UserWithRoles { 
                    User = new User { 
                        Key = keys[1],
                        Name = Constants.ValidAffiliateUserName,
                        Email = "fooAffiliate@example.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { 
                            Key = Guid.NewGuid(), Name = "Affiliate"
                        }
                    }
                },

                new UserWithRoles { 
                    User = new User { 
                        Key = keys[2],
                        Name = Constants.ValidEmployeeUserName,
                        Email = "fooEmployee@example.com",
                        IsLocked = false,
                        CreatedOn = DateTime.Now.AddDays(-10),
                        LastUpdatedOn = DateTime.Now.AddDays(-5)
                    },
                    Roles = new List<Role> { 
                        new Role { 
                            Key = Guid.NewGuid(), Name = "Employee"
                        }
                    }
                }
            };
            #endregion

            return users;
        }
    }
}