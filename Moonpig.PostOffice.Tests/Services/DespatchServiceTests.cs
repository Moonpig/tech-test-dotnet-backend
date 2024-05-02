using System;
using System.Collections.Generic;
using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Data;
using Moonpig.PostOffice.Data.Entities;
using Moq;
using Shouldly;
using Xunit;

namespace Moonpig.PostOffice.Tests.Services
{
    public class DespatchServiceTests
    {
        private readonly DespatchService _sut;
        private Mock<IDataProvider> _mockDataProvider;

        public DespatchServiceTests()
        {
            _mockDataProvider = new Mock<IDataProvider>();

            _sut = new DespatchService(_mockDataProvider.Object);
        }

        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            var product = new Product { ProductId = 1, Name = "Greetings Card", SupplierId = 1 };
            var supplier = new Supplier { SupplierId = 1, Name = "Acme Corporation", LeadTime = 1 };

            _mockDataProvider
                .Setup(x => x.GetProduct(1))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(1))
                .Returns(supplier);

            var date = _sut.GetDespatchDates(new List<int>() { 1 }, DateTime.Now);

            date.Date.ShouldBe(DateTime.Now.Date.AddDays(1));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            var product = new Product { ProductId = 2, Name = "Flowers", SupplierId = 2 };
            var supplier = new Supplier { SupplierId = 2, Name = "Sunnyside Flowers", LeadTime = 2 };

            _mockDataProvider
                .Setup(x => x.GetProduct(2))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(2))
                .Returns(supplier);

            var date = _sut.GetDespatchDates(new List<int>() { 2 }, DateTime.Now);

            date.Date.ShouldBe(DateTime.Now.Date.AddDays(2));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            var product = new Product { ProductId = 3, Name = "Soft Toy", SupplierId = 4 };
            var supplier = new Supplier { SupplierId = 4, Name = "TailSpin Toys", LeadTime = 3 };

            _mockDataProvider
                .Setup(x => x.GetProduct(3))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(4))
                .Returns(supplier);

            var date = _sut.GetDespatchDates(new List<int>() { 3 }, DateTime.Now);

            date.Date.ShouldBe(DateTime.Now.Date.AddDays(3));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            var product = new Product { ProductId = 1, Name = "Greetings Card", SupplierId = 1 };
            var supplier = new Supplier { SupplierId = 1, Name = "Acme Corporation", LeadTime = 1 };

            _mockDataProvider
                .Setup(x => x.GetProduct(1))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(1))
                .Returns(supplier);

            var date = _sut.GetDespatchDates(new List<int>() { 1 }, new DateTime(2018, 1, 26));

            date.Date.ShouldBe(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            var product = new Product { ProductId = 3, Name = "Soft Toy", SupplierId = 4 };
            var supplier = new Supplier { SupplierId = 4, Name = "TailSpin Toys", LeadTime = 3 };

            _mockDataProvider
                .Setup(x => x.GetProduct(3))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(4))
                .Returns(supplier);

            var date = _sut.GetDespatchDates(new List<int>() { 3 }, new DateTime(2018, 1, 25));

            date.Date.ShouldBe(new DateTime(2018, 1, 25).Date.AddDays(4));
        }
    }
}