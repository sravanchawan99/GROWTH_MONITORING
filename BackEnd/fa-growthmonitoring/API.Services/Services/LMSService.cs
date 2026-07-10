using API.Services.Models;
using Microsoft.Extensions.Logging;

namespace API.Services.Services
{
    public class LMSService
    {
        private readonly ILogger<LMSService> _logger;
        private readonly LMSforWLZ _lmsforWLZ;
        private readonly LMSforWAZ _lmsforWAZ;
        private readonly FilePathProvider _filePathProvider;
        public string filePath;

        public LMSService(
            ILogger<LMSService> logger,
            LMSforWLZ lmsforWLZ,
            FilePathProvider filePathProvider, LMSforWAZ lmsforWAZ)
        {
            _logger = logger;
            _lmsforWLZ = lmsforWLZ;
            _lmsforWAZ = lmsforWAZ;
            _filePathProvider = filePathProvider;
        }

        public async Task<LMS?> GetLMSValues(Beneficiary child, string type)
        {
            if (child == null)
                return null;

            switch (type.ToUpper())
            {
                case "WLZ":

                    filePath = _filePathProvider.ProvideFilePath(child, type);

                    if (child.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
                    {
                        return await _lmsforWLZ.ProvideLMSforBoy(child, filePath);
                    }

                    return await _lmsforWAZ.ProvideLMSforGirl(child, filePath);
                case "WAZ":

                    filePath = _filePathProvider.ProvideFilePath(child, type);

                    if (child.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
                    {
                        return await _lmsforWAZ.ProvideLMSforBoy(child, filePath);
                    }

                    return await _lmsforWAZ.ProvideLMSforGirl(child, filePath);

                default:

                    throw new NotSupportedException($"{type} is not supported.");
            }
        }
    }
}