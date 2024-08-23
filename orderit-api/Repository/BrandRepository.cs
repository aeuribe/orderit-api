using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;

namespace orderit_api.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly BusinessContext _context;
        public BrandRepository(BusinessContext context)
        {
            _context = context;
        }

        public bool BrandExists(int id)
        {
            return _context.Brands.Any(b => b.BrandId == id);
        }

        public Brand GetBrand(int id)
        {
            return _context.Brands.Where(b => b.BrandId == id).FirstOrDefault();
        }
    }
}
