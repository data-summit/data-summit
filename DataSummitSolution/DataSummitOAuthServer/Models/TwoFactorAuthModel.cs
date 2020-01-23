using System.ComponentModel.DataAnnotations;

namespace DataSummitOAuthServer.Models
{
    public class TwoFactorAuthModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Secret { get; set; }
    }
}