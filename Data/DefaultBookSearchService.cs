using Goodreads;
using Goodreads.Models.Response;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DefaultBookSearchService : IBookSearchService
    {
        //public Task<PaginatedList<Work>> Search(string querySearch)
        //{
        //    var client = GoodreadsClient.Create(apiKey, apiSecret);
        //    return client.Books.Search(querySearch);
        //}

        public Task<Goodreads.Models.Response.Book> SearchByTitle(string querySearch)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddUserSecrets("HW9Secrets");
            var config = configurationBuilder.Build();
            string goodreadsKey = config["goodreadsKey"];
            string goodreadsSecret = config["goodreadsSecret"];

            var client = GoodreadsClient.Create(goodreadsKey, goodreadsSecret);
            return client.Books.GetByTitle(querySearch);
        }

    }
}
