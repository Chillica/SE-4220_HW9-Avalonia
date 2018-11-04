using Data;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace HW9
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IDataService data;
        private readonly IBookSearchService bookSearch;
        public ObservableCollection<Book> Books { get; private set; }
        public ObservableCollection<SearchResult> RelaventBooks { get; private set; }

        public MainViewModel() : this(new DefaultDataService(), new DefaultBookSearchService())
        { }

        public MainViewModel(IDataService data, IBookSearchService bookSearch)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            this.bookSearch = bookSearch ?? throw new ArgumentNullException(nameof(bookSearch));
            Books = new ObservableCollection<Book>();
            
        }

        private string jsonPath;
        public string JsonPath
        {
            get => jsonPath;
            set
            {
                jsonPath = value;
                OnPropertyChanged(nameof(JsonPath));
                LoadBookJSON.RaiseCanExecuteChanged();
            }
        }

        private Book selectedBook;
        public Book SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                OnPropertyChanged(nameof(SelectedBook));
            }
        }

        private string output;
        public string Output
        {
            get => output;
            set
            {
                output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                LoadBookJSON.RaiseCanExecuteChanged();
                FindFile.RaiseCanExecuteChanged();
            }
        }

        private string userSearch;
        public string UserSearch
        {
            get => userSearch;
            set
            {
                userSearch = value;
                OnPropertyChanged(nameof(UserSearch));
            }
        }

        private SearchResult searchResult;
        public SearchResult SearchResult
        {
            get => searchResult;
            set
            {
                searchResult = value;
                OnPropertyChanged(nameof(SearchResult));
                OnPropertyChanged(nameof(RelaventBooks));
            }
        }

        private SimpleCommand getGoodreads;
        public SimpleCommand GetGoodreads => getGoodreads ?? (getGoodreads = new SimpleCommand(async () =>
        {
            SearchResult result = new SearchResult();

            var res = bookSearch.SearchByTitle(UserSearch);
            var val = res.Result;
            if (val != null)
            {
                RelaventBooks = new ObservableCollection<SearchResult>();
                foreach (var p in val.Authors)
                {
                    result.Author += p.Name + ", ";
                }
                result.Title = val.Title;
                result.Image = val.SmallImageUrl;
                foreach (var b in val.SimilarBooks)
                {
                    SearchResult otherBook = new SearchResult();
                    foreach (var o in b.Authors)
                    {
                        otherBook.Author += o.Name + ", ";
                    }
                    otherBook.Title = b.Title;
                    otherBook.Image = b.SmallImageUrl;
                    RelaventBooks.Add(otherBook);
                }
            }
            else
            {
                RelaventBooks = new ObservableCollection<SearchResult>();
                result.Title = "Not able to get info";
                result.Author = "Not Able to get info";
                result.Image = "Not Able to get info";
            }
            SearchResult = result;
        }));


        private SimpleCommand loadBookJSON;
        public SimpleCommand LoadBookJSON => loadBookJSON ?? (loadBookJSON = new SimpleCommand(
        () => !IsBusy && data.FileExists(JsonPath), //can execute
        async () => //execute
        {
            Output = "Loading...";
            IsBusy = true;
            foreach (var p in await data.GetBooksFromJSONAsync(JsonPath))
                Books.Add(p);
            Output = $"We found {Books.Count} people in {JsonPath}!";
            IsBusy = false;
        }));

        private SimpleCommand findFile;
        public SimpleCommand FindFile => findFile ?? (findFile = new SimpleCommand(
            () => !IsBusy,
            async () =>
            {
                JsonPath = await data.FindFileAsync();
                LoadBookJSON.RaiseCanExecuteChanged();
            }));

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
