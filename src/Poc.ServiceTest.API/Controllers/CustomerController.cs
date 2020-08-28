using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PoC.TestesServicos.Core.Interfaces;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
       
        
        [HttpGet("{code:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(int code)
        {
            Customer customerResponse = await _customerService.GetCustomerByCode(code);
            
            //Customer customerResponse = null;
            return Ok(customerResponse);
        }
    }
}