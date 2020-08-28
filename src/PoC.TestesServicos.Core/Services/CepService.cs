using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Configuration;
using PoC.TestesServicos.Core.Interfaces;
using PoC.TestesServicos.API.Configs;
using PoC.TestesServicos.Data.Models;
using Refit;


namespace PoC.TestesServicos.Core.Services
{
    public class CepService : ICepService
    {
        private readonly CepApiOptions _cepApiOptions;

        public CepService(IOptions<CepApiOptions> cepApiOptions)
        {
            _cepApiOptions = cepApiOptions.Value;
        }

        public async Task<CepModel> GetCepDetails(string cep)
        {
            if (_cepApiOptions == null)
                throw new ConfigurationErrorsException("Cep API Url setting must be configured!");

            ICepClientApiService client = RestService.For<ICepClientApiService>(_cepApiOptions.Url);

            CepModel cepDetails = await client.GetAddressAsync(cep);

            return cepDetails;
        }
    }
}