namespace Moonpig.PostOffice.Tests
{
    using System;
    using System.Collections.Generic;
    using Moonpig.PostOffice.Api.Controllers;
    using Moonpig.PostOffice.Api.Model;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            DespatchDateController controller = new DespatchDateController();
            DespatchDate date = controller.Get(new List<int>() { 1 }, new DateTime(2021, 11, 01));
            date.Date.Date.ShouldBe(new DateTime(2021, 11, 01).Date.AddDays(1));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            DespatchDateController controller = new DespatchDateController();
            DespatchDate date = controller.Get(new List<int>() { 2 }, new DateTime(2021, 11, 01));
            date.Date.Date.ShouldBe(new DateTime(2021, 11, 01).Date.AddDays(2));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            DespatchDateController controller = new DespatchDateController();
            DespatchDate date = controller.Get(new List<int>() { 3 }, new DateTime(2021, 11, 01));
            date.Date.Date.ShouldBe(new DateTime(2021, 11, 01).Date.AddDays(3));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            DespatchDateController controller = new DespatchDateController();
            DespatchDate date = controller.Get(new List<int>() { 1 }, new DateTime(2021,11,05));
            date.Date.ShouldBe(new DateTime(2021, 11, 05).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            DespatchDateController controller = new DespatchDateController();
            DespatchDate date = controller.Get(new List<int>() { 3 }, new DateTime(2021, 11, 04));
            date.Date.ShouldBe(new DateTime(2021, 11, 04).Date.AddDays(4));
        }
    }
}
