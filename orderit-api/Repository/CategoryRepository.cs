using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;

namespace orderit_api.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BusinessContext _context;
        public CategoryRepository(BusinessContext context)
        {
            _context = context;
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.CategoryId == id);
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.CategoryId == id).FirstOrDefault();
        }
    }
}
