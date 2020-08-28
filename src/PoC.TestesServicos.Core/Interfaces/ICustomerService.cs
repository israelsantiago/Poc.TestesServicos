using System.Threading.Tasks;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerByCode(int code);        
    }
}