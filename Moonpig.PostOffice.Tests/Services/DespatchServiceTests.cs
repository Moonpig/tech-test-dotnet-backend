using System;
using Moonpig.PostOffice.Api.Services;
using Moonpig.PostOffice.Api.Tests.Services;
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
            _mockDataProvider = new(MockBehavior.Strict);
            _sut = new(_mockDataProvider.Object);
        }

        [Fact(DisplayName = "Case 1")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnMondayShouldArriveTuesday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 01));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 02)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Tuesday);
        }

        [Fact(DisplayName = "Case 2")]
        public void OneProductWithLeadTimeOfTwoDays_WhenOrderedOnMondayShouldArriveWednesday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 2);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 01));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 03)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Wednesday);
        }

        [Fact(DisplayName = "Case 3")]
        public void TwoProducts_WithMultipLeadTimes_WhenOrderedOnMondayShouldArriveWednesday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);
            SetupProduct(id: 2, leadTime: 2);

            // Act
            var result = _sut.GetDespatchDates([1, 2], new DateTime(2018, 1, 01));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 1, 03)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Wednesday);
        }

        [Fact(DisplayName = "Case 4")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnFridayShouldArriveMonday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 5));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 8)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Monday);
        }

        [Fact(DisplayName = "Case 5")]
        public void OneProductWithLeadTimeOfOneDay_WhenOrderedOnSaturdayShouldArriveTuesday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 6));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 9)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Tuesday);
        }

        [Fact(DisplayName = "Case 6")]
        public void OneProductWithLeadTimeOf1Day_WhenOrderedOnSundayShouldArriveTuesday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 7));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 9)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Tuesday);
        }

        [Fact(DisplayName = "Case 7")]
        public void OneProductWithLeadTimeOf6Days_WhenOrderedOnFridayShouldArriveMonday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 6);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 5));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 15)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Monday);
        }

        [Fact(DisplayName = "Case 8")]
        public void OneProductWithLeadTimeOf11Days_WhenOrderedOnFridayShouldArriveMonday()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 11);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 01, 5));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 01, 22)));
            result.Date.DayOfWeek.ShouldBe(DayOfWeek.Monday);
        }

        [Fact]
        public void OneProductWithLeadTimeOfZeroDays()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 0);

            // Act
            var act = () => _sut.GetDespatchDates([1], DateTime.Now);

            // Assert
            act.ShouldThrow<NotSupportedException>();
        }

        [Theory, MemberData(nameof(TestCases.OneDayLeadTimeOrders), MemberType = typeof(TestCases))]
        public void OneProductWithLeadTimeOfOneDay_MultipleCases(DateTime orderDate, DateTime expectedDate, string message)
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);

            // Act
            var result = _sut.GetDespatchDates([1], orderDate);

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(expectedDate), message);
        }

        [Theory, MemberData(nameof(TestCases.TwoDayLeadTimeOrders), MemberType = typeof(TestCases))]
        public void OneProductWithLeadTimeOfTwoDays_MultipleCases(DateTime orderDate, DateTime expectedDate, string message)
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 2);

            // Act
            var result = _sut.GetDespatchDates([1], orderDate);

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(expectedDate), message);
        }

        [Theory, MemberData(nameof(TestCases.VaryingLeadTimeOrders), MemberType = typeof(TestCases))]
        public void OneProductWithVaryingLeadTimes_MultipleCases(DateOnly orderDate, DateOnly expectedDate, int leadTime)
        {
            // Arrange
            SetupProduct(id: 1, leadTime: leadTime);

            // Act
            var result = _sut.GetDespatchDates([1], orderDate.ToDateTime(new TimeOnly()));

            // Assert
            result.Date.ShouldBe(expectedDate, $"{leadTime} day lead time.");
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 1);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 1, 26));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 1, 29)));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            // Arrange
            SetupProduct(id: 1, leadTime: 3);

            // Act
            var result = _sut.GetDespatchDates([1], new DateTime(2018, 1, 25));

            // Assert
            result.Date.ShouldBe(DateOnly.FromDateTime(new DateTime(2018, 1, 30)));
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
    }
}