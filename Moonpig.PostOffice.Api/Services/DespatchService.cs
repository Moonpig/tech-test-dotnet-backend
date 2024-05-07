using Moonpig.PostOffice.Api.Interfaces;
using Moonpig.PostOffice.Api.Model;
using Moonpig.PostOffice.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Moonpig.PostOffice.Api.Services
{
    public class DespatchService : IDespatchService
    {
        private readonly IDataProvider _dataProvider;

        public DespatchService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        //public DespatchDate GetDespatchDates(List<int> productIds, DateTime orderDate)
        //{
        //    var maxLeadTime = orderDate; // max lead time
        //    foreach (var id in productIds)
        //    {
        //        var product = _dataProvider.GetProduct(id);
        //        var supplierId = product.SupplierId;

        //        var supplier = _dataProvider.GetSupplier(supplierId);
        //        var leadTime = supplier.LeadTime;

        //        if (orderDate.AddDays(leadTime) > maxLeadTime)
        //        {
        //            maxLeadTime = orderDate.AddDays(leadTime);
        //        }
        //    }
        //    if (maxLeadTime.DayOfWeek == DayOfWeek.Saturday)
        //    {
        //        return new DespatchDate { Date = maxLeadTime.AddDays(2) };
        //    }
        //    else if (maxLeadTime.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        return new DespatchDate { Date = maxLeadTime.AddDays(1) };
        //    }

        //    else return new DespatchDate { Date = maxLeadTime };
        //}

        public DespatchDate GetDespatchDates(List<int> productIds, DateTime orderDate)
        {
            var maxLeadTime = GetMaxLeadTime(productIds);

            if (orderDate.DayOfWeek == DayOfWeek.Friday)
            {
                var daysUntilMonday = GetDaysUntilMonday(orderDate);
                var leadTime = GetDaysUntilMonday(orderDate) + maxLeadTime;
                return new DespatchDate(orderDate.AddDays(leadTime));
            }
            if (orderDate.DayOfWeek == DayOfWeek.Saturday || orderDate.DayOfWeek == DayOfWeek.Sunday)
            {
                var daysUntilMonday = GetDaysUntilMonday(orderDate);
                return new DespatchDate(orderDate.AddDays(daysUntilMonday + maxLeadTime));
            }
            else
            {
                return new DespatchDate(orderDate.AddDays(maxLeadTime));
            };
        }

        private int GetMaxLeadTime(List<int> productIds)
        {
            var leadTimes = new List<int>();

            foreach (var id in productIds)
            {
                var product = _dataProvider.GetProduct(id);
                var supplierId = product.SupplierId;

                var supplier = _dataProvider.GetSupplier(supplierId);
                var leadTime = supplier.LeadTime;

                leadTimes.Add(leadTime);
            }

            return leadTimes.Max();
        }

        private static int GetDaysUntilMonday(DateTime date) => (DayOfWeek.Monday - date.DayOfWeek + 7) % 7;

        private static int GetWeekendDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            var dates = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToArray();

            var weekendDays = dates.Where(x => x.DayOfWeek == DayOfWeek.Saturday || x.DayOfWeek == DayOfWeek.Sunday);
            return weekendDays.Count();
        }

        private static int GetWeekDaysBetweenDates(DateTime startDate, DateTime endDate)
        {
            var dates = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .ToArray();

            var weekendDays = dates.Where(x => x.DayOfWeek != DayOfWeek.Saturday || x.DayOfWeek != DayOfWeek.Sunday);
            return weekendDays.Count();
        }

        private static DateTime AddWeekdays(DateTime startDate, int weekdaysToAdd)
        {
            var maxLeadTime = 30;

            var weekdays = Enumerable.Range(0, maxLeadTime)
                .Select(offset => startDate.AddDays(offset))
                .Where(x => x.DayOfWeek != DayOfWeek.Saturday)
                .Where(x => x.DayOfWeek != DayOfWeek.Sunday)
                .ToList();

            return weekdays[weekdaysToAdd];
        }
    }
}