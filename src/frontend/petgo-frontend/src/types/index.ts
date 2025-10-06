import { z } from "zod";

// Enums baseados no backend
export enum TipoUsuario {
  CLIENTE = 0,
  PASSEADOR = 1,
  ADMIN = 2,
}

export enum EspeciePet {
  CACHORRO = 0,
  GATO = 1,
  OUTRO = 2,
}

export enum PortePet {
  PEQUENO = 0,
  MEDIO = 1,
  GRANDE = 2,
}

export enum StatusProduto {
  ATIVO = 0,
  RASCUNHO = 1,
}

export enum StatusAnuncio {
  ATIVO = 0,
  PAUSADO = 1,
  ADOTADO = 2,
  REMOVIDO = 3,
}

export enum Moderacao {
  PENDENTE = 0,
  APROVADO = 1,
  REJEITADO = 2,
}

export enum AlvoTipo {
  PRODUTO = 0,
  PASSEADOR = 1,
  SERVICO = 2,
}

// Schemas Zod para validação
export const UsuarioSchema = z.object({
  id: z.number(),
  nome: z.string().min(1, "Nome é obrigatório"),
  email: z.string().email("Email inválido"),
  telefone: z.string().min(1, "Telefone é obrigatório"),
  tipo: z.nativeEnum(TipoUsuario),
  fotoPerfil: z.string(),
  dataCriacao: z.string(),
  enderecos: z.array(z.any()).optional(),
  pets: z.array(z.any()).optional(),
});

export const PetSchema = z.object({
  id: z.number(),
  usuarioId: z.number(),
  nome: z.string().min(1, "Nome é obrigatório"),
  especie: z.nativeEnum(EspeciePet),
  raca: z.string().min(1, "Raça é obrigatória"),
  idadeMeses: z.number().min(0, "Idade deve ser positiva"),
  porte: z.nativeEnum(PortePet),
  cidade: z.string().min(1, "Cidade é obrigatória"),
  estado: z.string().min(1, "Estado é obrigatório"),
  fotos: z.array(z.string()),
  observacoes: z.string(),
  usuario: UsuarioSchema.optional(),
  anuncioDoacao: z.any().optional(),
});

export const ProdutoSchema = z.object({
  id: z.number(),
  nome: z.string().min(1, "Nome é obrigatório"),
  descricao: z.string().min(1, "Descrição é obrigatória"),
  preco: z.number().min(0, "Preço deve ser positivo"),
  imagens: z.array(z.string()),
  categoriaId: z.number(),
  estoque: z.number().min(0, "Estoque deve ser positivo"),
  status: z.nativeEnum(StatusProduto),
  categoriaProdutoId: z.number(),
  categoriaProduto: z.any().optional(),
});

export const AnuncioDoacaoSchema = z.object({
  id: z.number(),
  petId: z.number(),
  status: z.nativeEnum(StatusAnuncio),
  descricao: z.string().min(1, "Descrição é obrigatória"),
  contatoWhatsapp: z.string().optional(),
  moderacao: z.nativeEnum(Moderacao),
  pet: PetSchema.optional(),
});

// Types inferidos dos schemas
export type Usuario = z.infer<typeof UsuarioSchema>;
export type Pet = z.infer<typeof PetSchema>;
export type Produto = z.infer<typeof ProdutoSchema>;
export type AnuncioDoacao = z.infer<typeof AnuncioDoacaoSchema>;

// Types para formulários
export type CreateProdutoInput = Omit<Produto, "id" | "categoriaProduto">;
export type UpdateProdutoInput = Partial<CreateProdutoInput>;
export type CreatePetInput = Omit<Pet, "id" | "usuario" | "anuncioDoacao">;
export type UpdatePetInput = Partial<CreatePetInput>;

// API Response types
export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
}

export interface ApiError {
  message: string;
  status: number;
  details?: any;
}

// Pagination
export interface PaginatedResponse<T> {
  data: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

// Loading states
export interface LoadingState {
  isLoading: boolean;
  error: string | null;
}
