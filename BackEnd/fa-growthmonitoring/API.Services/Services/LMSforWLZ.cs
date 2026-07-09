using API.Services.Models;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace API.Services.Services
{
    public class LMSforWLZ : ILMSService
    {
        private readonly ILogger<LMSforWLZ> _logger;

        public LMSforWLZ(ILogger<LMSforWLZ> logger)
        {
            _logger = logger;
        }

        private Dictionary<double, LMS> LoadExcel(string filePath)
        {
            var lookup = new Dictionary<double, LMS>();

            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var height = row.Cell(1).GetDouble();

                lookup[height] = new LMS
                {
                    L = row.Cell(2).GetDouble(),
                    M = row.Cell(3).GetDouble(),
                    S = row.Cell(4).GetDouble()
                };
            }

            return lookup;
        }

        public Task<LMS> ProvideLMSforBoy(Beneficiary child, string filePath)
        {
            var lookup = LoadExcel(filePath);

            double height = Math.Round(child.height * 2, MidpointRounding.AwayFromZero) / 2.0;

            if (lookup.TryGetValue(height, out var lms))
            {
                return Task.FromResult(lms);
            }

            _logger.LogWarning("No LMS found for height {Height}", height);

            throw new KeyNotFoundException($"No LMS found for height {height}");
        }

        public Task<LMS> ProvideLMSforGirl(Beneficiary child, string filePath)
        {
            var lookup = LoadExcel(filePath);

            double height = Math.Round(child.height * 2, MidpointRounding.AwayFromZero) / 2.0;

            if (lookup.TryGetValue(height, out var lms))
            {
                return Task.FromResult(lms);
            }

            _logger.LogWarning("No LMS found for height {Height}", height);

            throw new KeyNotFoundException($"No LMS found for height {height}");
        }
    }
}