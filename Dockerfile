FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Bank.Account.Api/Bank.Api.csproj", "src/Bank.Account.Api/"]
COPY ["src/Bank.Account.Application/Bank.Application.csproj", "src/Bank.Account.Application/"]
COPY ["src/Bank.CrossCutting.Exceptions/Bank.CrossCutting.Exceptions.csproj", "src/Bank.CrossCutting.Exceptions/"]
COPY ["src/Bank.Infrastructure.Cache/Bank.Infrastructure.Cache.csproj", "src/Bank.Infrastructure.Cache/"]
COPY ["src/Bank.Account.Persistence/Bank.Persistence.csproj", "src/Bank.Account.Persistence/"]
COPY ["src/Bank.Account.Data/Bank.Data.csproj", "src/Bank.Account.Data/"]
RUN dotnet restore "src/Bank.Account.Api/Bank.Api.csproj"
COPY . .
WORKDIR "/src/src/Bank.Account.Api"
RUN dotnet build "Bank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bank.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
FROM base AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bank.Api.dll"]