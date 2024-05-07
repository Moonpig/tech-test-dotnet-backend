namespace Moonpig.PostOffice.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers;
    using AutoFixture;
    using Microsoft.AspNetCore.Mvc;
    using Moonpig.PostOffice.Api.Interfaces;
    using Moonpig.PostOffice.Api.Model;
    using Moq;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        private readonly DespatchDateController _sut;
        private readonly Fixture _fixture;
        private readonly Mock<IDespatchService> _mockDespatchService;

        public PostOfficeTests()
        {
            _mockDespatchService = new Mock<IDespatchService>();
            _fixture = new Fixture();

            _sut = new(_mockDespatchService.Object);
        }

        [Fact]
        public void GivenValidParameters_ShouldReturn_OK()
        {
            // Arrange
            var ids = new List<int>() { 1 };
            var date = _fixture.Create<DateTime>();

            _mockDespatchService
                .Setup(x => x.GetDespatchDates(ids, date))
                .Returns(new DespatchDate());

            // Act
            var result = _sut.Get(ids, date);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }

        [Fact]
        public void GivenInvalidDate_ShouldReturn_BadRequest()
        {
            // Arrange
            var ids = new List<int>() { 1 };
            var date = DateTime.MinValue;

            // Act
            var result = _sut.Get(ids, date);

            // Assert
            result.ShouldBeOfType<BadRequestResult>();
        }

        [Fact]
        public void GivenInvalidIds_ShouldReturn_BadRequest()
        {
            // Arrange
            var ids = new List<int>() { };
            var date = _fixture.Create<DateTime>();

            // Act
            var result = _sut.Get(ids, date);

            // Assert
            result.ShouldBeOfType<BadRequestResult>();
        }
    }
}