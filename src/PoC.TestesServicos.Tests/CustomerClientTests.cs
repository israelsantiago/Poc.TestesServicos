using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PoC.TestesServicos.Data.Models;
using PoC.TestesServicos.Tests.Fixtures;
using Xunit;

namespace PoC.TestesServicos.Tests
{
    [Collection("Integration containers collection")]
    public class CustomerClientTests : ControllerTestsBase
    {
        public CustomerClientTests(IntegrationContainersAppFactory integrationContainersFixture) 
            : base(integrationContainersFixture)
        {
        }
        
        [Theory]
        [Trait("Categoria", "Componente")]
        [InlineData("1")]
        [InlineData("2")]
        public async Task Should_Get_A_Valid_Customer_With_CepDetails_For_Customer_Code(string customerCode)
        {
            string actualResponseContent;
            HttpResponseMessage response;
            Customer customerResponse;

            response = await Client.GetAsync($"/api/Customer/{customerCode}");
            response.EnsureSuccessStatusCode();

            actualResponseContent = await response.Content.ReadAsStringAsync();
            customerResponse = JsonConvert.DeserializeObject<Customer>(actualResponseContent);

            Assert.Equal("Mocked", customerResponse.CepDetails.Localidade);
        }
        
        
    }
}