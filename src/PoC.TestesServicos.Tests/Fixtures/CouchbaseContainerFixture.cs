using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class CouchbaseContainerFixture : IAsyncLifetime
    {
        public CouchbaseContainerFixture()
        {
            var testcontainersBuilder = new TestcontainersBuilder<CouchbaseTestcontainer>()
                    .WithDatabase(new CouchbaseTestcontainerConfiguration
                    {
                        Username = "couchbase",
                        Password = "couchbase",
                        BucketName = "customers"
                    })
                    .WithExposedPort(8091)
                    .WithExposedPort(8093)
                    .WithExposedPort(11210)
                    .WithPortBinding(8091, 8091)
                    .WithPortBinding(8093, 8093)
                    .WithPortBinding(11210, 11210)
                ;

            Container = testcontainersBuilder.Build();
        }

        public CouchbaseTestcontainer Container { get; }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();

            /*
            using (var cluster = await Cluster.ConnectAsync(
                this.Container.ConnectionString,
                this.Container.Username,
                this.Container.Password))
            {
                var buckets = await cluster.Buckets.GetAllBucketsAsync();

                await using (var bucket = await cluster.BucketAsync("customers"))
                {
                    var collection = bucket.DefaultCollection();

                    //await cluster.QueryAsync<long>($"CREATE PRIMARY INDEX `#primary` ON `customers`");

                }
            }
            */
        }

        public Task DisposeAsync()
        {
            return Container.DisposeAsync().AsTask();
        }
    }
}