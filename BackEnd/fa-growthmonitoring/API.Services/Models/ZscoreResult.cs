namespace API.Services.Models
{
    public class ZscoreResult
    {
        public string Wasting {get;set;}
        public string Underweight {get;set;}
        public string Stunting {get;set;}
        public double NWL{get;set;}
        public double NLA{get;set;}
        public double NWA{get;set;}
    }
}