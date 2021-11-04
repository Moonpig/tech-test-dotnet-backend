namespace Moonpig.PostOffice.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Moonpig.PostOffice.Data;
    using Microsoft.AspNetCore.Mvc;
    using Moonpig.PostOffice.Api.Model;

    [Route("api/[controller]")]
    public class DespatchDateController : Controller
    {
        private DateTime _maxLeadTime;

        [HttpGet]
        public DespatchDate Get(List<int> productIds, DateTime orderDate)
        {
            _maxLeadTime = orderDate; // max lead time

            foreach (int ID in productIds)
            {
                DbContext dbContext = new DbContext();
                int supplierId = dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
                int leadTime = dbContext.Suppliers.Single(x => x.SupplierId == supplierId).LeadTime;
                if (orderDate.AddDays(leadTime) > _maxLeadTime) 
                {
                    _maxLeadTime = orderDate.AddDays(leadTime);
                }
            }

            int weekendCounter = 0;
            DateTime dateHolder = orderDate;

            while (dateHolder <= _maxLeadTime)
            {
                if(dateHolder.DayOfWeek == DayOfWeek.Saturday || dateHolder.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekendCounter++;
                }

                dateHolder = dateHolder.AddDays(1);
            }

            _maxLeadTime = _maxLeadTime.AddDays(weekendCounter);


            if (_maxLeadTime.DayOfWeek == DayOfWeek.Saturday)
            {
                return new DespatchDate { Date = _maxLeadTime.AddDays(2) };
            }
            else if (_maxLeadTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return new DespatchDate { Date = _maxLeadTime.AddDays(1) };
            }
            else
            {
                return new DespatchDate { Date = _maxLeadTime };
            }
        }
    }
}
