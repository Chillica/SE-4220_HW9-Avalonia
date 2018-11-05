using Goodreads.Models.Response;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IBookSearchService
    {
        //Task<PaginatedList<Work>> Search(string querySearch);
        Task<SearchResult> SearchByTitle(string querySearch);
    }

    public class SearchResult
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
        public ObservableCollection<SearchResult> RelatedBooks { get; set; }
    }

    //public class ReviewResult
    //{
    //    public string Reviewer { get; set; }
    //    public string Review { get; set; }
    //    public string Rating { get; set; }
    //}
}
