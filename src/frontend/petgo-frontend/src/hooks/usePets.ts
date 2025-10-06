import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { petService } from "../services/pet.service";
import { Pet, CreatePetInput, UpdatePetInput } from "../types";
import { toast } from "../lib/toast";

// Query Keys
export const PET_QUERY_KEYS = {
  all: ["pets"] as const,
  lists: () => [...PET_QUERY_KEYS.all, "list"] as const,
  list: (filters: string) => [...PET_QUERY_KEYS.lists(), { filters }] as const,
  details: () => [...PET_QUERY_KEYS.all, "detail"] as const,
  detail: (id: number) => [...PET_QUERY_KEYS.details(), id] as const,
  byUser: (userId: number) => [...PET_QUERY_KEYS.all, "user", userId] as const,
};

// Hook para listar pets
export function usePets() {
  return useQuery({
    queryKey: PET_QUERY_KEYS.lists(),
    queryFn: petService.getAll,
    staleTime: 1000 * 60 * 5,
  });
}

// Hook para um pet específico
export function usePet(id: number) {
  return useQuery({
    queryKey: PET_QUERY_KEYS.detail(id),
    queryFn: () => petService.getById(id),
    enabled: !!id,
  });
}

// Hook para pets de um usuário
export function usePetsByUser(userId: number) {
  return useQuery({
    queryKey: PET_QUERY_KEYS.byUser(userId),
    queryFn: () => petService.getByUsuarioId(userId),
    enabled: !!userId,
  });
}

// Hook para criar pet
export function useCreatePet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (pet: CreatePetInput) => petService.create(pet),
    onSuccess: (newPet) => {
      queryClient.invalidateQueries({ queryKey: PET_QUERY_KEYS.all });
      queryClient.setQueryData(PET_QUERY_KEYS.detail(newPet.id), newPet);
      toast.success("Pet cadastrado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao cadastrar pet");
    },
  });
}

// Hook para atualizar pet
export function useUpdatePet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, pet }: { id: number; pet: UpdatePetInput }) =>
      petService.update(id, pet),
    onSuccess: (updatedPet, variables) => {
      queryClient.setQueryData(PET_QUERY_KEYS.detail(variables.id), updatedPet);
      queryClient.invalidateQueries({ queryKey: PET_QUERY_KEYS.lists() });
      toast.success("Pet atualizado com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao atualizar pet");
    },
  });
}

// Hook para deletar pet
export function useDeletePet() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => petService.delete(id),
    onSuccess: (_, deletedId) => {
      queryClient.removeQueries({ queryKey: PET_QUERY_KEYS.detail(deletedId) });
      queryClient.invalidateQueries({ queryKey: PET_QUERY_KEYS.lists() });
      toast.success("Pet removido com sucesso!");
    },
    onError: (error: any) => {
      toast.error(error.message || "Erro ao remover pet");
    },
  });
}

// Hook combinado para operações CRUD
export function usePetOperations() {
  const createMutation = useCreatePet();
  const updateMutation = useUpdatePet();
  const deleteMutation = useDeletePet();

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
