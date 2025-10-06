"use client";

import React from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Button } from "./ui/Button";
import { LoadingSpinner } from "./ui/LoadingSpinner";
import { X, Upload, Trash2 } from "lucide-react";
import { StatusProduto } from "../types";

// Schema de validação
const produtoFormSchema = z.object({
  nome: z.string().min(1, "Nome é obrigatório").max(100, "Nome muito longo"),
  descricao: z
    .string()
    .min(1, "Descrição é obrigatória")
    .max(500, "Descrição muito longa"),
  preco: z.number().min(0.01, "Preço deve ser maior que zero"),
  estoque: z.number().int().min(0, "Estoque não pode ser negativo"),
  categoriaId: z.number().min(1, "Categoria é obrigatória"),
  categoriaProdutoId: z.number().min(1, "Categoria é obrigatória"),
  status: z.nativeEnum(StatusProduto),
  imagens: z.array(z.string()).optional(),
});

type ProdutoFormData = z.infer<typeof produtoFormSchema>;

interface ProdutoFormProps {
  produto?: any; // Para edição
  onSubmit: (data: ProdutoFormData) => void;
  onCancel: () => void;
  isLoading?: boolean;
  title: string;
}

// Categorias mock - depois substituir por API
const CATEGORIAS_MOCK = [
  { id: 1, nome: "Ração e Alimentação" },
  { id: 2, nome: "Brinquedos" },
  { id: 3, nome: "Acessórios" },
  { id: 4, nome: "Higiene e Cuidados" },
  { id: 5, nome: "Coleiras e Guias" },
];

export function ProdutoForm({
  produto,
  onSubmit,
  onCancel,
  isLoading,
  title,
}: ProdutoFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
    watch,
    setValue,
    reset,
  } = useForm<ProdutoFormData>({
    resolver: zodResolver(produtoFormSchema),
    defaultValues: produto
      ? {
          nome: produto.nome,
          descricao: produto.descricao,
          preco: produto.preco,
          estoque: produto.estoque,
          categoriaId: produto.categoriaId || produto.categoriaProdutoId,
          categoriaProdutoId: produto.categoriaProdutoId || produto.categoriaId,
          status: produto.status,
          imagens: produto.imagens || [],
        }
      : {
          nome: "",
          descricao: "",
          preco: 0,
          estoque: 0,
          categoriaId: 1,
          categoriaProdutoId: 1,
          status: StatusProduto.RASCUNHO,
          imagens: [],
        },
  });

  const imagens = watch("imagens") || [];

  const handleImageAdd = () => {
    const url = prompt("Digite a URL da imagem:");
    if (url && url.trim()) {
      setValue("imagens", [...imagens, url.trim()]);
    }
  };

  const handleImageRemove = (index: number) => {
    setValue(
      "imagens",
      imagens.filter((_, i) => i !== index)
    );
  };

  const onFormSubmit = (data: ProdutoFormData) => {
    // Garantir que ambos os campos de categoria tenham o mesmo valor
    const formattedData = {
      ...data,
      categoriaId: data.categoriaProdutoId,
    };
    onSubmit(formattedData);
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b">
          <h2 className="text-2xl font-bold text-gray-900">{title}</h2>
          <button
            onClick={onCancel}
            className="text-gray-400 hover:text-gray-600 transition-colors"
          >
            <X size={24} />
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit(onFormSubmit)} className="p-6 space-y-6">
          {/* Nome */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Nome do Produto *
            </label>
            <input
              {...register("nome")}
              type="text"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Ex: Ração Premium para Cães"
            />
            {errors.nome && (
              <p className="mt-1 text-sm text-red-600">{errors.nome.message}</p>
            )}
          </div>

          {/* Descrição */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Descrição *
            </label>
            <textarea
              {...register("descricao")}
              rows={4}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              placeholder="Descreva as características e benefícios do produto..."
            />
            {errors.descricao && (
              <p className="mt-1 text-sm text-red-600">
                {errors.descricao.message}
              </p>
            )}
          </div>

          {/* Preço e Estoque */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Preço (R$) *
              </label>
              <input
                {...register("preco", { valueAsNumber: true })}
                type="number"
                step="0.01"
                min="0"
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="0,00"
              />
              {errors.preco && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.preco.message}
                </p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Estoque *
              </label>
              <input
                {...register("estoque", { valueAsNumber: true })}
                type="number"
                min="0"
                className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                placeholder="0"
              />
              {errors.estoque && (
                <p className="mt-1 text-sm text-red-600">
                  {errors.estoque.message}
                </p>
              )}
            </div>
          </div>

          {/* Categoria */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Categoria *
            </label>
            <select
              {...register("categoriaProdutoId", { valueAsNumber: true })}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              {CATEGORIAS_MOCK.map((categoria) => (
                <option key={categoria.id} value={categoria.id}>
                  {categoria.nome}
                </option>
              ))}
            </select>
            {errors.categoriaProdutoId && (
              <p className="mt-1 text-sm text-red-600">
                {errors.categoriaProdutoId.message}
              </p>
            )}
          </div>

          {/* Status */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Status *
            </label>
            <select
              {...register("status", { valueAsNumber: true })}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value={StatusProduto.RASCUNHO}>Rascunho</option>
              <option value={StatusProduto.ATIVO}>Ativo</option>
            </select>
            {errors.status && (
              <p className="mt-1 text-sm text-red-600">
                {errors.status.message}
              </p>
            )}
          </div>

          {/* Imagens */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Imagens do Produto
            </label>
            <div className="space-y-3">
              {imagens.length > 0 && (
                <div className="grid grid-cols-2 gap-3">
                  {imagens.map((url, index) => (
                    <div key={index} className="relative group">
                      <img
                        src={url}
                        alt={`Produto ${index + 1}`}
                        className="w-full h-24 object-cover rounded-lg border"
                        onError={(e) => {
                          e.currentTarget.src =
                            "data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iMjQiIGhlaWdodD0iMjQiIHZpZXdCb3g9IjAgMCAyNCAyNCIgZmlsbD0ibm9uZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj4KPHJlY3QgeD0iMyIgeT0iMyIgd2lkdGg9IjE4IiBoZWlnaHQ9IjE4IiByeD0iMiIgZmlsbD0iI0Y5RkFGQiIgc3Ryb2tlPSIjRDFENUQ4IiBzdHJva2Utd2lkdGg9IjIiLz4KPHN2ZyB4PSI4IiB5PSI4IiB3aWR0aD0iOCIgaGVpZ2h0PSI4IiB2aWV3Qm94PSIwIDAgMjQgMjQiIGZpbGw9Im5vbmUiPgo8cGF0aCBkPSJtMjEgMTktNy01LTMuNSAzLjUtOS0xMVY3YTIgMiAwIDAgMSAyLTJoMTZhMiAyIDAgMCAxIDIgMnoiIHN0cm9rZT0iIzlDQTNBRiIgc3Ryb2tlLXdpZHRoPSIyIiBzdHJva2UtbGluZWNhcD0icm91bmQiIHN0cm9rZS1saW5lam9pbj0icm91bmQiLz4KPC9zdmc+Cjwvc3ZnPgo=";
                        }}
                      />
                      <button
                        type="button"
                        onClick={() => handleImageRemove(index)}
                        className="absolute top-1 right-1 bg-red-500 text-white rounded-full p-1 opacity-0 group-hover:opacity-100 transition-opacity"
                      >
                        <Trash2 size={12} />
                      </button>
                    </div>
                  ))}
                </div>
              )}
              <button
                type="button"
                onClick={handleImageAdd}
                className="w-full border-2 border-dashed border-gray-300 rounded-lg p-4 text-center hover:border-gray-400 transition-colors"
              >
                <Upload size={24} className="mx-auto mb-2 text-gray-400" />
                <span className="text-sm text-gray-600">
                  Clique para adicionar imagem (URL)
                </span>
              </button>
            </div>
          </div>

          {/* Actions */}
          <div className="flex justify-end gap-3 pt-6 border-t">
            <Button
              type="button"
              variant="secondary"
              onClick={onCancel}
              disabled={isLoading}
            >
              Cancelar
            </Button>
            <Button type="submit" loading={isLoading} disabled={isLoading}>
              {produto ? "Atualizar Produto" : "Criar Produto"}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
