using System.Threading.Tasks;

namespace PoliticsNet.Data
{
    public interface IPhoneRespository
    {
        Task<string>   SendCode(string phone);
        Task<string>   CheckCode(string phone, string code);
    }
}