using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface ICategoryRepository
    {
        public Category GetCategory(int id);
        public bool CategoryExists(int id);
    }
}
