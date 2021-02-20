using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public class UserRespository : IUserRespository
    {
        private readonly DataContext _context;
        public UserRespository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}