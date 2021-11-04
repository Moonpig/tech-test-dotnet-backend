namespace Moonpig.PostOffice.Data
{
    using System.Linq;
    using System.Collections.Generic;

    public class DbContext : IDbContext
    {
        public IQueryable<Supplier> Suppliers
        {
            get
            {
                return new List<Supplier>
                {
                    new Supplier
                    {
                        SupplierId = 1,
                        Name = "Acme Corporation",
                        LeadTime = 1
                    },
                    new Supplier
                    {
                        SupplierId = 2,
                        Name = "Sunnyside Flowers",
                        LeadTime = 2
                    },
                    new Supplier
                    {
                        SupplierId = 3,
                        Name = "Drinks Warehouse",
                        LeadTime = 1
                    },
                    new Supplier
                    {
                        SupplierId = 4,
                        Name = "TailSpin Toys",
                        LeadTime = 3
                    },
                    new Supplier
                    {
                        SupplierId = 5,
                        Name = "Disney",
                        LeadTime = 6
                    }
                    ,
                    new Supplier
                    {
                        SupplierId = 6,
                        Name = "Tacky T-Shrts",
                        LeadTime = 11
                    }
                }.AsQueryable();
            }
        }

        public IQueryable<Product> Products
        {
            get
            {
                return new List<Product>
                           {
                               new Product { ProductId = 1, Name = "Greetings Card", SupplierId = 1 },
                               new Product { ProductId = 2, Name = "Flowers", SupplierId = 2 },
                               new Product { ProductId = 3, Name = "Soft Toy", SupplierId = 4 },
                               new Product { ProductId = 4, Name = "Chocolate", SupplierId = 1 },
                               new Product { ProductId = 5, Name = "Canvas", SupplierId = 1 },
                               new Product { ProductId = 6, Name = "Plant", SupplierId = 2 },
                               new Product { ProductId = 7, Name = "Alcohol", SupplierId = 1 },
                               new Product { ProductId = 8, Name = "Box Set", SupplierId = 7 },
                               new Product { ProductId = 9, Name = "Frozen Doll", SupplierId = 5 },
                               new Product { ProductId = 10, Name = "Personalised Tacky Thshrt", SupplierId = 6 },
                           }.AsQueryable();
            }
        }
    }
}
