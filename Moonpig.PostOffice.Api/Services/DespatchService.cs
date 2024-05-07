using Moonpig.PostOffice.Api.Interfaces;
using Moonpig.PostOffice.Api.Model;
using Moonpig.PostOffice.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moonpig.PostOffice.Api.Services
{
    public class DespatchService : IDespatchService
    {
        private readonly IDataProvider _dataProvider;

        public DespatchService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public DespatchDate GetDespatchDates(List<int> productIds, DateTime orderDate)
        {
            var maxLeadTime = GetMaxLeadTime(productIds);
            var dispatchDate = AddWeekdays(orderDate, maxLeadTime);

            return new DespatchDate(dispatchDate);
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