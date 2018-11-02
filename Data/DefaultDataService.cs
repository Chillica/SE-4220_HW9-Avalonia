using Avalonia.Controls;
using Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Data
{
    public class DefaultDataService : IDataService
    {
        public bool FileExists(string jsonFile)
        {
            return File.Exists(jsonFile);
        }

        public async Task<string> FindFileAsync()
        {
            var openFileDialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Title = "What json file do you want to use?"
            };

            var pathArray = await openFileDialog.ShowAsync();
            if ((pathArray?.Length ?? 0) > 0)
                return pathArray[0];
            return null;
        }

        public Task<List<Book>> GetBooksFromJSONAsync(string jsonFile)
        {
            return Task.Run(() =>
            {
                List<Book> bookList = new List<Book>();

                var res = JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(jsonFile));

                return res;
            });
        }
    }
}
