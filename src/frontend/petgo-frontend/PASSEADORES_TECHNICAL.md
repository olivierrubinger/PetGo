# Documentação Técnica - Página de Passeadores

## Estrutura de Arquivos

```
src/
├── app/
│   └── passeadores/
│       ├── page.tsx                    # Página principal com Suspense
│       └── PasseadoresContent.tsx      # Lógica e UI principal
├── components/
│   ├── PasseadorCard.tsx               # Card de exibição
│   ├── PasseadorForm.tsx               # Formulário modal
│   └── PasseadorSkeleton.tsx           # Loading states
├── hooks/
│   └── usePasseadores.ts               # React Query hooks
├── services/
│   └── passeador.service.ts            # Chamadas HTTP
└── types/
    └── index.ts                        # TypeScript types (atualizado)
```

## Fluxo de Dados

```
┌─────────────────────────────────────────────────────────────┐
│                      PasseadoresPage                         │
│  (Suspense boundary + loading fallback)                     │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   PasseadoresContent                         │
│  - usePasseadores() → busca dados                           │
│  - useState para modals e filtros                           │
│  - Filtros client-side                                      │
└─────────────────────────────────────────────────────────────┘
                            │
         ┌──────────────────┼──────────────────┐
         ▼                  ▼                  ▼
   ┌──────────┐      ┌──────────┐      ┌──────────┐
   │ Card     │      │ Form     │      │ Skeleton │
   └──────────┘      └──────────┘      └──────────┘
         │                  │
         ▼                  ▼
   ┌──────────────────────────────────────────────┐
   │         usePasseadores hooks                 │
   │  - usePasseadores()                          │
   │  - usePasseador(id)                          │
   │  - useCreatePasseador()                      │
   │  - useUpdatePasseador()                      │
   │  - useDeletePasseador()                      │
   └──────────────────────────────────────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │   passeadorService         │
        │  - getAll()                │
        │  - getById()               │
        │  - search()                │
        │  - filterByService()       │
        │  - create()                │
        │  - update()                │
        │  - delete()                │
        └────────────────────────────┘
                     │
                     ▼
        ┌────────────────────────────┐
        │    API Backend             │
        │  /api/passeadores          │
        └────────────────────────────┘
```

## Gerenciamento de Estado

### React Query (TanStack Query)

#### Query Keys

```typescript
PASSEADOR_QUERY_KEYS = {
  all: ["passeadores"],
  lists: () => ["passeadores", "list"],
  details: () => ["passeadores", "detail"],
  detail: (id) => ["passeadores", "detail", id],
  search: (query) => ["passeadores", "search", query],
  byService: (tipo) => ["passeadores", "servico", tipo],
};
```

#### Cache Strategy

- **Listagem**: 5 minutos (staleTime)
- **Detalhes**: 5 minutos (staleTime)
- **Busca**: 2 minutos (staleTime)
- **Refetch**: Automático ao montar componente
- **Invalidação**: Após mutações (create, update, delete)

### Local State (useState)

```typescript
// Modal state
const [modal, setModal] = useState<ModalState>({
  isOpen: boolean,
  mode: "create" | "edit",
  passeador: Passeador,
});

// Filtros
const [searchTerm, setSearchTerm] = useState("");
const [filterServico, setFilterServico] = useState<TipoServico | "all">("all");
```

## Componentes Principais

### 1. PasseadorCard

**Props:**

```typescript
interface PasseadorCardProps {
  passeador: Passeador;
  onEdit?: (passeador: Passeador) => void;
  onDelete?: (id: number) => void;
  onContact?: (passeador: Passeador) => void;
}
```

**Funcionalidades:**

- Exibição de foto (SafeImage com fallback)
- Sistema de avaliação com estrelas
- Badge de status online/offline
- Tags de serviços oferecidos
- Botões de ação (Contatar, Editar, Excluir)
- Formatação de telefone brasileira
- Integração WhatsApp

### 2. PasseadorForm

**Props:**

```typescript
interface PasseadorFormProps {
  passeador?: Passeador;
  onSubmit: (data: PasseadorFormData) => void;
  onClose: () => void;
  isLoading?: boolean;
}
```

**Validação (Zod):**

```typescript
const passeadorFormSchema = z.object({
  usuarioId: z.number().min(1),
  descricao: z.string().min(20).max(1500),
  valorCobrado: z.number().min(1).max(10000),
});
```

**Comportamento:**

- Modal overlay com backdrop
- Validação em tempo real
- Mensagens de erro inline
- Estado de loading nos botões
- Auto-populate em modo edição

### 3. PasseadorSkeleton

**Componentes:**

- `PasseadorSkeleton`: skeleton individual
- `PasseadoresGridSkeleton`: grid de skeletons

**Uso:**

```tsx
<PasseadoresGridSkeleton count={6} />
```

## Hooks Customizados

### usePasseadores()

```typescript
// Listagem completa
const { data, isLoading, error } = usePasseadores();
```

### usePasseador(id)

```typescript
// Buscar específico
const { data: passeador } = usePasseador(id);
```

### useCreatePasseador()

```typescript
const createMutation = useCreatePasseador();

createMutation.mutate(data, {
  onSuccess: () => console.log("Criado!"),
  onError: (err) => console.error(err),
});
```

### useUpdatePasseador()

```typescript
const updateMutation = useUpdatePasseador();

updateMutation.mutate(
  { id: 1, data: { descricao: "Nova descrição" } },
  { onSuccess: () => console.log("Atualizado!") }
);
```

### useDeletePasseador()

```typescript
const deleteMutation = useDeletePasseador();

deleteMutation.mutate(id);
```

## Integração com Backend

### Endpoints Utilizados

| Método | Endpoint                               | Descrição           |
| ------ | -------------------------------------- | ------------------- |
| GET    | `/api/passeadores`                     | Listar todos        |
| GET    | `/api/passeadores/{id}`                | Buscar por ID       |
| GET    | `/api/passeadores/search?name={query}` | Buscar por nome     |
| GET    | `/api/passeadores/servico/{tipo}`      | Filtrar por serviço |
| POST   | `/api/passeadores`                     | Criar perfil        |
| PUT    | `/api/passeadores/{id}`                | Atualizar perfil    |
| DELETE | `/api/passeadores/{id}`                | Excluir perfil      |

### Modelos de Dados

**Request (Create/Update):**

```json
{
  "usuarioId": 1,
  "descricao": "Profissional experiente com 5 anos...",
  "valorCobrado": 50.0
}
```

**Response (PasseadorDto):**

```json
{
  "usuarioId": 1,
  "descricao": "Profissional experiente...",
  "valorCobrado": 50.0,
  "avaliacaoMedia": 4.8,
  "quantidadeAvaliacoes": 23,
  "nome": "João Silva",
  "fotoPerfil": "https://...",
  "telefone": "11987654321",
  "servicos": [
    {
      "id": 1,
      "titulo": "Passeio em parques",
      "descricao": "Passeios de 30min a 1h",
      "tipoServico": 0
    }
  ]
}
```

## Tratamento de Erros

### No Service Layer

```typescript
try {
  const response = await api.get<Passeador[]>(this.baseUrl);
  return response.data;
} catch (error) {
  console.error("Erro ao buscar passeadores:", error);
  throw error;
}
```

### Nos Hooks

```typescript
onError: (error: ApiError) => {
  const errorMessage =
    error?.response?.data?.message ||
    error?.message ||
    "Erro ao carregar passeadores";
  toast.error(errorMessage);
};
```

### No Component

```tsx
if (error) {
  return (
    <div className="bg-red-50 border border-red-200">
      <h3>Erro ao carregar passeadores</h3>
      <p>{error.message}</p>
    </div>
  );
}
```

## Funcionalidades Especiais

### 1. Contato via WhatsApp

```typescript
const handleContact = (passeador: Passeador) => {
  const telefone = passeador.telefone.replace(/\D/g, "");
  const mensagem = encodeURIComponent(
    `Olá ${passeador.nome}, vi seu perfil no PetGo...`
  );
  window.open(`https://wa.me/55${telefone}?text=${mensagem}`, "_blank");
};
```

### 2. Filtros Client-Side

```typescript
const filteredPasseadores = passeadores?.filter((passeador) => {
  const matchesSearch =
    passeador.nome.toLowerCase().includes(searchTerm.toLowerCase()) ||
    passeador.descricao.toLowerCase().includes(searchTerm.toLowerCase());

  const matchesServico =
    filterServico === "all" ||
    passeador.servicos?.some((s) => s.tipoServico === filterServico);

  return matchesSearch && matchesServico;
});
```

### 3. Sistema de Avaliação

```typescript
const renderStars = (rating: number) => {
  return (
    <div className="flex items-center gap-1">
      {[1, 2, 3, 4, 5].map((star) => (
        <Star
          key={star}
          className={
            star <= Math.round(rating)
              ? "fill-yellow-400 text-yellow-400"
              : "text-gray-300"
          }
        />
      ))}
    </div>
  );
};
```

## Performance

### Otimizações Implementadas

1. **Lazy Loading**: Suspense boundary na página
2. **Code Splitting**: Componentes separados
3. **Memoization**: React Query cache
4. **Skeleton Loading**: Feedback visual imediato
5. **Debounce**: Busca com delay (pode ser implementado)
6. **Pagination**: API suporta (pode ser adicionado)

### Bundle Size

Estimativa de impacto no bundle:

- `passeador.service.ts`: ~2KB
- `usePasseadores.ts`: ~5KB
- `PasseadorCard.tsx`: ~4KB
- `PasseadorForm.tsx`: ~6KB
- `PasseadorSkeleton.tsx`: ~2KB
- **Total**: ~19KB (gzipped: ~6KB)

## Acessibilidade

### Implementações

- Labels semânticos em formulários
- Botões com aria-labels implícitos
- Contraste de cores WCAG AA
- Navegação por teclado
- Focus states visíveis

### Melhorias Futuras

- [ ] ARIA live regions para toasts
- [ ] Keyboard shortcuts
- [ ] Screen reader announcements
- [ ] Skip links

## Testes Sugeridos

### Unit Tests

```typescript
// PasseadorCard.test.tsx
describe("PasseadorCard", () => {
  it("deve renderizar informações do passeador", () => {
    // ...
  });

  it("deve abrir WhatsApp ao clicar em Contatar", () => {
    // ...
  });
});
```

### Integration Tests

```typescript
// passeadores.test.tsx
describe("Página de Passeadores", () => {
  it("deve carregar lista de passeadores", async () => {
    // ...
  });

  it("deve filtrar passeadores por busca", () => {
    // ...
  });
});
```

### E2E Tests

```typescript
// passeadores.spec.ts (Playwright)
test("fluxo completo de criação de passeador", async ({ page }) => {
  // ...
});
```

## Logs e Debugging

### Console Logs Implementados

```typescript
console.log("✅ Passeadores carregados:", result);
console.log("✅ Passeador criado com sucesso:", newPasseador);
console.error("❌ Erro ao carregar passeadores:", error);
console.warn("⚠️ Já existe uma operação em andamento");
```

### React Query Devtools

```tsx
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

// Já configurado no Providers.tsx
```

## Segurança

### Validações

- Input sanitization (Zod)
- SQL Injection prevention (EF Core)
- XSS protection (React default)

### Autenticação

- JWT tokens (backend)
- Protected routes (pode ser adicionado)

## Deployment

### Checklist

- [x] TypeScript sem erros
- [x] ESLint sem warnings críticos
- [x] Build de produção testado
- [x] Variáveis de ambiente configuradas
- [x] CORS configurado no backend

### Build

```bash
npm run build
```

### Variáveis de Ambiente

```env
NEXT_PUBLIC_API_URL=https://petgo-production.up.railway.app
```

## Manutenção

### Pontos de Atenção

1. **API Changes**: sincronizar types com backend
2. **Cache**: limpar cache ao atualizar versão
3. **Images**: validar URLs de imagens
4. **WhatsApp**: verificar formatação de telefone

### Versionamento

- v1.0.0: Implementação inicial
- Próximas versões: ver PASSEADORES_README.md

---

**Desenvolvido seguindo as melhores práticas de:**

- Clean Code
- SOLID principles
- React best practices
- TypeScript strict mode
- Accessibility guidelines
