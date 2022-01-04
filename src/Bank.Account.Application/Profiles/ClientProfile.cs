global using AutoMapper;
using Bank.Application.Commands.Clients.Post;
using Bank.Data.Entities;

namespace Bank.Application.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<PostClientCommand, Client>();
        }
    }
}
