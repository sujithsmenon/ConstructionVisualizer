using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Services
{
    public interface IMobileStorageService
    {
        Task SaveUserDataAsync(string key, string data);
        Task<string> GetUserDataAsync(string key);
        Task DeleteUserDataAsync(string key);
        Task<List<string>> GetSavedCustomizationsAsync();
        Task SaveImageToGalleryAsync(byte[] imageData, string filename);
    }
}
