using System;

namespace TemplateMapper.Keys
{
    public static class Azure
    {
        public const String BlobStorageName = "scannedpdfs";
        //public const String BlobStorageKey = "https://scannedpdfs.blob.core.windows.net/00000015";
        public const String BlobStorageKey = "exQVhIE2OwLwjcOEqAP8gf3NLyHMHrAY7+hUn4HfOaXoTb5ylqNVU6tYw8IW+SpYihMKpMknoShu/wVPn8Rcuw==";
        public const String SQLConnection = @"Server=tcp:abte94o0wd.database.windows.net,1433;Initial Catalog=BSDetector;Persist Security Info=False;User ID=lightosDB;Password=!Aa1234567;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public const String VisionKey = "3d51318e13a74e6f8b688610c60a551d";
        public const String VisionUri = "https://northeurope.api.cognitive.microsoft.com/vision/v2.0/ocr";

        public const String ADALServiceURL = "https://login.microsoftonline.com";     // Service root URL for Azure AD instance WITH NO TRAILING SLASH! -->
        public const String ADALRedirectURL = "http://localhost/billingapi";          // Redirect URL for Azure AD to use which MUST MATCH YOUR AAD APP CONFIGURATION! -->
        public const String ARMBillingServiceURL = "https://management.azure.com";    // Service root URL for ARM/Billing service WITH NO TRAILING SLASH!  -->
        public const String TenantDomain = "turkeyboy100hotmail.onmicrosoft.com";     // DNS name for your Azure AD tenant, ie: contoso.onmicrosoft.com -->
        public const String TenantId = "8b7360be-12d9-466e-a5d7-edd92e4fa45f";
        public const String SubscriptionID = "257231cb-b5b5-48f5-8f4e-89c34247518d";  // Pay-as-you-go
        public const String SubscriptionIDDD = "8b7360be-12d9-466e-a5d7-edd92e4fa45f";// Default directory
        public const String ClientID = "11a3a828-5ff6-45df-b9ba-cc6c51193d76";
        public const String ClientSecret = "5fq5hP42Yrdco13lU+vFQ8xsUM4dI0tBj6Hz/kNsaUQ=";
    }
}
