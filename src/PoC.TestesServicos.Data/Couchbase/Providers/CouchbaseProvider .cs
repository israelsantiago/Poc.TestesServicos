using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Couchbase;
using Microsoft.Extensions.Configuration;

namespace PoC.TestesServicos.Data.Couchbase.Providers
{
    public class CouchbaseProvider : ICouchbaseProvider
    {
        private IBucket _bucket;
        private ICluster _cluster;
        
        public async Task<ICluster> GetCluster()
        {
            if (_cluster != null) return _cluster;

            var hosts    = Environment.GetEnvironmentVariable("COUCHBASE_HOSTS");
            var username = Environment.GetEnvironmentVariable("COUCHBASE_USER_NAME");
            var password = Environment.GetEnvironmentVariable("COUCHBASE_PASSWORD");
            
            _cluster = await Cluster.ConnectAsync(hosts, username, password).ConfigureAwait(false);
            
            return _cluster;
        }

        public async Task<IBucket> GetBucket()
        {
            if (_bucket == null)
            {
                var bucketName = Environment.GetEnvironmentVariable("COUCHBASE_BUCKET_NAME");

                var cluster = await GetCluster();
                
                _bucket = await cluster.BucketAsync(bucketName).ConfigureAwait(false);
            }

            return _bucket;
        }
    }
}