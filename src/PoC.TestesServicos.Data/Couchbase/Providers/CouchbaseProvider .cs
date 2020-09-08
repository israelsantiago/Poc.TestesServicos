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
           
            _cluster = await Cluster.ConnectAsync(hosts, username, password).ConfigureAwait(false);
            
            return _cluster;
        }

        public async Task<IBucket> GetBucket()
        {
            if (_bucket == null)
            {
                var bucketName = _configuration.GetValue<string>("Couchbase:BucketName");

                var cluster = await GetCluster();
                
                _bucket = await cluster.BucketAsync(bucketName).ConfigureAwait(false);
            }

            return _bucket;
        }
    }
}