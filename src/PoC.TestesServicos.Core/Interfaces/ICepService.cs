using System.Threading.Tasks;
using PoC.TestesServicos.Data.Models;

namespace PoC.TestesServicos.Core.Interfaces
{
    public interface ICepService
    {
        Task<CepModel> GetCepDetails(string cep);        
    }
}