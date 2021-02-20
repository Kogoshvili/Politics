using System.Threading.Tasks;
using Politics.Data;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace PoliticsNet.Data
{
    public class PhoneRespository : IPhoneRespository
    {
        private readonly DataContext _context;
        public PhoneRespository(DataContext context)
        {
            _context = context;
        }
        private void TwilioInit()
        {
            string accountSid = "ACd62e592c7975abdedde0980d7d3b6a4a";
            //_config.GetSection("AppSettings:Twilio:AccountSid").Value;
            string authToken = "f17f1ef048e449893887595abc720363";
            //_config.GetSection("AppSettings:Twilio:AuthToken").Value;
            //VA4ecd8e6e80f84392b619552df9a6ed47
            TwilioClient.Init(accountSid, authToken);
        }
        public async Task<string> CheckCode(string phone, string code)
        {
            TwilioInit();
            try{
                var verificationCheck = await VerificationCheckResource.CreateAsync(
                    to: phone,
                    code: code,
                    pathServiceSid: "VA4ecd8e6e80f84392b619552df9a6ed47"
                );
                return verificationCheck.Status;
            }catch {
                return "Error";
            }

        }

        public async Task<string> SendCode(string phone)
        {
            TwilioInit();
            try{
                var verification = await VerificationResource.CreateAsync(
                    to: phone,
                    channel: "sms",
                    pathServiceSid: "VA4ecd8e6e80f84392b619552df9a6ed47"
                );
                return verification.Status;
            }catch  {
                return "Error";
            }
        }

    }
}