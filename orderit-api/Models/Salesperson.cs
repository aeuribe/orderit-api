namespace orderit_api.Models
{
    public class Salesperson
    {
        public int SalespersonId { get; set; }
        public string? UserId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }

    }
}
