using System.ComponentModel.DataAnnotations;

namespace DataSummitOAuthServer.Models
{
    public class TwoFactorAuthCodeModel
    {
        [Required]
        public string TwoFactorAuthCode { get; set; }
        [Required]
        public string AccountSecretKey { get; set; }
    }
}