﻿using Data;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace HW9
{
    class MainViewModel : INotifyPropertyChanged
    {

        public MainViewModel() : this(new DefaultDataService())
        { }

        public MainViewModel(IDataService data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
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

        private readonly IDataService data;
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

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Book> Books { get; private set; }

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
    }
}
