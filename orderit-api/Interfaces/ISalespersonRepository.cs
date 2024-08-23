using orderit_api.Models;

namespace orderit_api.Interfaces
{
    public interface ISalespersonRepository
    {
        Salesperson GetById(int id); 
        ICollection<Salesperson> GetSalespersons(); 
        bool CreateSalesperson(Salesperson salesperson);
        bool UpdateSalesperson(Salesperson salesperson);
        bool DeleteSalesperson(Salesperson salesperson);
        bool SalespersonExist(int id);
        bool Save();
    }
}
