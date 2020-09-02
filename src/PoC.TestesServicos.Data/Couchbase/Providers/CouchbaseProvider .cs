using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Microsoft.Extensions.Configuration;

namespace PoC.TestesServicos.Data.Couchbase.Providers
{
    public class CouchbaseProvider : ICouchbaseProvider
    {
        private readonly IConfiguration _configuration;
        private IBucket _bucket;
        private ICluster _cluster;

        public CouchbaseProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ICluster> GetCluster()
        {
            if (_cluster != null) return _cluster;


            var hosts = _configuration.GetValue<string>("Couchbase:Hosts");
            var username = _configuration.GetValue<string>("Couchbase:Username");
            var password = _configuration.GetValue<string>("Couchbase:Password");
            var uiPort = _configuration.GetValue<int>("Couchbase:UIPort");
            //   // $"{hosts}:{uiPort}"

            //var teste = $"127.0.0.1:{uiPort}";
            
            //127.0.0.1:8091 
            //$"http://localhost:{uiPort}"
            //$"http://localhost:{uiPort}"
            
            //_cluster = await Cluster.ConnectAsync($"http://localhost:{uiPort}" , 
             //                                      username, password);

            var options = new ClusterOptions().WithBuckets("customers")
                                              .WithCredentials("couchbase", "couchbase")
                                              .WithConnectionString($"http://localhost:{uiPort}");
                
             //options.BootstrapHttpPort = uiPort;
             //options.BootstrapHttpPortTls = uiPort;

             options.KvTimeout = TimeSpan.FromSeconds(60);
             options.KvConnectTimeout = TimeSpan.FromSeconds(60);
   
 
            ICluster cluster = await Cluster.ConnectAsync(options).ConfigureAwait(false);
            await cluster.WaitUntilReadyAsync(TimeSpan.FromSeconds(60));            
            
  
            return _cluster;
        }


        public async Task<IBucket> GetBucket()
        {
            if (_bucket == null)
            {
                var bucketName = _configuration.GetValue<string>("Couchbase:BucketName");

                var cluster = await GetCluster();

                _bucket = await cluster.BucketAsync(bucketName);
            }

            return _bucket;
        }
    }
}