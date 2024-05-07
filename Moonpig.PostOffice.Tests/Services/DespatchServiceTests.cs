using System;
using System.Collections.Generic;
using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Data.Entities;
using Moonpig.PostOffice.Data.Interfaces;
using Moq;
using Shouldly;
using Xunit;

namespace Moonpig.PostOffice.Tests.Services
{
    public class DespatchServiceTests
    {
        private readonly DespatchService _sut;
        private readonly Mock<IDataProvider> _mockDataProvider;

        public DespatchServiceTests()
        {
            _mockDataProvider = new Mock<IDataProvider>();
            _sut = new DespatchService(_mockDataProvider.Object);
        }

        [Fact]
        public void OneProductWithLeadTimeOfOneDay_DateTimeNow()
        {
            SetupProduct(id: 1, leadTime: 1);

            var result = _sut.GetDespatchDates([1], DateTime.Now);

            result.Date.ShouldBe(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        }

        [Fact]
        public void OneProductWithLeadTimeOfZeroDays_DateTimeNow()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 0);

            // Act
            var act = () => _sut.GetDespatchDates([1], DateTime.Now);

            // Assert
            act.ShouldThrow<NotSupportedException>();
        }

        [Theory, MemberData(nameof(OneDayLeadTimeOrders))]
        public void OneProductWithLeadTimeOfOneDays(DateTime orderDate, DateTime expectedDate, string message)
        {
            SetupProduct(id: 1, leadTime: 1);

            var result = _sut.GetDespatchDates([1], orderDate);

            result.Date.ShouldBe(DateOnly.FromDateTime(expectedDate), message);
        }

        [Theory, MemberData(nameof(TwoDayLeadTimeOrders))]
        public void OneProductWithLeadTimeOfTwoDays(DateTime orderDate, DateTime expectedDate, string message)
        {
            SetupProduct(id: 1, leadTime: 2);

            var result = _sut.GetDespatchDates([1], orderDate);

            result.Date.ShouldBe(DateOnly.FromDateTime(expectedDate), message);
        }

        [Theory, MemberData(nameof(LeadTimeOrders))]
        public void OneProductWithVaryingLeadTimes(DateOnly orderDate, DateOnly expectedDate, int leadTime, string message)
        {
            SetupProduct(id: 1, leadTime: leadTime);

            var result = _sut.GetDespatchDates([1], orderDate.ToDateTime(new TimeOnly()));

            result.Date.ShouldBe(expectedDate, $"{message}, {leadTime} lead time.");
        }

        [Fact(DisplayName = "Case 1")]
        public void OneProductWithLeadTimeOfOneDay_WithDate()
        {
            SetupProduct(id: 1, leadTime: 1);

            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 01));

            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 02)));
        }

        [Fact(DisplayName = "Case 4")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnFridayShouldArriveMonday()
        {
            SetupProduct(id: 1, leadTime: 1);

            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 5));

            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 8)));
        }

        [Fact(DisplayName = "Case 5")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnSaturdayShouldArriveTuesday()
        {
            SetupProduct(id: 1, leadTime: 1);

            var date = _sut.GetDespatchDates([1], new DateTime(2018, 01, 6));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 09)));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            SetupProduct(id: 1, leadTime: 2);

            var actualDate = _sut.GetDespatchDates([1], DateTime.Now).Date;
            var expectedDate = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(2));

            actualDate.ShouldBe(expectedDate);
        }

        [Fact(DisplayName = "Case 2")]
        public void OneProductWithLeadTimeOfTwoDay_WithDate()
        {
            SetupProduct(id: 1, leadTime: 2);

            var date = _sut.GetDespatchDates([1], new DateTime(2018, 01, 01));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 03)));
        }

        [Fact(DisplayName = "Case 3")]
        public void OneProductWithLeadTimeOfTwoDay_WithMultipleSuppliers()
        {
            SetupProduct(id: 1, leadTime: 1);
            SetupProduct(id: 2, leadTime: 2);

            var date = _sut.GetDespatchDates([1, 2], new DateTime(2018, 01, 01));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 03)));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            SetupProduct(id: 1, leadTime: 3);

            var date = _sut.GetDespatchDates([1], DateTime.Now);

            date.Date.ShouldBe(DateOnly.FromDateTime(DateTime.Now.AddDays(3)));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            SetupProduct(id: 1, leadTime: 1);


            var date = _sut.GetDespatchDates([1], new DateTime(2018, 1, 26));

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

            SetupProduct(id: 1, leadTime: 3);

            var date = _sut.GetDespatchDates([1], new DateTime(2018, 1, 25));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 1, 25).Date.AddDays(4)));
        }

        [Fact(DisplayName = "Case 6")]
        public void OneProductWithLeadTimeOf1Day_WhenOrderedOnSundayShouldArriveTuesday()
        {
            SetupProduct(id: 1, leadTime: 1);

            var date = _sut.GetDespatchDates([1], new DateTime(2018, 01, 7));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 09)));
        }

        [Fact(DisplayName = "Case 7")]
        public void OneProductWithLeadTimeOf6Days_WhenOrderedOnFridayShouldArriveMonday()
        {
            SetupProduct(id: 1, leadTime: 6);

            var date = _sut.GetDespatchDates([1], new DateTime(2018, 01, 5));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 15)));
        }

        [Fact(DisplayName = "Case 8")]
        public void OneProductWithLeadTimeOf11Days_WhenOrderedOnFridayShouldArriveMonday()
        {
            SetupProduct(id: 1, leadTime: 11);

            var date = _sut.GetDespatchDates([1], new DateTime(2018, 01, 5));

            date.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 22)));
        }

        private void SetupProduct(int id, int leadTime)
        {
            var product = new Product { ProductId = id, Name = "Greetings Card", SupplierId = id };
            var supplier = new Supplier { SupplierId = id, Name = "Acme Corporation", LeadTime = leadTime };

            _mockDataProvider
                .Setup(x => x.GetProduct(id))
                .Returns(product);

            _mockDataProvider
                .Setup(x => x.GetSupplier(id))
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

        public static TheoryData<DateOnly, DateOnly, int, string> LeadTimeOrders = new()
        {
            // One day lead time
            { new (2018, 1, 1), new (2018, 1, 2), 1, "Order on Monday, arrive on Tuesday" },
            { new (2018, 1, 2), new (2018, 1, 3), 1, "Order on Tuesday, arrive on Wednesday" },
            { new (2018, 1, 3), new (2018, 1, 4), 1, "Order on Wednesday, arrive on Thursday" },
            { new (2018, 1, 4), new (2018, 1, 5), 1, "Order on Thursday, arrive on Friday" },
            { new (2018, 1, 5), new (2018, 1, 8), 1, "Order on Friday, arrive on Monday" },
            { new (2018, 1, 6), new (2018, 1, 9), 1, "Order on Saturday, arrive on Tuesday" },
            { new (2018, 1, 7), new (2018, 1, 9), 1, "Order on Sunday, arrive on Tuesday" },

            // Two day lead time
            { new (2018, 1, 1), new (2018, 1, 3), 2, "Order on Monday, arrive on Wednesday" },
            { new (2018, 1, 2), new (2018, 1, 4), 2, "Order on Tuesday, arrive on Thursday" },
            { new (2018, 1, 3), new (2018, 1, 5), 2, "Order on Wednesday, arrive on Friday" },
            { new (2018, 1, 4), new (2018, 1, 8), 2, "Order on Thursday, arrive on Monday" },
            { new (2018, 1, 5), new (2018, 1, 9), 2, "Order on Friday, arrive on Tuesday" },
            { new (2018, 1, 6), new (2018, 1, 10), 2, "Order on Saturday, arrive on Wednesday" },
            { new (2018, 1, 7), new (2018, 1, 10), 2, "Order on Sunday, arrive on Wednesday" },

            // Varying lead times
            { new (2018, 1, 1), new (2018, 1, 17), 12, "Order on Monday, arrive on Wednesday" },
            { new (2018, 1, 2), new (2018, 1, 30), 20, "Order on Tuesday, arrive on Thursday" },
            { new (2018, 1, 13), new (2018, 1, 22), 7, "Order on Thursday, arrive on Monday" },
            { new (2018, 1, 5), new (2018, 1, 25), 14, "Order on Friday, arrive on Tuesday" },
            { new (2018, 1, 10), new (2018, 2, 14), 25, "Order on Saturday, arrive on Wednesday" },
        };
    }
}