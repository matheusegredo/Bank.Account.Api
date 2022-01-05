using Bank.Application.Commands.AccountMovimentations.Post;
using Bank.Data.Entities;

namespace Bank.Application.Profiles
{
    public class AccountMovimentationProfile : Profile
    {
        public AccountMovimentationProfile()
        {
            CreateMap<PostAccountMovimentationCommand, AccountMovimentation>();
        }
    }
}
