using API.Services.Models;
using Microsoft.Extensions.Logging;

namespace API.Services.Services
{
    public class LMSService
    {
        private readonly ILogger<LMSService> _logger;
        private readonly LMSforWLZ _lmsforWLZ;
        private readonly FilePathProvider _filePathProvider;

        public LMSService(
            ILogger<LMSService> logger,
            LMSforWLZ lmsforWLZ,
            FilePathProvider filePathProvider)
        {
            _logger = logger;
            _lmsforWLZ = lmsforWLZ;
            _filePathProvider = filePathProvider;
        }

        public async Task<LMS?> GetLMSValues(Beneficiary child, string type)
        {
            if (child == null)
                return null;

            switch (type.ToUpper())
            {
                case "WLZ":

                    string filePath = _filePathProvider.ProvideFilePath(child, type);

                    if (child.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
                    {
                        return await _lmsforWLZ.ProvideLMSforBoy(child, filePath);
                    }

                    return await _lmsforWLZ.ProvideLMSforGirl(child, filePath);

                default:

                    throw new NotSupportedException($"{type} is not supported.");
            }
        }
    }
}