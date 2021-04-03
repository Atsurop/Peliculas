using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace back_end.Utilities
{
    public interface IStoreFiles
    {
        Task DeleteFile(string path, string container);
        Task<string> SaveFile(string container, IFormFile file);
        Task<string> UpdateFile(string container, IFormFile file, string path);
    }
}