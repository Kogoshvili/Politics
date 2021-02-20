using System.Threading.Tasks;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserNameExists(string username);
         Task<bool> PhoneExists(string phone);
    }
}