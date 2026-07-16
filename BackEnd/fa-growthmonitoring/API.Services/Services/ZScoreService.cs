using API.Services.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace API.Services.Services
{
    public class ZScoreService
    {
        private readonly ILogger<ZScoreService> _logger;
        private readonly string _apiurl;
        public ZscoreResult result;
        public ZScoreService(ILogger<ZScoreService> logger,IConfiguration config)
        {
            _logger = logger;
            _apiurl=config["APIUrl"];
        }
        public async Task<ZscoreResult> ProvideZscore(Beneficiary child)
        {
            _logger.LogInformation("Started Processing Request......");
            HttpClient client=new HttpClient();
            // Weight for Length
            _logger.LogInformation("Wasting.....");
            child.type="WLZ";
            var json = JsonSerializer.Serialize(child);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
           _logger.LogInformation("Sending request to {ApiUrl}", _apiurl);
            var response=await client.PostAsync(_apiurl,content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var lms = JsonSerializer.Deserialize<LMS>(responseContent);
            if(lms!=null)
            _logger.LogInformation("LMS Values for Weight for Length as per WHO are L:{L}, M:{M},S:{S}", lms.L,lms.M,lms.S);
            var z=CalculateZscore(lms,child.weight);
            var wasting=Categorize(z);
            var nwl=CalculateIdealParameter(lms);
            _logger.LogInformation("Z Score Values for Weight for Length as per Calculation {Z}", z);
            // Weight for Age
            _logger.LogInformation("Underweight.....");
            child.type="WAZ";
            var json1 = JsonSerializer.Serialize(child);
            var content1 = new StringContent(json1,Encoding.UTF8,"application/json");
           _logger.LogInformation("Sending request to {ApiUrl}", _apiurl);
            var response1=await client.PostAsync(_apiurl,content1);
            var responseContent1 = await response1.Content.ReadAsStringAsync();
            var lms1 = JsonSerializer.Deserialize<LMS>(responseContent1);
            if(lms1!=null)
            _logger.LogInformation("LMS Values for Weight for Age as per WHO are L:{L}, M:{M},S:{S}", lms1.L,lms1.M,lms1.S);
            var z1=CalculateZscore(lms1,child.weight);
            var underweight=Categorize(z1);
            var nwa=CalculateIdealParameter(lms1);
            _logger.LogInformation("Z Score Values for Weight for age as per Calculation is {Z}", z1);
            // Height for Age
            _logger.LogInformation("Stunting.....");
            child.type="LAZ";
            var json2 = JsonSerializer.Serialize(child);
            var content2 = new StringContent(json2,Encoding.UTF8,"application/json");
           _logger.LogInformation("Sending request to {ApiUrl}", _apiurl);
            var response2=await client.PostAsync(_apiurl,content2);
            var responseContent2 = await response2.Content.ReadAsStringAsync();
            var lms2 = JsonSerializer.Deserialize<LMS>(responseContent2);
            if(lms2!=null)
            _logger.LogInformation("LMS Values for Height for Age as per WHO are L:{L}, M:{M},S:{S}", lms2.L,lms2.M,lms2.S);
            var z2=CalculateZscore(lms2,child.height);
            var stunting=Categorize(z2);
            var nla=CalculateIdealParameter(lms2);
             _logger.LogInformation("Z Score Values for Length for age as per Calculation is {Z}", z2);
            return new ZscoreResult
            {
                Wasting=wasting,
                Underweight=underweight,
                Stunting=stunting,
                NWL=nwl,
                NWA=nwa,
                NLA=nla
            };
        }
        public static double CalculateZscore(LMS lms, double x)
        {
            double l=lms.L;
            double m=lms.M;
            double s=lms.S;
            double Zscore= (Math.Pow((x/m),l)-1)/(l*s);
            return Zscore;
        }
        public double CalculateIdealParameter(LMS lms)
        {
            double l=lms.L;
            double m=lms.M;
            double s=lms.S;
            double X= m*Math.Pow((1+l*s*-2),(1/l));
            return Math.Round(X,2);
        }
        private static string Categorize(double zscore)
        {
            string res="";
            if(zscore>=-2)
            res= "Normal";
            else if(zscore>=-3 && zscore<-2)
            res= "MAM";
            else if(zscore<-3)
            res= "SAM";
            return res;
        }
    }
}