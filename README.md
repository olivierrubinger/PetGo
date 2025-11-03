# PetGo

`Eixo 4 - Projeto: Desenvolvimento de Aplicações Distribuídas - Turma 01 - 2025/2`

`4º SEMESTRE`

---

## 🌐 Deploy em Produção

| Serviço                | URL                                                                                                | Status   |
| ---------------------- | -------------------------------------------------------------------------------------------------- | -------- |
| **Frontend (Vercel)**  | [https://pet-go-puc.vercel.app](https://pet-go-puc.vercel.app)                                     | ✅ Ativo |
| **Backend (Railway)**  | [https://petgo-production.up.railway.app](https://petgo-production.up.railway.app)                 | ✅ Ativo |
| **API Docs (Swagger)** | [https://petgo-production.up.railway.app/swagger](https://petgo-production.up.railway.app/swagger) | ✅ Ativo |
| **Banco de Dados**     | Supabase PostgreSQL                                                                                | ✅ Ativo |

---

## Integrantes

- Olivier Lopes Rubinger
- Ligia de Castro Martins
- Marcello Abritta Nogueira Rezende

## Orientador

- Luiz Alberto Ferreira Gomes

---

# Visão do Produto

Para **tutores de animais de estimação**, cujo problema é a **falta de tempo para passeios, dificuldade em encontrar serviços confiáveis e acesso limitado a produtos e adoções seguras**.  
O **PetGo** é uma **plataforma digital integrada** que **facilita passeios, promove adoções responsáveis e conecta serviços especializados**.  
Diferentemente das soluções fragmentadas, que combinam redes sociais, lojas genéricas e serviços isolados, o PetGo oferece **um ecossistema único** que promove a confiança e o bem-estar dos pets e suas famílias.

---

## Etapa 1: Requisitos do produto

- [Problemas e dores atuais](docs/problemas.md)
- [Expectativas com o produto](docs/expectativas.md)
- [Personas do produto](docs/personas.md)
- [Entendendo as funcionalidades](docs/funcionalidades.md)
- [Trade-off de requisitos não funcionais](docs/tradeoffs.md)
- [Diagrama de Contexto](docs/diagrama-de-contexto.md)

---

### Etapa 2: Integração de APIs com banco de dados e serviços externos

- [Diagrama de Contêiner](docs/diagrama-de-conteiner.md)
- [Especificação das APIs](docs/apis.md)
- [Diagrama Entidade-Relacionamento](docs/projeto-do-banco-de-dados.md)
- [Roteiro de implementação](docs/roteiro-de-implementacao-etapa-2.md)

---

### Etapa 3: Arquitetura da aplicação em nuvem

- [Roteiro de implementação](docs/roteiro-de-implementacao-etapa-3.md)

---

### Etapa 4: Implantação da aplicação em nuvem

- [Roteiro de implementação](docs/roteiro-de-implementacao.md)

---

### Etapa 5: Entrega e apresentação do produto

- [Roteiro para entrega e apresentação](docs/roteiro-de-entrega-e-apresentacao.md)

---

## 📋 Sobre o Projeto

**PetGo** é uma plataforma completa para cuidado de animais de estimação que conecta tutores, prestadores de serviços (dog walkers/pet sitters) e voluntários de ONGs de adoção. A aplicação oferece funcionalidades como:

- 🐕 **Catálogo de Produtos**: Ração, brinquedos, acessórios e produtos de higiene
- 🚶 **Serviços de Passeio**: Conexão entre tutores e dog walkers
- ❤️ **Adoção de Pets**: Plataforma para doação e adoção responsável
- ⭐ **Sistema de Avaliações**: Feedback sobre produtos e serviços
- 👤 **Gerenciamento de Usuários**: Cadastro e autenticação

## 🏗️ Arquitetura do Sistema

### **Stack Tecnológica**

- **Frontend**: Next.js 15 (React/TypeScript) com Tailwind CSS
  - Deploy: **Vercel** ([https://pet-go-puc.vercel.app](https://pet-go-puc.vercel.app))
  - CI/CD: Automático via GitHub
- **Backend**: ASP.NET Core 9.0 (C#) com Entity Framework Core
  - Deploy: **Railway** ([https://petgo-production.up.railway.app](https://petgo-production.up.railway.app))
  - CI/CD: Automático via GitHub
- **Banco de Dados**: **Supabase PostgreSQL**
  - Managed PostgreSQL 15
  - Connection Pooler (PgBouncer)
  - Backups automáticos
- **API**: REST com documentação Swagger/OpenAPI
  - Swagger UI: [https://petgo-production.up.railway.app/swagger](https://petgo-production.up.railway.app/swagger)

---

## 🚀 Como Executar o Projeto

### 📋 **Pré-requisitos**

Certifique-se de ter instalado:

- **.NET 9.0 SDK** - [Download aqui](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js 18+** - [Download aqui](https://nodejs.org/)
- **Git** - [Download aqui](https://git-scm.com/)

### 1️⃣ **Clonar o Repositório**

```bash
git clone https://github.com/[seu-usuario]/PetGo.git
cd PetGo
```

### 2️⃣ **Configurar e Executar o Backend (.NET Core)**

```bash
# Navegar para a pasta do backend
cd src/backend/petgo-api

# Restaurar dependências
dotnet restore

# Configurar connection string do banco de dados
# Edite appsettings.json ou appsettings.Development.json
# Exemplo com Supabase PostgreSQL:
# "DefaultConnection": "Host=db.xxx.supabase.co;Port=5432;Database=postgres;Username=postgres.xxx;Password=xxx;SSL Mode=Require"

# Criar e aplicar migrations (primeira execução)
dotnet ef migrations add InitialCreate
dotnet ef database update

# Executar a API
dotnet run
```

**🌐 Backend estará disponível em:**

- API: `http://localhost:5021`
- Swagger UI: `http://localhost:5021/swagger`

**📊 Produção:**

- API: `https://petgo-production.up.railway.app`
- Swagger: `https://petgo-production.up.railway.app/swagger`

### 3️⃣ **Configurar e Executar o Frontend (Next.js)**

```bash
# Em um novo terminal, navegar para a pasta do frontend
cd src/frontend/petgo-frontend

# Instalar dependências
npm install

# Configurar variável de ambiente
# Crie o arquivo .env.local com:
# NEXT_PUBLIC_API_URL=http://localhost:5021

# Executar o frontend em modo desenvolvimento
npm run dev
```

**🌐 Frontend estará disponível em:**

- Desenvolvimento: `http://localhost:3000`
- Produção: `https://pet-go-puc.vercel.app`

---

## 🛠️ **Comandos Úteis**

### **Backend Commands**

```bash
# Restaurar dependências
dotnet restore

# Build do projeto
dotnet build

# Executar testes
dotnet test

# Limpar migrations (se necessário)
dotnet ef database drop --force
dotnet ef migrations remove --force

# Recriar banco de dados
dotnet ef migrations add InitialCreate_PetGo
dotnet ef database update

# Executar com hot reload
dotnet watch run
```

### **Frontend Commands**

```bash
# Instalar dependências
npm install

# Executar em desenvolvimento
npm run dev

# Build para produção
npm run build

# Executar em produção
npm start

# Lint do código
npm run lint

# Limpar cache do Next.js
rm -rf .next
```

---

## 📂 **Estrutura do Projeto**

```
PetGo/
├── 📁 docs/                          # Documentação do projeto
│   ├── personas.md                   # Personas do sistema
│   ├── funcionalidades.md            # Lista de funcionalidades
│   └── roteiro-de-implementacao-*.md # Roteiros de desenvolvimento
├── 📁 src/                          # Código fonte
│   ├── 📁 backend/petgo-api/        # API .NET Core
│   │   ├── Controllers/             # Controllers da API
│   │   ├── Data/                    # Contexto do banco de dados
│   │   ├── Models/                  # Modelos de entidade
│   │   ├── Services/                # Serviços de negócio
│   │   ├── Migrations/              # Migrations do EF Core
│   │   ├── Program.cs               # Configuração da aplicação
│   │   └── petgo.api.csproj        # Arquivo de projeto
│   └── 📁 frontend/petgo-frontend/  # Aplicação Next.js
│       ├── src/
│       │   ├── app/                 # App Router (Next.js 13+)
│       │   ├── components/          # Componentes React
│       │   ├── hooks/              # Hooks customizados
│       │   ├── lib/                # Utilitários e configurações
│       │   ├── services/           # Services para API
│       │   └── types/              # Types TypeScript
│       ├── public/                 # Arquivos estáticos
│       ├── package.json            # Dependências Node.js
│       └── next.config.ts          # Configuração Next.js
├── README.md                       # Este arquivo
└── LICENSE                        # Licença MIT
```

---

## 🔗 **Endpoints da API**

### **Produtos**

| Método | Endpoint             | Descrição                |
| ------ | -------------------- | ------------------------ |
| GET    | `/api/produtos`      | Listar todos os produtos |
| GET    | `/api/produtos/{id}` | Obter produto específico |
| POST   | `/api/produtos`      | Criar novo produto       |
| PUT    | `/api/produtos/{id}` | Atualizar produto        |
| DELETE | `/api/produtos/{id}` | Deletar produto          |

### **Usuários**

| Método | Endpoint             | Descrição                |
| ------ | -------------------- | ------------------------ |
| GET    | `/api/usuarios`      | Listar todos os usuários |
| GET    | `/api/usuarios/{id}` | Obter usuário específico |
| POST   | `/api/usuarios`      | Criar novo usuário       |
| PUT    | `/api/usuarios/{id}` | Atualizar usuário        |
| DELETE | `/api/usuarios/{id}` | Deletar usuário          |

### **Pets**

| Método | Endpoint         | Descrição            |
| ------ | ---------------- | -------------------- |
| GET    | `/api/pets`      | Listar todos os pets |
| GET    | `/api/pets/{id}` | Obter pet específico |
| POST   | `/api/pets`      | Criar novo pet       |
| PUT    | `/api/pets/{id}` | Atualizar pet        |
| DELETE | `/api/pets/{id}` | Deletar pet          |

---

## 🧪 **Testes**

### **Backend (Testes Unitários)**

```bash
cd src/backend/petgo-api
dotnet test
```

### **Frontend (Em desenvolvimento)**

```bash
cd src/frontend/petgo-frontend
npm test
```

---

## 🌐 **Tecnologias Utilizadas**

### **Backend**

- **ASP.NET Core 9.0** - Framework web
- **Entity Framework Core 9.0** - ORM
- **Npgsql** - PostgreSQL driver para .NET
- **Supabase PostgreSQL 15** - Banco de dados gerenciado
- **Swagger/OpenAPI** - Documentação da API
- **Railway** - Plataforma de deploy

### **Frontend**

- **Next.js 15** - Framework React com App Router
- **TypeScript 5** - Linguagem tipada
- **Tailwind CSS 4** - Framework CSS utilitário
- **React Query (TanStack Query v5)** - Gerenciamento de estado servidor
- **React Hook Form** - Gerenciamento de formulários
- **Zod** - Validação de schemas
- **Axios** - Cliente HTTP
- **Lucide React** - Ícones
- **Vercel** - Plataforma de deploy

### **DevOps & Qualidade**

- **ESLint** - Linting JavaScript/TypeScript
- **Prettier** - Formatação de código
- **Husky** - Git hooks
- **Docker** - Containerização (futuro)

---

## 🔧 **Configurações Específicas**

### **Variáveis de Ambiente**

#### **Frontend (.env.local)**

```env
# API Configuration
NEXT_PUBLIC_API_URL=http://localhost:5021

# Production
# NEXT_PUBLIC_API_URL=https://petgo-production.up.railway.app

# App Configuration
NEXT_PUBLIC_APP_NAME=PetGo
NEXT_PUBLIC_APP_VERSION=1.0.0

# Desenvolvimento
NODE_ENV=development
NEXT_TELEMETRY_DISABLED=1
```

#### **Backend (appsettings.json)**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=petgo;Username=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

#### **Produção (Railway Environment Variables)**

```env
# Supabase PostgreSQL Connection
ConnectionStrings__DefaultConnection=Host=aws-0-us-east-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.xxx;Password=xxx;SSL Mode=Require;Trust Server Certificate=true

# ASP.NET Configuration
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```

### **Configuração do Banco de Dados**

O projeto usa **Supabase PostgreSQL** em produção e pode usar PostgreSQL local em desenvolvimento.

#### **Supabase (Produção)**

1. Acesse [https://supabase.com](https://supabase.com)
2. Crie um novo projeto
3. Copie a **Connection String** (use o **Connection Pooler** para Railway)
4. Configure no Railway como variável de ambiente

#### **PostgreSQL Local (Desenvolvimento)**

```bash
# Instale o PostgreSQL
# Windows: https://www.postgresql.org/download/windows/
# Mac: brew install postgresql
# Linux: sudo apt-get install postgresql

# Crie o banco de dados
createdb petgo

# Configure a connection string no appsettings.Development.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=petgo;Username=postgres;Password=postgres"
  }
}
```

### Referências bibliográficas

- CAROLI, Paulo. **Lean Inception: Saiba como alinhar pessoas e construir o produto certo**. Disponível em: [https://caroli.org/lean-inception-3/](https://caroli.org/lean-inception-3/). Acessado em: 12 de agosto de 2024.
- BROWN, Simon. **O modelo C4 de documentação para Arquitetura de Software**. Disponível em: [https://www.infoq.com/br/articles/C4-architecture-model/](https://www.infoq.com/br/articles/C4-architecture-model/). Acessado em: 12 de agosto de 2024.
- DRESHER, Tamir; ZUKER, Amir; FRIEDMAN, Shay. **Hands-on full stack web development with ASP.NET Core:** learn end-to-end web development with leading frontend frameworks, such as Angular, React, and Vue. Birmingham, UK: Packt Publishing, 2018. E-book. ISBN 9781788622882.
