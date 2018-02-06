namespace Moonpig.PostOffice.Tests
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            PostOffice postOffice = new PostOffice();
            var date = postOffice.CalculateDespatchDate(new Order(new List<int>() {1}, DateTime.Now));
            date.Date.ShouldBe(DateTime.Now.Date.AddDays(1));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            PostOffice postOffice = new PostOffice();
            var date = postOffice.CalculateDespatchDate(new Order(new List<int>() { 2 }, DateTime.Now));
            date.Date.ShouldBe(DateTime.Now.Date.AddDays(2));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            PostOffice postOffice = new PostOffice();
            var date = postOffice.CalculateDespatchDate(new Order(new List<int>() { 3 }, DateTime.Now));
            date.Date.ShouldBe(DateTime.Now.Date.AddDays(3));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            PostOffice postOffice = new PostOffice();
            var date = postOffice.CalculateDespatchDate(new Order(new List<int>() { 1 }, new DateTime(2018,1,26)));
            date.Date.ShouldBe(new DateTime(2018, 1, 26).Date.AddDays(3));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            PostOffice postOffice = new PostOffice();
            var date = postOffice.CalculateDespatchDate(new Order(new List<int>() { 3 }, new DateTime(2018, 1, 25)));
            date.Date.ShouldBe(new DateTime(2018, 1, 25).Date.AddDays(4));
        }
    }
}
