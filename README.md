# 🚗 Seguro de Veículos

API REST para registro e cálculo de seguros de veículos, com frontend de relatório de médias, construída com Clean Architecture em .NET 10 e React.

---

## 📐 Arquitetura

```
┌─────────────────────────────────────────────────────────────────────┐
│                        Docker Compose                               │
│                                                                     │
│  ┌──────────────┐     ┌──────────────┐     ┌──────────────────────┐│
│  │   Frontend   │────▶│     API      │────▶│     PostgreSQL       ││
│  │  React/Nginx │     │  .NET 10     │     │  (seguro_veiculos)   ││
│  │  :5080       │     │  :5070/:8080 │     │  :5432               ││
│  └──────────────┘     └──────┬───────┘     └──────────────────────┘│
│                              │                                      │
│                              ▼                                      │
│                    ┌──────────────────┐    ┌──────────────────────┐│
│                    │   Mock REST      │    │      pgAdmin         ││
│                    │  JSON Server     │    │  (Admin DB)          ││
│                    │  :3001           │    │  :5050               ││
│                    └──────────────────┘    └──────────────────────┘│
└─────────────────────────────────────────────────────────────────────┘
```

### Camadas — Clean Architecture

```
SeguroVeiculos
├── Domain          → Entidades, Value Objects, Interfaces (sem dependências externas)
├── Application     → Use Cases, Commands, Responses, Interfaces de serviços
├── Infrastructure  → EF Core, Repositórios, Serviço HTTP externo (JSON Server)
└── API             → Controllers, Swagger, Program.cs
```

---

## 🛠️ Tecnologias

| Camada       | Tecnologia                            |
|--------------|---------------------------------------|
| Backend      | .NET 10, ASP.NET Core, EF Core        |
| Frontend     | React 19, TypeScript, Vite, Tailwind  |
| Banco        | PostgreSQL 16                         |
| ORM          | Entity Framework Core + Npgsql        |
| Docs         | Swagger / OpenAPI                     |
| Mock REST    | JSON Server (Node 22)                 |
| Admin DB     | pgAdmin 4                             |
| Testes       | xUnit                                 |
| Container    | Docker + Docker Compose               |
| Cloud        | Azure Static Web Apps + App Service   |

---

## 🐳 Rodando com Docker Compose

### Pré-requisitos

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado e em execução

### Subir todos os serviços

```bash
docker compose up -d --build
```

> O `--build` reconstrói as imagens da API e do Frontend. Após a primeira vez, pode usar apenas `docker compose up -d`.

### Parar os serviços

```bash
docker compose down
```

### URLs disponíveis

| Serviço        | URL                                        | Descrição                          |
|----------------|--------------------------------------------|------------------------------------|
| **Frontend**   | http://localhost:5080                      | Relatório de Médias Aritméticas    |
| **Swagger**    | http://localhost:5070/swagger              | Documentação interativa da API     |
| **pgAdmin**    | http://localhost:5050                      | Interface visual do banco de dados |
| **Mock REST**  | http://localhost:3001/segurados            | Mock do serviço externo de segurados |

#### Acesso ao pgAdmin

| Campo    | Valor           |
|----------|-----------------|
| Email    | `admin@admin.com` |
| Senha    | `admin`         |

> O servidor PostgreSQL já vem pré-configurado automaticamente. Basta abrir o pgAdmin e expandir **Servers → Seguro Veículos - Dev**.

---

## 💻 Rodando Localmente (sem Docker)

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 22+](https://nodejs.org/)
- [PostgreSQL 16](https://www.postgresql.org/) rodando em `localhost:5432`

### 1. Mock REST (JSON Server)

```bash
npx json-server mock/db.json --port 3001
```

### 2. Backend (.NET)

```bash
dotnet run --project src/SeguroVeiculos.API
```

API disponível em: http://localhost:5070/swagger

### 3. Frontend (React + Vite)

```bash
cd frontend
npm install
npm run dev
```

Frontend disponível em: http://localhost:5173

---

## 🔌 Endpoints da API

| Método | Rota                          | Descrição                            |
|--------|-------------------------------|--------------------------------------|
| POST   | `/api/seguro`                 | Cria um novo seguro                  |
| GET    | `/api/seguro/{id}`            | Busca um seguro por ID               |
| GET    | `/api/seguro/relatorio/medias`| Retorna médias aritméticas dos seguros |

### Exemplo — Criar Seguro

```json
POST /api/seguro
{
  "valorVeiculo": 45000,
  "marcaModeloVeiculo": "Toyota Corolla",
  "cpfSegurado": "12345678900",
  "nomeSegurado": "João",
  "idadeSegurado": 30
}
```

> Se o CPF existir no Mock REST, o nome e a idade são buscados automaticamente do serviço externo.

---

## 🌐 Serviço Externo de Segurados (Mock REST)

O sistema consulta dados do segurado via REST antes de criar o seguro.  
O [JSON Server](https://github.com/typicode/json-server) simula esse serviço externo.

**Segurados disponíveis no mock:**

| CPF           | Nome            | Idade |
|---------------|-----------------|-------|
| 12345678900   | João da Silva   | 30    |
| 98765432100   | Maria Oliveira  | 45    |
| 11122233344   | Pedro Santos    | 22    |
| 55566677788   | Ana Costa       | 38    |

> Se o serviço estiver indisponível, o sistema usa os dados enviados no body como **fallback**.

---

## 🧪 Testes

```bash
dotnet test
```

---

## ☁️ Deploy no Azure

| Serviço        | Plataforma                 | URL                                                                                    |
|----------------|----------------------------|----------------------------------------------------------------------------------------|
| **Frontend**   | Azure Static Web Apps      | https://kind-grass-0fe31ee0f.azurestaticapps.net                                       |
| **API**        | Azure App Service          | https://seguroveiculodesafio-e9a9djaddyg5gcds.canadacentral-01.azurewebsites.net/swagger|
| **Banco**      | Supabase (PostgreSQL)      | Pool: `aws-0-us-west-2.pooler.supabase.com`                                            |

O deploy do frontend é feito automaticamente via **GitHub Actions** a cada push na branch `master`.

