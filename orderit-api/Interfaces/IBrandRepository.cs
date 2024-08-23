using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface IBrandRepository
    {
        public Brand GetBrand(int id);
        public bool BrandExists(int id);
    }
}
