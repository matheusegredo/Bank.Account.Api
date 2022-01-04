using Bank.Application.Commands.Accounts.Post;
using Bank.Data.Entities;

namespace Bank.Application.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<PostAccountCommand, Account>();
        }
    }
}
