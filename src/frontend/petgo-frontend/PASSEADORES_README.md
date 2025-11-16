# Funcionalidade de Passeadores

Esta funcionalidade permite gerenciar perfis de passeadores de pets na plataforma PetGo.

## Arquivos Criados

### Backend (já existente)

- `Models/Passeador.cs` - Modelo de dados do passeador
- `Controllers/PasseadoresController.cs` - Endpoints da API
- `Dtos/Passeador/PasseadorDto.cs` - DTO de resposta
- `Dtos/Passeador/PasseadorCreateDto.cs` - DTO de criação/atualização

### Frontend (novos arquivos)

#### Services

- `src/services/passeador.service.ts` - Serviço de comunicação com a API

#### Hooks

- `src/hooks/usePasseadores.ts` - Hooks React Query para gerenciamento de estado

#### Components

- `src/components/PasseadorCard.tsx` - Card de exibição do passeador
- `src/components/PasseadorForm.tsx` - Formulário de criação/edição
- `src/components/PasseadorSkeleton.tsx` - Loading skeletons

#### Pages

- `src/app/passeadores/page.tsx` - Página principal com Suspense
- `src/app/passeadores/PasseadoresContent.tsx` - Componente principal com lógica

#### Types

- Adicionado em `src/types/index.ts`:
  - `Passeador` (alias de PasseadorDto)
  - `CreatePasseadorInput`
  - `UpdatePasseadorInput`

## Funcionalidades Implementadas

### 1. Listagem de Passeadores

- Grid responsivo (1 coluna mobile, 2 tablet, 3 desktop)
- Card com foto, nome, avaliação, preço e serviços
- Skeleton loading durante carregamento

### 2. Busca e Filtros

- Busca por nome ou descrição
- Filtro por tipo de serviço (Passeio, Cuidado Diário, Hospedagem, Outro)
- Filtros aplicados localmente (client-side) para melhor performance

### 3. Criação de Perfil

- Formulário com validação Zod
- Campos:
  - ID do Usuário (obrigatório na criação)
  - Descrição (20-1500 caracteres)
  - Valor cobrado por hora (R$ 1,00 - R$ 10.000,00)
- Toast de sucesso/erro

### 4. Edição de Perfil

- Modal reutilizado do formulário de criação
- Campos editáveis: descrição e valor cobrado
- ID do usuário não pode ser alterado

### 5. Exclusão de Perfil

- Confirmação antes de excluir
- Invalidação de cache após exclusão
- Toast de confirmação

### 6. Contato via WhatsApp

- Botão "Contatar" que abre o WhatsApp
- Mensagem pré-formatada
- Formatação automática do número de telefone

## Endpoints da API

```
GET    /api/passeadores              - Listar todos
GET    /api/passeadores/{id}         - Buscar por ID
GET    /api/passeadores/search?name= - Buscar por nome
GET    /api/passeadores/servico/{tipo} - Filtrar por serviço
POST   /api/passeadores              - Criar perfil
PUT    /api/passeadores/{id}         - Atualizar perfil
DELETE /api/passeadores/{id}         - Excluir perfil
```

## Validações

### Backend (C#)

- UsuarioId: obrigatório
- Descrição: 20-1500 caracteres
- ValorCobrado: R$ 1,00 - R$ 10.000,00

### Frontend (Zod)

- Mesmas validações do backend
- Validação em tempo real no formulário
- Mensagens de erro amigáveis

## Design System

### Cores

- Primária: Blue 600 → Indigo 600 (gradient)
- Secundária: Gray 200
- Sucesso: Green 500
- Erro: Red 600

### Componentes

- Seguem o mesmo padrão visual de Produtos
- Cards com hover effect
- Botões com gradient
- Skeleton loading consistente

## Como Usar

### Acessar a página

```
http://localhost:3000/passeadores
```

### Criar um perfil de passeador

1. Clique em "Adicionar"
2. Preencha o ID do usuário
3. Escreva uma descrição detalhada
4. Defina o valor por hora
5. Clique em "Criar Perfil"

### Editar um perfil

1. Clique em "Editar" no card do passeador
2. Modifique a descrição ou valor
3. Clique em "Atualizar"

### Contatar um passeador

1. Clique em "Contatar" no card
2. Será redirecionado para o WhatsApp
3. Mensagem pré-formatada será inserida

### Excluir um perfil

1. Clique no ícone de lixeira
2. Confirme a exclusão
3. Perfil será removido

## Cache e Performance

### React Query

- Cache de 5 minutos para listagem
- Refetch automático ao montar componente
- Invalidação inteligente após mutações
- Retry automático em caso de erro (max 2 tentativas)

### Otimizações

- Filtros aplicados no client-side
- Lazy loading com Suspense
- Skeleton loading para melhor UX
- Imagens com SafeImage component

## Integração com Navegação

O link "Passeadores" foi adicionado ao menu principal:

- Desktop: menu horizontal
- Mobile: menu hambúrguer

## Próximas Melhorias Sugeridas

1. **Avaliações**: permitir usuários avaliarem passeadores
2. **Serviços**: CRUD de serviços oferecidos pelo passeador
3. **Agenda**: visualização de disponibilidade
4. **Galeria**: múltiplas fotos do passeador com pets
5. **Localização**: mapa com passeadores próximos
6. **Favoritos**: salvar passeadores favoritos
7. **Chat**: comunicação in-app

## Testes

Para testar localmente:

```bash
# Backend
cd C:\GitClones\PetGo\src\backend\petgo-api
dotnet run

# Frontend
cd C:\GitClones\PetGo\src\frontend\petgo-frontend
npm run dev
```

Acesse: http://localhost:3000/passeadores

## Troubleshooting

### Erro: "Passeador não encontrado"

- Verifique se o ID do usuário existe na tabela Usuarios
- Confirme que o backend está rodando

### Erro: "Validation failed"

- Verifique se a descrição tem 20-1500 caracteres
- Confirme que o valor está entre R$ 1,00 e R$ 10.000,00

### Imagem não carrega

- O componente SafeImage usa fallback automático
- Verifique a URL da foto do perfil do usuário

## Compatibilidade

- Next.js 15.5.4
- React 19.1.0
- TypeScript 5
- React Query (TanStack Query) 5.90.2
- Tailwind CSS 4
- ASP.NET Core 9.0 (backend)
