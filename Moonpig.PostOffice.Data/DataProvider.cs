using System;
using System.Linq;
using Moonpig.PostOffice.Data.Entities;
using Moonpig.PostOffice.Data.Interfaces;

namespace Moonpig.PostOffice.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly IDbContext _dbContext;

        public DataProvider(IDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Product GetProduct(int id)
        {
            bool query(Product product) => product.ProductId == id;
            return !_dbContext.Products.Any(query) ? null : _dbContext.Products.Single(query);
        }

        public Supplier GetSupplier(int id)
        {
            bool query(Supplier supplier) => supplier.SupplierId == id;
            return !_dbContext.Suppliers.Any(query) ? null : _dbContext.Suppliers.Single(query);
        }
    }
}