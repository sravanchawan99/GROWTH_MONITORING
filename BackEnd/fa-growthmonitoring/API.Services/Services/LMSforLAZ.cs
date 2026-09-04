using API.Services.Models;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace API.Services.Services
{
    public class LMSforLAZ : ILMSService
    {
        private readonly ILogger<LMSforLAZ> _logger;

        public LMSforLAZ(ILogger<LMSforLAZ> logger)
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
                var age = row.Cell(1).GetDouble();

                lookup[age] = new LMS
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

            double age = child.age;

            if (lookup.TryGetValue(age, out var lms))
            {
                return Task.FromResult(lms);
            }

            _logger.LogWarning("No LMS found for height {age}", age);

            throw new KeyNotFoundException($"No LMS found for age {age}");
        }
        public Task<LMS> ProvideLMSforGirl(Beneficiary child, string filePath)
        {
            var lookup = LoadExcel(filePath);

            double age = child.age;

            if (lookup.TryGetValue(age, out var lms))
            {
                return Task.FromResult(lms);
            }

            _logger.LogWarning("No LMS found for height {age}", age);

            throw new KeyNotFoundException($"No LMS found for age {age}");
        }
    }
}