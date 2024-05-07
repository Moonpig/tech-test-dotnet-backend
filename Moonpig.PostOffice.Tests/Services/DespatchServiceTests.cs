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
        public void OneProductWithLeadTimeOfOneDay_DateTimeNow()
        {
            SetupProduct(productId: 1, leadTime: 1);

            var result = _sut.GetDespatchDates(new List<int>() { 1 }, DateTime.Now);

            result.Date.ShouldBe(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        }

        [Theory, MemberData(nameof(OneDayLeadTimeOrders))]
        public void OneProductWithLeadTimeOfOneDays(DateTime orderDate, DateTime expectedDate, string message)
        {
            SetupProduct(productId: 1, leadTime: 1);

            var result = _sut.GetDespatchDates(new List<int>() { 1 }, orderDate);

            result.Date.ShouldBe(DateOnly.FromDateTime(expectedDate), message);
        }

        [Theory, MemberData(nameof(TwoDayLeadTimeOrders))]
        public void OneProductWithLeadTimeOfTwoDays(DateTime orderDate, DateTime expectedDate, string message)
        {
            SetupProduct(productId: 1, leadTime: 2);

            var result = _sut.GetDespatchDates(new List<int>() { 1 }, orderDate);

            result.Date.ShouldBe(DateOnly.FromDateTime(expectedDate), message);
        }

        [Fact(DisplayName = "Case 1")]
        public void OneProductWithLeadTimeOfOneDay_WithDate()
        {
            SetupProduct(productId: 1, leadTime: 1);

            var result = _sut.GetDespatchDates(new List<int>() { 1 }, new DateTime(2018, 01, 01));

            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 02)));
        }

        [Fact(DisplayName = "Case 4")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnFridayShouldArriveMonday()
        {
            SetupProduct(productId: 1, leadTime: 1);

            var result = _sut.GetDespatchDates(new List<int>() { 1, }, new DateTime(2018, 01, 5));

            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 8)));
        }

        [Fact(DisplayName = "Case 5")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnSaturdayShouldArriveTuesday()
        {
            SetupProduct(productId: 1, leadTime: 1);

            var date = _sut.GetDespatchDates(new List<int>() { 1, }, new DateTime(2018, 01, 6));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 09)));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            SetupProduct(productId: 1, leadTime: 2);

            var actualDate = _sut.GetDespatchDates(new List<int>() { 1 }, DateTime.Now).Date;
            var expectedDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(2));

            actualDate.ShouldBe(expectedDate);
        }

        [Fact(DisplayName = "Case 2")]
        public void OneProductWithLeadTimeOfTwoDay_WithDate()
        {
            SetupProduct(productId: 1, leadTime: 2);

            var date = _sut.GetDespatchDates(new List<int>() { 1 }, new DateTime(2018, 01, 01));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 03)));
        }

        [Fact(DisplayName = "Case 3")]
        public void OneProductWithLeadTimeOfTwoDay_WithMultipleSuppliers()
        {
            SetupProduct(productId: 1, leadTime: 1);
            SetupProduct(productId: 2, leadTime: 2);

            var date = _sut.GetDespatchDates(new List<int>() { 1, 2 }, new DateTime(2018, 01, 01));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 03)));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            SetupProduct(productId: 1, leadTime: 3);

            var date = _sut.GetDespatchDates(new List<int>() { 1 }, DateTime.Now);

            date.Date.ShouldBe(DateOnly.FromDateTime(DateTime.Now.AddDays(3)));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            SetupProduct(productId: 1, leadTime: 1);


            var date = _sut.GetDespatchDates(new List<int>() { 1 }, new DateTime(2018, 1, 26));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 1, 26).Date.AddDays(3)));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            //var product = new Product { ProductId = 3, Name = "Soft Toy", SupplierId = 4 };
            //var supplier = new Supplier { SupplierId = 4, Name = "TailSpin Toys", LeadTime = 3 };

            //_mockDataProvider
            //    .Setup(x => x.GetProduct(3))
            //    .Returns(product);

            //_mockDataProvider
            //    .Setup(x => x.GetSupplier(4))
            //    .Returns(supplier);

            SetupProduct(productId: 1, leadTime: 3);

            var date = _sut.GetDespatchDates(new List<int>() { 1 }, new DateTime(2018, 1, 25));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 1, 25).Date.AddDays(4)));
        }

        [Fact(DisplayName = "Case 6")]
        public void OneProductWithLeadTimeOf1Day_WhenOrderedOnSundayShouldArriveTuesday()
        {
            SetupProduct(productId: 1, leadTime: 1);

            var date = _sut.GetDespatchDates(new List<int>() { 1, }, new DateTime(2018, 01, 7));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 09)));
        }

        [Fact(DisplayName = "Case 7")]
        public void OneProductWithLeadTimeOf6Days_WhenOrderedOnFridayShouldArriveMonday()
        {
            SetupProduct(productId: 1, leadTime: 6);

            var date = _sut.GetDespatchDates(new List<int>() { 1, }, new DateTime(2018, 01, 5));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 15)));
        }

        [Fact(DisplayName = "Case 8")]
        public void OneProductWithLeadTimeOf11Days_WhenOrderedOnFridayShouldArriveMonday()
        {
            SetupProduct(productId: 1, leadTime: 11);

            var date = _sut.GetDespatchDates(new List<int>() { 1, }, new DateTime(2018, 01, 5));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 22)));
        }

        private void SetupProduct(int productId, int leadTime)
        {
            var product = new Product { ProductId = productId, Name = "Greetings Card", SupplierId = productId };
            var supplier = new Supplier { SupplierId = productId, Name = "Acme Corporation", LeadTime = leadTime };

            _mockDataProvider
                .SetupSequence(x => x.GetProduct(productId))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(productId))
                .Returns(supplier);
        }

        public static TheoryData<DateTime, DateTime, string> OneDayLeadTimeOrders = new()
        {
            { new DateTime(2018, 1, 1), new DateTime(2018, 1, 2), "Order on Monday, arrive on Tuesday" },
            { new DateTime(2018, 1, 2), new DateTime(2018, 1, 3), "Order on Tuesday, arrive on Wednesday" },
            { new DateTime(2018, 1, 3), new DateTime(2018, 1, 4), "Order on Wednesday, arrive on Thursday" },
            { new DateTime(2018, 1, 4), new DateTime(2018, 1, 5), "Order on Thursday, arrive on Friday" },
            { new DateTime(2018, 1, 5), new DateTime(2018, 1, 8), "Order on Friday, arrive on Monday" },
            { new DateTime(2018, 1, 6), new DateTime(2018, 1, 9), "Order on Saturday, arrive on Tuesday" },
            { new DateTime(2018, 1, 7), new DateTime(2018, 1, 9), "Order on Sunday, arrive on Tuesday" },
        };

        public static TheoryData<DateTime, DateTime, string> TwoDayLeadTimeOrders = new()
        {
            { new DateTime(2018, 1, 1), new DateTime(2018, 1, 3), "Order on Monday, arrive on Wednesday" },
            { new DateTime(2018, 1, 2), new DateTime(2018, 1, 4), "Order on Tuesday, arrive on Thursday" },
            { new DateTime(2018, 1, 3), new DateTime(2018, 1, 5), "Order on Wednesday, arrive on Friday" },
            { new DateTime(2018, 1, 4), new DateTime(2018, 1, 8), "Order on Thursday, arrive on Monday" },
            { new DateTime(2018, 1, 5), new DateTime(2018, 1, 9), "Order on Friday, arrive on Tuesday" },
            { new DateTime(2018, 1, 6), new DateTime(2018, 1, 10), "Order on Saturday, arrive on Wednesday" },
            { new DateTime(2018, 1, 7), new DateTime(2018, 1, 10), "Order on Sunday, arrive on Wednesday" },
        };
    }
}