# Lead Management API (.NET Core)

API RESTful para gerenciamento de leads desenvolvida com .NET Core 6, seguindo princípios de DDD e CQRS.

## 🏗️ Arquitetura

- **Domain-Driven Design (DDD)**
- **CQRS**
- **Repository Pattern**
- **Entity Framework Core**
- **SQL Server**

## 🚀 Como Executar

### 🐳 Docker (Recomendado)

```bash
# Build e execução com Docker Compose
docker-compose up -d

# Verificar status
docker-compose ps

# Ver logs
docker-compose logs -f api
```

**Acessos:**
- 🔌 API: http://localhost:5000
- 🗄️ Database: localhost:1433 (sa/LeadManagement123!)

### 💻 Desenvolvimento Local

**Pré-requisitos:**
- .NET 6 SDK
- SQL Server (LocalDB ou instância)

```bash
# Restaurar dependências
dotnet restore

# Configurar banco (se necessário)
dotnet ef database update --project LeadsManagement.Infra

# Executar API
cd LeadsManagement.Api
dotnet run
```

## 📁 Estrutura do Projeto

```
back/
├── LeadsManagement.Api/        # Web API (Controllers, Startup)
├── LeadsManagement.Domain/     # Domínio (Entities, Commands, Handlers)
├── LeadsManagement.Infra/      # Infraestrutura (EF, Repositories)
├── LeadsManagement.Tests/      # Testes Unitários
├── Dockerfile                  # Container da API
├── docker-compose.yml          # Orquestração (API + DB)
└── README.md                   # Este arquivo
```

## 🔍 API Endpoints

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/v1/leads/waiting?pageNumber=1&pageSize=10` | Lista leads aguardando |
| GET | `/v1/leads/accepted?pageNumber=1&pageSize=10` | Lista leads aceitos |
| PUT | `/v1/leads/{id}` | Atualiza status do lead |

## 💼 Regras de Negócio

### Status dos Leads
- **1 - Waiting:** Lead aguardando decisão
- **2 - Accepted:** Lead aceito
- **3 - Rejected:** Lead rejeitado

### Desconto Automático
- Leads com preço > $500 recebem **10% de desconto** automaticamente ao serem aceitos
- O desconto é aplicado na camada de domínio via método `ApplyDiscount()`

### Validações
- FirstName obrigatório
- Email válido obrigatório  
- Price deve ser > 0

## 🧪 Testes

```bash
# Executar todos os testes
dotnet test

# Testes com coverage
dotnet test --collect:"XPlat Code Coverage"

### Cenários de Teste
- ✅ Criação de leads válidos
- ✅ Validações de domínio
- ✅ Aplicação de desconto
- ✅ Atualização de status
- ✅ Handlers de commands

## 🐳 Docker

### Build da Imagem
```bash
# Build local
docker build -t leadmanagement-api .

# Run container
docker run -p 5000:80 leadmanagement-api
```

## 🛠️ Tecnologias

- **.NET Core 6**
- **Entity Framework Core** - ORM
- **SQL Server** - Database
- **MSTest** - Testes unitários
- **Docker** - Containerização

## 📄 Licença

Este projeto é parte de um desafio técnico. 