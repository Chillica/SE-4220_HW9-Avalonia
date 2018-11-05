using Goodreads;
using Goodreads.Models.Response;
using Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Task<SearchResult> SearchByTitle(string querySearch)
        {
            return Task.Run(() =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                .AddUserSecrets("HW9Secrets");
                var config = configurationBuilder.Build();
                string goodreadsKey = config["goodreadsKey"];
                string goodreadsSecret = config["goodreadsSecret"];

                var client = GoodreadsClient.Create(goodreadsKey, goodreadsSecret);

                SearchResult result = new SearchResult();

                var val = client.Books.GetByTitle(querySearch).Result;
                if (val != null)
                {
                    result.RelatedBooks = new ObservableCollection<SearchResult>();
                    foreach (var p in val.Authors)
                    {
                        result.Author += p.Name + ", ";
                    }
                    result.Title = val.Title;
                    result.Image = val.SmallImageUrl;
                    if (val.SimilarBooks != null)
                    {
                        foreach (var b in val.SimilarBooks)
                        {
                            SearchResult otherBook = new SearchResult();
                            foreach (var o in b.Authors)
                            {
                                otherBook.Author += o.Name + ", ";
                            }
                            otherBook.Title = b.Title;
                            otherBook.Image = b.SmallImageUrl;
                            result.RelatedBooks.Add(otherBook);
                        }
                    }
                    else
                    {
                        SearchResult otherBook = new SearchResult
                        {
                            Title = "Not Available",
                            Image = "Not Available",
                            Author = "Not Available"
                        };
                        result.RelatedBooks.Add(otherBook);
                    }
                }
                else
                {
                    result.RelatedBooks = new ObservableCollection<SearchResult>();
                    result.Title = "Not able to get info";
                    result.Author = "Not Able to get info";
                    result.Image = "Not Able to get info";
                }

                return result;
            });
        }

    }
}
