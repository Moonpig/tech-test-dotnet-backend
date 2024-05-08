using System;
using Xunit;

namespace Moonpig.PostOffice.Api.Tests.Services
{
    public static class TestCases
    {
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

        public static TheoryData<DateOnly, DateOnly, int> VaryingLeadTimeOrders = new()
        {
            { new (2018, 1, 1), new (2018, 1, 17), 12 },
            { new (2018, 1, 2), new (2018, 1, 30), 20 },
            { new(2018, 1, 13), new(2018, 1, 24), 7 },
            { new(2018, 1, 5), new(2018, 1, 25), 14 },
            { new(2018, 1, 10), new(2018, 2, 14), 25 },
            { new(2018, 1, 7), new(2018, 1, 9), 1 },
            { new(2018, 1, 25), new(2018, 1, 30), 3 },
        };
    }
}