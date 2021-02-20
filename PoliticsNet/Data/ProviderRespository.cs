using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Politics.Data;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public class ProviderRespository : IProviderRespository
    {
        private readonly DataContext _context;
        public ProviderRespository(DataContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Provider>> GetProviderList()
        {
            return await _context.Providers.ToListAsync();
        }
        public async Task<Provider> GetProvider(string name)
        {
            return await _context.Providers.FirstOrDefaultAsync(p => p.Name == name);
        }

    }
}
