using System.Threading.Tasks;
using Couchbase.Core.Exceptions.KeyValue;
using PoC.TestesServicos.Data.Couchbase.Models;
using PoC.TestesServicos.Data.Couchbase.Providers;

namespace PoC.TestesServicos.Data.Couchbase.Repositories
{
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly ICouchbaseProvider _couchbase;

        public DocumentsRepository(ICouchbaseProvider couchbase)
        {
            _couchbase = couchbase;
        }

        public async Task<Document> FindById(string id)
        {
            try
            {
                var couchbaseCollection = (await _couchbase.GetBucket()).DefaultCollection();
                var result = await couchbaseCollection.GetAsync(id);
                return result.ContentAs<Document>();
            }
            catch (DocumentNotFoundException)
            {
                return null;
            }
        }
        
        public async Task Save(Document document)
        {
            var couchbaseCollection = (await _couchbase.GetBucket()).DefaultCollection();
            await couchbaseCollection.UpsertAsync(document.Id, document);
        }
    }
}