namespace Moonpig.PostOffice.Domain
{
    using System;
    using System.Linq;
    using Data;

    public class PostOffice : IPostOffice
    {
        public DateTime _mlt;

        public DateTime CalculateDespatchDate(Order order)
        {
            _mlt = order.OrderDate; // max lead time
            foreach (var ID in order.ProductIds)
            {
                DbContext dbContext = new DbContext();
                var s = dbContext.Products.Single(x => x.ProductId == ID).SupplierId;
                var lt = dbContext.Suppliers.Single(x => x.SupplierId == s).LeadTime;
                if (order.OrderDate.AddDays(lt) > _mlt)
                    _mlt = order.OrderDate.AddDays(lt);
            }
            if (_mlt.DayOfWeek == DayOfWeek.Saturday){
                return _mlt.AddDays(2);
            }
            else if (_mlt.DayOfWeek == DayOfWeek.Sunday) return _mlt.AddDays(1);
            else return _mlt;
        }
    }
}