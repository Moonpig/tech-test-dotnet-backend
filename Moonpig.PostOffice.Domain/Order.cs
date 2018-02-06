namespace Moonpig.PostOffice.Domain
{
    using System;
    using System.Collections.Generic;

    public class Order
    {
        public Order(List<int> productIds, DateTime orderDate)
        {
            ProductIds = productIds;
            OrderDate = orderDate;
        }

        public List<int> ProductIds { get; private set; }

        public DateTime OrderDate { get; private set; }
    }
}
