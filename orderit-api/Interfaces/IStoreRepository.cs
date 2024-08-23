using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface IStoreRepository
    {
        Store GetById(int id);
        ICollection<Store> GetStores();
        bool CreateStore(Store store);
        bool UpdateStore(Store store);
        bool DeleteStore(Store store);
        bool StoreExist(int  id);
        bool Save();
    }
}
