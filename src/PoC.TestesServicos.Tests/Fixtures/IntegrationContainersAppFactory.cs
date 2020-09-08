using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PoC.TestesServicos.API;
using PoC.TestesServicos.Data;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class IntegrationContainersAppFactory<TStartup> : WebApplicationFactory<Startup> where TStartup : class
    {
        private readonly TestContextConfiguration _testtontexttonfiguration;
        private readonly string _usernamecouchbase;
        private readonly string _passwordCouchbase;
        private readonly string _hostsCouchbase;
        private readonly string _mockeserverurl;

        public IntegrationContainersAppFactory(TestContextConfiguration testContextConfigurationDb,
                                               string hostsCouchBase, string userNameCouchBase, string passwordCouchbase, 
                                               string mockeServerUrl)
        {
            _testtontexttonfiguration = testContextConfigurationDb;
            _hostsCouchbase = hostsCouchBase;
            _usernamecouchbase = userNameCouchBase;
            _passwordCouchbase = passwordCouchbase;
            _mockeserverurl = mockeServerUrl;
        }

        private const string CEP_API_URL_SECTION = "CepApiOptions:Url";           

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Dictionary<string, string> extraConfiguration = GetApiClientExtraConfiguration();
            
            builder.ConfigureTestServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                services.Replace(new ServiceDescriptor(typeof(IContextConfiguration), _testtontexttonfiguration));
   
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                configuration["Couchbase:Hosts"] = _hostsCouchbase;
                configuration["Couchbase:Username"] = _usernamecouchbase;
                configuration["Couchbase:Password"] = _passwordCouchbase;
                services.Replace(new ServiceDescriptor(typeof(IConfiguration), configuration));

            }).ConfigureAppConfiguration((context, configbuilder) =>
            {
                configbuilder.AddInMemoryCollection(extraConfiguration);
                
            }).UseUrls("http://*:0")
              .UseStartup<TStartup>()
              .UseEnvironment("Development");
            
        }
        
        /// <summary>
        /// This method creates configurations using mockserver url, that will be used by the API instead of configured in appsettings, this setup allows Wire Mock Server matches the http request mapped.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetApiClientExtraConfiguration()
        {
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            configuration.Add(CEP_API_URL_SECTION, _mockeserverurl);
            return configuration;
        }

    }
}