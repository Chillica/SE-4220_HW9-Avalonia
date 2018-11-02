using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public class Book
    {
        public Book(string title, string author, string pages = null, string chapters = null, string words = null)
        {
            Title = title;
            Author = author;
            Pages = pages;
            Chapters = chapters;
            Words = words;
        }

        public string Title { get; }
        public string Author { get; }
        public string Pages { get; }
        public string Chapters { get; }
        public string Words { get; }
    }
}
