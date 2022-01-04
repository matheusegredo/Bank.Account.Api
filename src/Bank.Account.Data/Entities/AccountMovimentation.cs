namespace Bank.Data.Entities
{
    public class AccountMovimentation : Auditable
    {
        public int AccountMovimentationId { get; set; }

        public int AccountId { get; set; }

        public decimal Value { get; set; }

        public MovimentationType Type { get; set; }

        public Account? Account { get; set; }
    }
}
