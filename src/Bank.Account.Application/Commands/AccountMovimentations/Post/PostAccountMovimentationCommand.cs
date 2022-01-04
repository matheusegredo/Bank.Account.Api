using Bank.Data;

namespace Bank.Application.Commands.AccountMovimentations.Post
{
    public class PostAccountMovimentationCommand : IRequest<PostAccountMovimentationCommandResponse>
    {
        public int AccountId { get; set; }

        public decimal Value { get; set; }

        public MovimentationType Type { get; set; }
    }
}
