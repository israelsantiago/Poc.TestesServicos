using System.Threading.Tasks;
using PoC.TestesServicos.Data.Couchbase.Models;

namespace PoC.TestesServicos.Data.Couchbase.Repositories
{
    public interface IDocumentsRepository
    {
        Task<Document> FindById(string id);
        Task Save(Document document);
    }
}