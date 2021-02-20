using System.Collections.Generic;
using System.Threading.Tasks;
using PoliticsNet.Models;

namespace PoliticsNet.Data
{
    public interface IProviderRespository
    {
        Task<IEnumerable<Provider>> GetProviderList();
        Task<Provider> GetProvider(string name);
    }
}