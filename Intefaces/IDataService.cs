using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IDataService
    {
        Task<List<Book>> GetBooksFromJSONAsync(string jsonFile);
        Task<string> FindFileAsync();
        bool FileExists(string jsonFile);
    }
}
