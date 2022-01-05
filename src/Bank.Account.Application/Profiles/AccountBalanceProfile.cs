using Bank.Application.Commands.AccountBalances.Notifications;
using Bank.Application.Commands.AccountBalances.Notifications.Post;
using Bank.Application.Queries.AccountBalances;
using Bank.Data.Entities;
using Bank.Infrastructure.Cache.Models.AccountBalances;

namespace Bank.Application.Profiles
{
    public class AccountBalanceProfile : Profile
    {
        public AccountBalanceProfile()
        {
            CreateMap<PostCacheAccountBalanceNotification, AccountBalanceCacheModel>();

            CreateMap<PutAccountBalanceNotification, AccountBalance>();

            CreateMap<AccountBalance, GetAccountBalanceQueryResponse>();

            CreateMap<AccountBalanceCacheModel, GetAccountBalanceQueryResponse>();
        }
    }
}
