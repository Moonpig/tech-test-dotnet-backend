using Moonpig.PostOffice.Data.Entities;

namespace Moonpig.PostOffice.Data.Interfaces
{
    public interface IDataProvider
    {
        Product GetProduct(int id);
        Supplier GetSupplier(int id);
    }
}