using Moonpig.PostOffice.Api.Interfaces;
using Moonpig.PostOffice.Api.Model;
using Moonpig.PostOffice.Data;
using System;
using System.Collections.Generic;

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
            var maxLeadTime = orderDate; // max lead time
            foreach (var ID in productIds)
            {
                var product = _dataProvider.GetProduct(ID);
                var supplierId = product.SupplierId;

                var supplier = _dataProvider.GetSupplier(supplierId);
                var leadTime = supplier.LeadTime;

                if (orderDate.AddDays(leadTime) > maxLeadTime)
                    maxLeadTime = orderDate.AddDays(leadTime);
            }
            if (maxLeadTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return new DespatchDate { Date = maxLeadTime.AddDays(2) };
            }
            else if (maxLeadTime.DayOfWeek == DayOfWeek.Sunday)
                return new DespatchDate { Date = maxLeadTime.AddDays(1) };

            else return new DespatchDate { Date = maxLeadTime };
        }
    }
}