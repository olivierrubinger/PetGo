import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { produtoService } from "../services/produto.service";
import { Produto, CreateProdutoInput, UpdateProdutoInput } from "../types";
import { toast } from "../lib/toast";

// Query Keys para consistência
export const PRODUTO_QUERY_KEYS = {
  all: ["produtos"] as const,
  lists: () => [...PRODUTO_QUERY_KEYS.all, "list"] as const,
  list: (filters: string) =>
    [...PRODUTO_QUERY_KEYS.lists(), { filters }] as const,
  details: () => [...PRODUTO_QUERY_KEYS.all, "detail"] as const,
  detail: (id: number) => [...PRODUTO_QUERY_KEYS.details(), id] as const,
  search: (query: string) =>
    [...PRODUTO_QUERY_KEYS.all, "search", query] as const,
};

// Hook para listar produtos
export function useProdutos(page = 1, pageSize = 10) {
  return useQuery({
    queryKey: PRODUTO_QUERY_KEYS.list(`page-${page}-size-${pageSize}`),
    queryFn: () => produtoService.getAll(page, pageSize),
    staleTime: 1000 * 60 * 5, // 5 minutos
    gcTime: 1000 * 60 * 10, // 10 minutos (era cacheTime)
  });
}

// Hook para um produto específico
export function useProduto(id: number) {
  return useQuery({
    queryKey: PRODUTO_QUERY_KEYS.detail(id),
    queryFn: () => produtoService.getById(id),
    enabled: !!id,
    staleTime: 1000 * 60 * 5,
  });
}

// Hook para buscar produtos
export function useSearchProdutos(searchTerm: string) {
  return useQuery({
    queryKey: PRODUTO_QUERY_KEYS.search(searchTerm),
    queryFn: () => produtoService.searchByName(searchTerm),
    enabled: searchTerm.length > 2,
    staleTime: 1000 * 60 * 2, // 2 minutos para search
  });
}

// Hook para criar produto
export function useCreateProduto() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (produto: CreateProdutoInput) => produtoService.create(produto),
    onSuccess: (newProduto) => {
      // Invalidar queries relacionadas
      queryClient.invalidateQueries({ queryKey: PRODUTO_QUERY_KEYS.all });

      // Adicionar o novo produto ao cache
      queryClient.setQueryData(
        PRODUTO_QUERY_KEYS.detail(newProduto.id),
        newProduto
      );

      toast.success("Produto criado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao criar produto");
    },
  });
}

// Hook para atualizar produto
export function useUpdateProduto() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({
      id,
      produto,
    }: {
      id: number;
      produto: UpdateProdutoInput;
    }) => produtoService.update(id, produto),
    onSuccess: (updatedProduto, variables) => {
      // Atualizar cache específico
      queryClient.setQueryData(
        PRODUTO_QUERY_KEYS.detail(variables.id),
        updatedProduto
      );

      // Invalidar listas
      queryClient.invalidateQueries({ queryKey: PRODUTO_QUERY_KEYS.lists() });

      toast.success("Produto atualizado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao atualizar produto");
    },
  });
}

// Hook para deletar produto
export function useDeleteProduto() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => produtoService.delete(id),
    onSuccess: (_, deletedId) => {
      // Remover do cache
      queryClient.removeQueries({
        queryKey: PRODUTO_QUERY_KEYS.detail(deletedId),
      });

      // Invalidar listas
      queryClient.invalidateQueries({ queryKey: PRODUTO_QUERY_KEYS.lists() });

      toast.success("Produto deletado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao deletar produto");
    },
  });
}

// Hook combinado para operações CRUD
export function useProdutoOperations() {
  const createMutation = useCreateProduto();
  const updateMutation = useUpdateProduto();
  const deleteMutation = useDeleteProduto();

  return {
    create: createMutation.mutate,
    update: updateMutation.mutate,
    delete: deleteMutation.mutate,
    isCreating: createMutation.isPending,
    isUpdating: updateMutation.isPending,
    isDeleting: deleteMutation.isPending,
    isLoading:
      createMutation.isPending ||
      updateMutation.isPending ||
      deleteMutation.isPending,
  };
}
