using System.IO;
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

            var user2 = new User {FirstName = "Pedro", LastName = "Faria"};
            Context.Add(user2);
            Context.SaveChanges();

            var users = await Client.GetAsync("api/users").DeserializeResponseAsync<User[]>();

            Assert.Equal(2, users.Length);
        }
        
        [Theory]
        [InlineData("Israel", "Santiago")]
        [InlineData("Carlos", "Pedrosa")]
        [InlineData("Enzo", "Santiago")]
        public async Task GetUsers_UsersInDb_ShouldReturnAddedUsersParametrized(string firstName, string lastName)
        {
            var user = new User {FirstName = firstName, LastName = lastName};
            Context.Add(user);
            Context.SaveChanges();

            var users = await Client.GetAsync("api/users").DeserializeResponseAsync<User[]>();

            Assert.Single(users);
            Assert.Equal(firstName, users[0].FirstName);
            Assert.Equal(lastName, users[0].LastName);
        }        
        
        
    }
}