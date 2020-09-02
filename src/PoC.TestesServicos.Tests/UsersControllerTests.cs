using System.Net;
using System.Threading.Tasks;
using PoC.TestesServicos.API;
using PoC.TestesServicos.Data.Models;
using PoC.TestesServicos.Tests.Extensions;
using PoC.TestesServicos.Tests.Fixtures;
using Xunit;

namespace PoC.TestesServicos.Tests
{
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class UsersControllerTests : ControllerTestsBase
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        
        public UsersControllerTests(IntegrationTestFixture<StartupApiTests> integrationTestFixture) 
            : base(integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

      
        [Fact]
        public async Task GetUsers_NoUsers_ShouldReturnOk()
        {
            var response = await Client.GetAsync("api/users");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUsers_UsersInDb_ShouldReturnAddedUsers()
        {
            var user = new User {FirstName = "Sam", LastName = "Jonson"};
            Context.Add(user);
            Context.SaveChanges();

            var users = await Client.GetAsync("api/users").DeserializeResponseAsync<User[]>();

            Assert.Single(users);
        }
     
        [Fact]
        public async Task GetUsers_TwoUsersInDb_ShouldReturnTwoAddedUsers()
        {
            var user = new User {FirstName = "Sam", LastName = "Jonson"};
            Context.Add(user);
            Context.SaveChanges();

            var user2 = new User {FirstName = "Israel", LastName = "Santiago"};
            Context.Add(user2);
            Context.SaveChanges();

            var users = await Client.GetAsync("api/users").DeserializeResponseAsync<User[]>();

            Assert.Equal(2, users.Length);
        }
    }
}