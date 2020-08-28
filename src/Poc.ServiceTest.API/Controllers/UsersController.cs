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
        private readonly UsersDataContext _context;
        private readonly IDocumentsRepository _documento;

        public UsersController(UsersDataContext context, IDocumentsRepository documento)
        {
            _context = context;
            _documento = documento;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAsync()
        {
            var sampleDocument = new Document
            {
                Id = "Document001",
                Data = "Sample document for Save_ItShouldSaveDocument test."
            };


            await _documento.Save(sampleDocument);

            var document = await _documento.FindById("Document001");


            return Ok(_context.Users.ToArray());
        }
    }
}