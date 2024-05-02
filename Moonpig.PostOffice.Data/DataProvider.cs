using System;
using System.Linq;
using Moonpig.PostOffice.Data.Entities;

namespace Moonpig.PostOffice.Data
{
    public class DataProvider : IDataProvider
    {
        private readonly DbContext _dbContext;

        public DataProvider(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Product GetProduct(int id)
        {
            bool func(Product product) => product.ProductId == id;
            return !_dbContext.Products.Any(func) ? null : _dbContext.Products.Single(func);
        }

        public Supplier GetSupplier(int id)
        {
            bool func(Supplier supplier) => supplier.SupplierId == id;
            return !_dbContext.Suppliers.Any(func) ? null : _dbContext.Suppliers.Single(func);
        }
    }
}