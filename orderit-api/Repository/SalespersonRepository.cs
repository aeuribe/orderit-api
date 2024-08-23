using orderit_api.Data;
using orderit_api.Interfaces;
using orderit_api.Models;

namespace orderit_api.Repository
{
    public class SalespersonRepository : ISalespersonRepository
    {
        BusinessContext _context;
        public SalespersonRepository(BusinessContext businessContext)
        {
            _context = businessContext;
        }
        public bool CreateSalesperson(Salesperson salesperson)
        {
            _context.Add(salesperson);
            return Save();
        }

        public bool DeleteSalesperson(Salesperson salesperson)
        {
            _context.Remove(salesperson);
            return Save();
        }

        public Salesperson GetById(int id)
        {
            return _context.Salespersons.Where(s => s.SalespersonId == id).FirstOrDefault();
        }


        public ICollection<Salesperson> GetSalespersons()
        {
            return _context.Salespersons.ToList();
        }

        public bool SalespersonExist(int id)
        {
            return _context.Salespersons.Any(s => s.SalespersonId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SellerExist(int id)
        {
            return _context.Salespersons.Any(S => S.SalespersonId == id);
        }

        public bool UpdateSalesperson(Salesperson salesperson)
        {
            _context.Update(salesperson);
            return Save();
        }
    }
}
