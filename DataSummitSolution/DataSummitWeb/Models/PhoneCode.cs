namespace DataSummitWeb.Models
{
    public class PhoneCode
    {
        public int CountryId { get; set; }
        public string ISO { get; set; }
        public string Name { get; set; }
        public string SentenceCaseName { get; set; }
        public string ISO3 { get; set; }
        public string Numcode { get; set; }
        // Why have you got numbers as strings?!
        public string Phonecode { get; set; }

        public PhoneCode()
        {

        }
    }
}
