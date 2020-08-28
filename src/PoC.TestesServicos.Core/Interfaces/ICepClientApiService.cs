using Refit;
using System.Threading.Tasks;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Core.Interfaces
{
    public interface ICepClientApiService
    {
        [Get("/ws/{cep}/json")]
        Task<CepModel> GetAddressAsync(string cep);
    }
}