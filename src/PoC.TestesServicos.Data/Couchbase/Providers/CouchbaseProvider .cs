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

            //
            // _cluster = await Cluster.ConnectAsync("couchbase://localhost", username, password);

            _cluster = await Cluster.ConnectAsync($"{hosts}", new ClusterOptions
            {
                BootstrapHttpPort = uiPort,
                UserName = username,
                Password = password
            });


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