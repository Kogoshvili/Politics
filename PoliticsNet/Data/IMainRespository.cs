using System.Threading.Tasks;

namespace PoliticsNet.Data
{
    public interface IMainRespository
    {
        void Add<T>(T entity) where T: class;
        void Update<T>(T entity) where T: class;
        void Delete<T>(T entity) where T: class;
        Task<bool> SaveAll();
    }
}