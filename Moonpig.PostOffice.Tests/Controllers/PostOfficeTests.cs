namespace Moonpig.PostOffice.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Moonpig.PostOffice.Api.Interfaces;
    using Moonpig.PostOffice.Api.Model;
    using Moq;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        private readonly DespatchDateController _sut;
        private readonly Mock<IDespatchService> _mockDespatchService;

        public PostOfficeTests()
        {
            _mockDespatchService = new Mock<IDespatchService>();
            _sut = new DespatchDateController(_mockDespatchService.Object);
        }

        [Fact]
        public void WithDespatchDateReturned()
        {
            var ids = new List<int>() { 1 };
            var date = DateTime.Now;

            _mockDespatchService
                .Setup(x => x.GetDespatchDates(ids, date))
                .Returns(new DespatchDate());

            var result = _sut.Get(ids, date);

            result.ShouldBeOfType<OkObjectResult>();
        }
    }
}