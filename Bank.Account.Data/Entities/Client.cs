namespace Bank.Data.Entities
{
    public class Client : Auditable
    {
        public int ClientId { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public Account? Account { get; set; }
    }
}
