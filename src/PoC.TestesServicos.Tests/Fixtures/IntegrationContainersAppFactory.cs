using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using PoC.TestesServicos.API;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class IntegrationContainersAppFactory<TStartup> : WebApplicationFactory<Startup> where TStartup : class
    {
        private readonly string _mockeserverurl;
        private const string CEP_API_URL_SECTION = "CepApiOptions:Url";           

        public IntegrationContainersAppFactory(string connectionStringDb,
                                               string hostsCouchBase, 
                                               string userNameCouchBase,
                                               string passwordCouchbase, 
                                               string bucketName,
                                               string mockeServerUrl)
        {
            _mockeserverurl = mockeServerUrl;
            
            Environment.SetEnvironmentVariable("COUCHBASE_HOSTS", hostsCouchBase);
            Environment.SetEnvironmentVariable("COUCHBASE_USER_NAME", userNameCouchBase);
            Environment.SetEnvironmentVariable("COUCHBASE_PASSWORD", passwordCouchbase);
            Environment.SetEnvironmentVariable("COUCHBASE_BUCKET_NAME", bucketName);
                
            Environment.SetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING", connectionStringDb);                
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Hook for possible changes to injected services
                
            }).ConfigureAppConfiguration((context, configbuilder) =>
            {
                Dictionary<string, string> apiClientMockServerConfiguration = GetApiClientMockServerConfiguration();
                
                configbuilder.AddInMemoryCollection(apiClientMockServerConfiguration);
                
            }).UseUrls("http://*:0")
              .UseStartup<TStartup>()
              .UseEnvironment("Testing");
            
        }
     
        // This method creates configurations using mockserver url, that will be used by the API instead of configured in appsettings, this setup allows Wire Mock Server matches the http request mapped.
        private Dictionary<string, string> GetApiClientMockServerConfiguration()
        {
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            configuration.Add(CEP_API_URL_SECTION, _mockeserverurl);
            return configuration;
        }
    }
}