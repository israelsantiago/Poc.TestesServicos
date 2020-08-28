using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PoC.TestesServicos.Core.Interfaces;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICepService _cepService;

        public CustomerService(ICepService cepService)
        {
            _cepService = cepService;
        }

        public async Task<Customer> GetCustomerByCode(int code)
        {
            Customer customerResponse = CustomerFakeRepository(c => c.Code == code)
                .FirstOrDefault();

            if(customerResponse != null && !string.IsNullOrEmpty(customerResponse.Cep))
                customerResponse.CepDetails = await _cepService.GetCepDetails(customerResponse.Cep);

            return customerResponse;
        }

      private IEnumerable<Customer> CustomerFakeRepository(Func<Customer, bool> predicate)
        {
            IList<Customer> customers = new List<Customer>
            {
                new Customer { Code = 1, Name = "Customer 01", Cep = "95600106" },
                new Customer { Code = 2, Name = "Customer 02", Cep = "95600104" }
            };

            return customers.Where(predicate);
        }
      }    
    
}