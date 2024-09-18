namespace orderit_api.Models
{
    public class LoginUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }  
        public string? Role { get; set; }
        public int? SalespersonId { get; set; }
    }
}
