using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PoC.TestesServicos.Data;
using PoC.TestesServicos.Data.Couchbase.Models;
using PoC.TestesServicos.Data.Couchbase.Repositories;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersDataContext _usersDataContext;
        private readonly IDocumentsRepository _documento;

        public UsersController(UsersDataContext usersDataContext, IDocumentsRepository documento)
        {
            _usersDataContext = usersDataContext;
            _documento = documento;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAsync()
        { 
            // Teste Couchbase
            var sampleDocument = new Document
            {
                Id = "Document001",
                Data = "Sample document for Save_ItShouldSaveDocument test."
            };

            await _documento.Save(sampleDocument);

            var document = await _documento.FindById("Document001");

            // Testes RabbitMQ
            /*
            var factory = new ConnectionFactory { Uri = new Uri(rabbitmqContainerFixture.Container.ConnectionString) };

            using (var connection = factory.CreateConnection())
            {
                
                IModel channel = connection.CreateModel();
                
                Assert.True(connection.IsOpen);
            }
            */
            
            // Teste MongoDB !

            return Ok(_usersDataContext.Users.ToArray());
        }
    }
}