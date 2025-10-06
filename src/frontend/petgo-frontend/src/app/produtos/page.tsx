"use client";

import React, { useState, useEffect } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import {
  useProdutos,
  useDeleteProduto,
  useCreateProduto,
  useUpdateProduto,
  useProduto,
} from "../../hooks/useProdutos";
import { ProdutoCard } from "../../components/ProdutoCard";
import { ProdutoForm } from "../../components/ProdutoForm";
import { LoadingSpinner } from "../../components/ui/LoadingSpinner";
import { Button } from "../../components/ui/Button";
import { Plus, Search, Filter } from "lucide-react";
import { Produto } from "../../types";

type ModalState = {
  isOpen: boolean;
  mode: "create" | "edit";
  produto?: Produto;
};

export default function ProdutosPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const editId = searchParams.get("edit");

  const { data: produtos, isLoading, error } = useProdutos();
  const deleteMutation = useDeleteProduto();
  const createMutation = useCreateProduto();
  const updateMutation = useUpdateProduto();

  // Buscar produto para edição se editId estiver presente
  const { data: produtoParaEdicao } = useProduto(editId ? parseInt(editId) : 0);

  const [modal, setModal] = useState<ModalState>({
    isOpen: false,
    mode: "create",
  });

  const [searchTerm, setSearchTerm] = useState("");
  const [filterStatus, setFilterStatus] = useState<
    "all" | "ativo" | "rascunho"
  >("all");

  // Effect para abrir modal de edição quando editId mudar
  useEffect(() => {
    if (editId && produtoParaEdicao) {
      setModal({
        isOpen: true,
        mode: "edit",
        produto: produtoParaEdicao,
      });
    }
  }, [editId, produtoParaEdicao]);

  // Filtrar produtos
  const filteredProdutos = produtos?.filter((produto) => {
    const matchesSearch =
      produto.nome.toLowerCase().includes(searchTerm.toLowerCase()) ||
      produto.descricao.toLowerCase().includes(searchTerm.toLowerCase());

    const matchesStatus =
      filterStatus === "all" ||
      (filterStatus === "ativo" && produto.status === 0) ||
      (filterStatus === "rascunho" && produto.status === 1);

    return matchesSearch && matchesStatus;
  });

  const handleCreate = () => {
    setModal({
      isOpen: true,
      mode: "create",
    });
  };

  const handleEdit = (produto: Produto) => {
    setModal({
      isOpen: true,
      mode: "edit",
      produto,
    });
  };

  const handleDelete = (id: number) => {
    if (window.confirm("Tem certeza que deseja excluir este produto?")) {
      deleteMutation.mutate(id);
    }
  };

  const handleModalClose = () => {
    setModal({
      isOpen: false,
      mode: "create",
    });

    // Limpar query params se estiver editando
    if (editId) {
      router.replace("/produtos");
    }
  };

  const handleFormSubmit = (data: any) => {
    if (modal.mode === "create") {
      createMutation.mutate(data, {
        onSuccess: () => {
          handleModalClose();
        },
      });
    } else if (modal.mode === "edit" && modal.produto) {
      updateMutation.mutate(
        { id: modal.produto.id, data },
        {
          onSuccess: () => {
            handleModalClose();
          },
        }
      );
    }
  };

  if (isLoading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <LoadingSpinner size="lg" />
          <p className="mt-4 text-gray-600">Carregando produtos...</p>
        </div>
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
          <p className="text-gray-600">
            Gerencie o catálogo de produtos PetGo ({produtos?.length || 0}{" "}
            produtos)
          </p>
        </div>
        <Button onClick={handleCreate}>
          <Plus size={20} className="mr-2" />
          Novo Produto
        </Button>
      </div>

      {/* Filters */}
      <div className="bg-white rounded-lg shadow-sm border p-4 mb-6">
        <div className="flex flex-col md:flex-row gap-4">
          {/* Search */}
          <div className="flex-1">
            <div className="relative">
              <Search
                size={20}
                className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400"
              />
              <input
                type="text"
                placeholder="Buscar produtos..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              />
            </div>
          </div>

          {/* Status Filter */}
          <div className="md:w-48">
            <div className="relative">
              <Filter
                size={20}
                className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400"
              />
              <select
                value={filterStatus}
                onChange={(e) => setFilterStatus(e.target.value as any)}
                className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent appearance-none"
              >
                <option value="all">Todos os Status</option>
                <option value="ativo">Apenas Ativos</option>
                <option value="rascunho">Apenas Rascunhos</option>
              </select>
            </div>
          </div>
        </div>
      </div>

      {/* Products Grid */}
      {!filteredProdutos || filteredProdutos.length === 0 ? (
        <div className="text-center py-12">
          <div className="text-6xl mb-4">
            {searchTerm || filterStatus !== "all" ? "🔍" : "📦"}
          </div>
          <h3 className="text-xl font-semibold text-gray-900 mb-2">
            {searchTerm || filterStatus !== "all"
              ? "Nenhum produto encontrado"
              : "Nenhum produto cadastrado"}
          </h3>
          <p className="text-gray-600 mb-6">
            {searchTerm || filterStatus !== "all"
              ? "Tente ajustar os filtros de busca"
              : "Comece adicionando alguns produtos ao catálogo"}
          </p>
          {!searchTerm && filterStatus === "all" && (
            <Button onClick={handleCreate}>
              <Plus size={20} className="mr-2" />
              Adicionar Primeiro Produto
            </Button>
          )}
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {filteredProdutos.map((produto) => (
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

      {/* Modal de Produto */}
      {modal.isOpen && (
        <ProdutoForm
          produto={modal.produto}
          onSubmit={handleFormSubmit}
          onCancel={handleModalClose}
          isLoading={createMutation.isPending || updateMutation.isPending}
          title={modal.mode === "create" ? "Novo Produto" : "Editar Produto"}
        />
      )}
    </div>
  );
}
