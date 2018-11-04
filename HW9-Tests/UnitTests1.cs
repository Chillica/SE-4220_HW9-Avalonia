using NUnit.Framework;
using System;
using Interfaces;
using Moq;
using FluentAssertions;
using System.Collections.Generic;
using HW9;

namespace HW9_Tests
{

    public class UnitTests1
    {
        public static string TestString = null;
        public static Mock<IDataService> dataSvcMock = new Mock<IDataService>();
        public static Mock<IBookSearchService> bookSvcMock = new Mock<IBookSearchService>();

        [Test]
        public void TestSimpleCommand()
        {
            var command = new SimpleCommand(() => TestString = "I executed my Command!");
            command.Execute(this);
            Assert.AreEqual("I executed my Command!", TestString);
        }

        [Test]
        public void TestReadingJsonFile()
        {
            List<Book> bogusList = new List<Book>();
            bogusList.Add(new Book("BogusTitle", "BogusAuthor"));

            
            dataSvcMock.Setup(m => m.GetBooksFromJSONAsync(It.IsAny<string>()))
                .ReturnsAsync(bogusList);

            dataSvcMock.Setup(m => m.FileExists(It.IsAny<String>())).Returns(true);

            MainViewModel vm = new MainViewModel(dataSvcMock.Object, bookSvcMock.Object)
            {
                JsonPath = "totally random"
            };
            Assert.IsTrue(vm.LoadBookJSON.CanExecute(this));

            vm.LoadBookJSON.Execute(this);
            Assert.AreEqual(vm.Output, $"We found {vm.Books.Count} people in {vm.JsonPath}!");
        }

        [Test]
        public void CanLoadAfterFindingValidFile()
        {
            dataSvcMock.Setup(m => m.FindFileAsync()).ReturnsAsync("/fake/file");
            dataSvcMock.Setup(m => m.FileExists(It.IsAny<String>())).Returns(true);
            var vm = new MainViewModel(dataSvcMock.Object, bookSvcMock.Object);

            vm.FindFile.Execute(this);

            Assert.IsTrue(vm.LoadBookJSON.CanExecute(this));
        }

        //[Test]
        //[TestCase("Dragon Rider", "Dragon Rider (Dragon Rider, #1)")]
        //[TestCase(null, "Not able to get info.")]
        //public void TestBookSearchService(string bookTitle, string expectedTitle)
        //{

        //    SearchResult result = new SearchResult
        //    {
        //        Title = expectedTitle
        //    };
        //    bookSvcMock.Setup(m => m.SearchByTitle(bookTitle)).Returns(result);


        //    var vm = new MainViewModel(dataSvcMock.Object, bookSvcMock.Object)
        //    {
        //        UserSearch = bookTitle
        //    };
        //    vm.GetGoodreads.Execute(this);

        //    vm.SearchResult.Should().Be(expectedTitle);
        //}
    }
}
