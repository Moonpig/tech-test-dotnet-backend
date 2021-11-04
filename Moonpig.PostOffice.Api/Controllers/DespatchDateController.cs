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

        /// <summary>
        /// Get method for the DespatchDate API designed to return the despatch date for a product based on ordered date.
        /// </summary>
        /// <param name="productIds">Id for the list of products a despatch date is requested for</param>
        /// <param name="orderDate">Date the order was requested</param>
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

            // check each date between the order date and the max lead time, count the weekend days that exist between these dates.
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
