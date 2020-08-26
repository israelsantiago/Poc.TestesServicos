using System.Threading.Tasks;
using Couchbase;

namespace PoC.TestesServicos.Data.Couchbase.Providers
{
    public interface ICouchbaseProvider
    {
        Task<ICluster> GetCluster();
        Task<IBucket> GetBucket();
    }
}