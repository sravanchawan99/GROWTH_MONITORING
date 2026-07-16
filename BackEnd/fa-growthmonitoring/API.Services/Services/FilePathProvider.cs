using API.Services.Models;

namespace API.Services.Services
{
    public class FilePathProvider
    {
        public string ProvideFilePath(Beneficiary child, string type)
        {
            if (type.Equals("WLZ", StringComparison.OrdinalIgnoreCase))
            {
                if (child.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
                {
                    return Path.Combine(
                        AppContext.BaseDirectory,
                        "Data",
                        "Boys",
                        child.age < 24
                            ? "wfl_boys_0-to-2-years_zscores.xlsx"
                            : "wfh_boys_2-to-5-years_zscores.xlsx");
                }

                return Path.Combine(
                    AppContext.BaseDirectory,
                    "Data",
                    "Girls",
                    child.age < 24
                        ? "wfl_girls_0-to-2-years_zscores.xlsx"
                        : "wfh_girls_2-to-5-years_zscores.xlsx");
            }
            else if (type.Equals("WAZ", StringComparison.OrdinalIgnoreCase))
            {
                if (child.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
                {
                    return Path.Combine(
                        AppContext.BaseDirectory,
                        "Data",
                        "Boys",
                        "wfa_boys_0-to-5-years_zscores.xlsx");
                }

                return Path.Combine(
                    AppContext.BaseDirectory,
                    "Data",
                    "Girls",
                    "wfa_girls_0-to-5-years_zscores.xlsx");
            }
            else if (type.Equals("LAZ", StringComparison.OrdinalIgnoreCase))
            {
                if (child.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
                {
                    return Path.Combine(
                        AppContext.BaseDirectory,
                        "Data",
                        "Boys",
                        child.age < 24
                            ? "lhfa_boys_0-to-2-years_zscores.xlsx"
                            : "lhfa_boys_2-to-5-years_zscores.xlsx");
                }

                return Path.Combine(
                    AppContext.BaseDirectory,
                    "Data",
                    "Girls",
                    child.age < 24
                        ? "lhfa_girls_0-to-2-years_zscores.xlsx"
                        : "lhfa_girls_2-to-5-years_zscores.xlsx");
            }
            

            throw new NotSupportedException($"{type} is not supported.");
        }
    }
}