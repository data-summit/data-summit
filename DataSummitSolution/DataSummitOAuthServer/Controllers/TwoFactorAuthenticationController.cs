using Google.Authenticator;
using DataSummitOAuthServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DataSummitOAuthServer.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TwoFactorAuthenticationController : ControllerBase
    {
        private readonly TwoFactorAuthenticator _twoFactorAuthenticator;
        private const string _issuer = "DataSummit";
        private const int _qrCodeSizePixels = 300;

        public TwoFactorAuthenticationController()
        {
            _twoFactorAuthenticator = new TwoFactorAuthenticator();
        }

        // Post api/twofactorauthentication/getcodes
        [HttpPost]
        [Route("GetCodes")]
        public string[] GetCodes([FromBody]TwoFactorAuthModel twoFactorAuthModel)
        {
            var setupCode = GetSetupCode(twoFactorAuthModel.Username, twoFactorAuthModel.Secret);

            var qrCodeSetupImageUrl = setupCode.QrCodeSetupImageUrl;
            var manualEntryKey = setupCode.ManualEntryKey;

            return new string[] { manualEntryKey, qrCodeSetupImageUrl };
        }

        // Post api/twofactorauthentication/validate
        [HttpPost]
        [Route("Validate")]
        public bool Validate([FromBody]TwoFactorAuthCodeModel twoFactorAuthCode)
        {
            return IsUserPinValid(twoFactorAuthCode.AccountSecretKey, twoFactorAuthCode.TwoFactorAuthCode);
        }

        private SetupCode GetSetupCode(string userAccount, string accountSecretKey)
        {
            return _twoFactorAuthenticator.GenerateSetupCode(_issuer, userAccount, Encoding.ASCII.GetBytes(accountSecretKey), _qrCodeSizePixels);
        }

        private bool IsUserPinValid(string accountSecretKey, string userTwoFactorCode)
        {
            return _twoFactorAuthenticator.ValidateTwoFactorPIN(accountSecretKey, userTwoFactorCode);
        }
    }
}