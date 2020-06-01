using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSummitHelper.Services
{
    public static class ServicePrinciple
    {
        public static string ClientId { get; set; }
        public static string ServicePrincipalPassword { get; set; }
        public static string AzureTenantId { get; set; }
        public static string AzureSubscriptionId { get; set; }

        public static string AuthToken { get; set; }
        public static TokenCredentials TokenCredentials { get; set; }
        public static string ResourceGroupName { get; set; }

    }
}
