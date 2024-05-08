using System;
using System.Collections.Generic;
using System.Linq;
using Moonpig.PostOffice.Api.Interfaces;
using Moonpig.PostOffice.Api.Model;
using Moonpig.PostOffice.Data.Interfaces;

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
            if (productIds.Count == 0 || orderDate == DateTime.MinValue)
            {
                return null;
            }

            var maxLeadTime = GetMaxLeadTime(productIds);
            var despatchDate = GetDespatchDate(orderDate, maxLeadTime);

            return new DespatchDate(despatchDate);
        }

        private int GetMaxLeadTime(List<int> productIds)
        {
            var leadTimes = productIds.Select(productId =>
            {
                var product = _dataProvider.GetProduct(productId);
                var supplier = _dataProvider.GetSupplier(product.SupplierId);

                return supplier.LeadTime;
            });

            var leadTime = leadTimes.Max();

            if (leadTime < 1)
            {
                throw new NotSupportedException("Same day despatch not supported.");
            }

            return leadTime;
        }

        private static DateTime GetDespatchDate(DateTime startDate, int weekdaysToAdd)
        {
            var weekdays = Enumerable.Range(0, 45)
                .Select(offset => startDate.AddDays(offset))
                .Where(x => x.DayOfWeek != DayOfWeek.Saturday)
                .Where(x => x.DayOfWeek != DayOfWeek.Sunday)
                .ToList();

            return weekdays[weekdaysToAdd];
        }
    }
}