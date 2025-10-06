"use client";

import React from "react";
import { useProdutos, useDeleteProduto } from "../../hooks/useProdutos";
import { ProdutoCard } from "../../components/ProdutoCard";
import { LoadingSpinner } from "../../components/ui/LoadingSpinner";
import { Button } from "../../components/ui/Button";
import { Plus } from "lucide-react";

export default function ProdutosPage() {
  const { data: produtos, isLoading, error } = useProdutos();
  const deleteMutation = useDeleteProduto();

  const handleDelete = (id: number) => {
    deleteMutation.mutate(id);
  };

  const handleEdit = (produto: any) => {
    console.log("Editar produto:", produto);
    // Implementar modal de edição futuramente
  };

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <LoadingSpinner size="lg" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
          <h3 className="font-semibold">Erro ao carregar produtos</h3>
          <p>{error.message}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 mb-2">Produtos</h1>
          <p className="text-gray-600">Gerencie o catálogo de produtos PetGo</p>
        </div>
        <Button>
          <Plus size={20} className="mr-2" />
          Novo Produto
        </Button>
      </div>

      {/* Products Grid */}
      {!produtos || produtos.length === 0 ? (
        <div className="text-center py-12">
          <div className="text-6xl mb-4">📦</div>
          <h3 className="text-xl font-semibold text-gray-900 mb-2">
            Nenhum produto encontrado
          </h3>
          <p className="text-gray-600 mb-6">
            Comece adicionando alguns produtos ao catálogo
          </p>
          <Button>
            <Plus size={20} className="mr-2" />
            Adicionar Primeiro Produto
          </Button>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {produtos.map((produto) => (
            <ProdutoCard
              key={produto.id}
              produto={produto}
              onEdit={handleEdit}
              onDelete={handleDelete}
              isDeleting={deleteMutation.isPending}
            />
          ))}
        </div>
      )}
    </div>
  );
}
