using API.Services.Models;

namespace API.Services.Services
{
    public interface ILMSService
    {
        Task<LMS> ProvideLMSforBoy(Beneficiary child, string filePath);

        Task<LMS> ProvideLMSforGirl(Beneficiary child, string filePath);
    }
}