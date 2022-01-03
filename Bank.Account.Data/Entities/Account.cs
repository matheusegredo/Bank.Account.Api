namespace Bank.Data.Entities
{
    public class Account : Auditable
    {
        public Account()
        {
            Movimentations = new HashSet<AccountMovimentation>();
        }

        public int AccountId { get; set; }

        public int ClientId { get; set; }

        public string AccountNumber { get; set; }

        public string Password { get; set; }

        public Client Client { get; set; }

        public ICollection<AccountMovimentation> Movimentations { get; set; }

        public AccountBalance AccountBalance { get; set; }
    }
}
