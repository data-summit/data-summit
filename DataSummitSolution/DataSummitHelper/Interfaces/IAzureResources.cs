using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IAzureResources
    {
        //Company
        Task<string> CreateResourceGroup(string name);
        Task<string> UpdateResourceGroup(string oldName, string newName);
        Task<bool> DeleteResourceGroup(string name);
        //Project
        Task<string> CreateStorageAccount(string resourceGroup, string name);
        Task<string> UpdateStorageAccount(string resourceGroup, string oldName, string newName);
        Task<bool> DeleteStorageAccount(string resourceGroup, string name);
    }
}
