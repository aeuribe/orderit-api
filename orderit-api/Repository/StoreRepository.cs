using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;

namespace orderit_api.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly BusinessContext _context;
        public StoreRepository(BusinessContext context)
        {
            _context = context;
        }

        public bool CreateStore(Store store)
        {
            _context.Add(store);
            return Save();
        }

        public bool DeleteStore(Store store)
        {
            _context.Stores.Remove(store);
            return Save();
        }

        public Store GetById(int id)
        {
            return _context.Stores.Where(s => s.StoreId == id).FirstOrDefault();
        }

        public ICollection<Store> GetStores()
        {
            return _context.Stores.ToList();
        }

        public bool Save()
        {
            var saved =_context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool StoreExist(int id)
        {
            return _context.Stores.Any(s => s.StoreId == id);
        }

        public bool UpdateStore(Store store)
        {
            _context.Stores.Update(store);
            return Save();
        }
    }
}
