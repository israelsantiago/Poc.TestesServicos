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

            string hosts    = Environment.GetEnvironmentVariable("COUCHBASE_HOSTS") ?? throw new ArgumentNullException(nameof(hosts), "Variável de ambiente COUCHBASE_HOSTS inexistente.");
            string username = Environment.GetEnvironmentVariable("COUCHBASE_USER_NAME") ?? throw new ArgumentNullException(nameof(username), "Variável de ambiente COUCHBASE_USER_NAME inexistente.");            
            string password = Environment.GetEnvironmentVariable("COUCHBASE_PASSWORD") ?? throw new ArgumentNullException(nameof(password), "Variável de ambiente COUCHBASE_PASSWORD inexistente.");        
            
            _cluster = await Cluster.ConnectAsync(hosts, username, password).ConfigureAwait(false);
            
            return _cluster;
        }

        public async Task<IBucket> GetBucket()
        {
            if (_bucket == null)
            {
                string bucketName = Environment.GetEnvironmentVariable("COUCHBASE_BUCKET_NAME") ?? throw new ArgumentNullException(nameof(bucketName), "Variável de ambiente COUCHBASE_BUCKET_NAME inexistente.");                

                var cluster = await GetCluster();
                
                _bucket = await cluster.BucketAsync(bucketName).ConfigureAwait(false);
            }

            return _bucket;
        }
    }
}