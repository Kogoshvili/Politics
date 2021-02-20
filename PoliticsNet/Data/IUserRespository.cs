using System.Threading.Tasks;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IUserRespository
    {
         Task<User> GetUser(int id);
    }
}