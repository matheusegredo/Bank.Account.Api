# Introdução 
Bank.Api - Projeto para simulação de uma conta bancária.

Frameworks:
Mediatr, AutoMapper, FluentValidation.

ORM:
EntityFrameworkCore com Mysql.

Cache distribuido:
Redis.

Testes:
xunit.

# Execução do Projeto
Execução via docker:
1. Acessar a pasta raiz do projeto Bank.Api
2. Abrir o prompt de comando na pasta raiz.
3. Executar o comando "docker-compose up -d".
4. Acessar o projeto pela url "http://localhost:4200/swagger/index.html".
5. Redis será executado na porta 6379
6. Mysql será executado na porta 3306

# Funcionalidades:

## Account
POST: /api/account
> Método para incluir uma conta vinculada ao cliente.
>> Parâmetros marcados com * são obrigatórios.
```bash
# Body Params:
{
  *"clientId": 0,
  *"accountNumber": "string"
}

# resposta
{
    "data": [
        {
            "message": "Account inserted succesfully!",            
        },        
    ],
    "statusCode": 200,
    "errors": null
}
```

POST: /api/account/authentication
- **O método de autenticação foi criado apenas para demonstração do uso do token jwt, é necessário informar apenas o account number, em casos de produção, a autenticação deve ser mais segura.**
> Método autenticar uma conta existente.
>> Parâmetros marcados com * são obrigatórios.
```bash
# Body Params:
{
  *"accountNumber": "string"
}

# resposta
{
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA5LzA5L2lkZW50aXR5L2NsYWltcy9hY3RvciI6IjAwMDEiLCJuYmYiOjE2NDE0OTYwNjUsImV4cCI6MTY0MTQ5OTY2NSwiaXNzIjoiV2FycmVuIiwiYXVkIjoiQmFuay5BcGkifQ.Cy5ZI1MMSl2FLtEK_VYTDba6FSWe-er4bEips_SSj2c",
    "expiresOn": "2022-01-06T20:07:45.3399785Z"
  },
  "statusCode": 200,
  "errors": null
}
```

## AccountBalance
GET: /api/account/balance/{accountId}
- Necessário autenticação.
> Método obter saldo da conta.
>> Parâmetros marcados com * são obrigatórios.
```bash
# Route Params
accountId - int

# resposta
{
  "data": {
    *"accountId": int,
    *"value": decimal
  },
  "statusCode": 200,
  "errors": null
}
```

## AccountMovimentation
POST: /api/account/movimentation
- Necessário autenticação.
> Método para incluir uma movimentação na conta.
>> Parâmetros marcados com * são obrigatórios.
```bash
# Body Params:
{
  *"accountId": int,
  *"value": decimal,
  *"type": enum (1 - Depósito, 2 - Resgate, 3 - Pagamento)
}

# resposta
{
  "data": {
    "message": "Account movimentation inserted succefully!"
  },
  "statusCode": 200,
  "errors": null
}
```

GET: /api/account/movimentation
- Necessário autenticação.
> Método para obter movimentações e saldo da conta. Informar data inicial e final para obter movimentações por período de datas.
>> Parâmetros marcados com * são obrigatórios.
```bash
# Query Params:
*"AccountId": int,
"InitialDate": string(date-time),
"FinalDate": string(date-time)

# resposta
{
  "data": {
    "accountNumber": "string",
    "accountBalance": decimal,
    "movimentations": [
      {
        "value": decimal,
        "type": "string",
        "date": "string"
      }
    ]
  },
  "statusCode": 200,
  "errors": null
}
```

## Client
POST: /api/client
> Método para incluir um novo cliente.
>> Parâmetros marcados com * são obrigatórios.
```bash
# Body Params:
{
  *"firstName": "string",
  *"lastName": "string",
  *"email": "string"
}

# resposta
{
  "data": {
    "message": "Client inserted successfully!"
  },
  "statusCode": 200,
  "errors": null
}
```

## Respostas em caso de erro
```bash
{
  "data": null,
  "statusCode": {status retornado na resposta},
  "errors": [
    {
      "message": "Descrição do erro",
      "property": "Caso exista, propriedade da requisição inválida"
    }
  ]
}
```
