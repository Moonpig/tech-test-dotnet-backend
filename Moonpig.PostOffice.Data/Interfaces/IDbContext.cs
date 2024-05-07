namespace Moonpig.PostOffice.Data.Interfaces
{
    using System.Linq;
    using Moonpig.PostOffice.Data.Entities;

    public interface IDbContext
    {
        IQueryable<Supplier> Suppliers { get; }

        IQueryable<Product> Products { get; }
    }
}