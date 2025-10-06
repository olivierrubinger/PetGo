"use client";

import React from "react";
import Image from "next/image";
import { Produto, StatusProduto } from "../types";
import { Button } from "./ui/Button";
import { formatCurrency, truncateText } from "../lib/utils";
import { Edit, Trash2, Package } from "lucide-react";

interface ProdutoCardProps {
  produto: Produto;
  onEdit?: (produto: Produto) => void;
  onDelete?: (id: number) => void;
  isDeleting?: boolean;
  showActions?: boolean;
}

export function ProdutoCard({
  produto,
  onEdit,
  onDelete,
  isDeleting = false,
  showActions = true,
}: ProdutoCardProps) {
  const handleDelete = () => {
    if (window.confirm("Tem certeza que deseja excluir este produto?")) {
      onDelete?.(produto.id);
    }
  };

  const getStatusBadge = (status: StatusProduto) => {
    const baseClasses = "px-2 py-1 rounded-full text-xs font-medium";

    switch (status) {
      case StatusProduto.ATIVO:
        return `${baseClasses} bg-green-100 text-green-800`;
      case StatusProduto.RASCUNHO:
        return `${baseClasses} bg-gray-100 text-gray-800`;
      default:
        return `${baseClasses} bg-gray-100 text-gray-800`;
    }
  };

  const getStatusText = (status: StatusProduto) => {
    switch (status) {
      case StatusProduto.ATIVO:
        return "Ativo";
      case StatusProduto.RASCUNHO:
        return "Rascunho";
      default:
        return "Desconhecido";
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-200 border border-gray-200">
      {/* Imagem do produto */}
      <div className="relative h-48 bg-gray-100">
        {produto.imagens && produto.imagens.length > 0 ? (
          <Image
            src={produto.imagens[0]}
            alt={produto.nome}
            fill
            className="object-cover"
            sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
          />
        ) : (
          <div className="flex items-center justify-center h-full text-gray-400">
            <Package className="h-12 w-12" />
          </div>
        )}

        {/* Badge de status */}
        <div className="absolute top-2 right-2">
          <span className={getStatusBadge(produto.status)}>
            {getStatusText(produto.status)}
          </span>
        </div>
      </div>

      {/* Conteúdo do card */}
      <div className="p-4">
        <div className="flex justify-between items-start mb-2">
          <h3 className="text-lg font-semibold text-gray-900 line-clamp-1">
            {produto.nome}
          </h3>
        </div>

        <p className="text-gray-600 text-sm mb-3 line-clamp-2">
          {truncateText(produto.descricao, 100)}
        </p>

        <div className="flex justify-between items-center mb-4">
          <div>
            <span className="text-2xl font-bold text-green-600">
              {formatCurrency(produto.preco)}
            </span>
          </div>
          <div className="text-right">
            <p className="text-sm text-gray-500">
              Estoque: <span className="font-medium">{produto.estoque}</span>
            </p>
          </div>
        </div>

        {/* Ações - CORRIGIDO */}
        {showActions && (
          <div className="flex gap-2">
            <Button
              variant="secondary"
              size="sm"
              onClick={() => onEdit?.(produto)}
              className="flex-1"
            >
              <Edit className="h-4 w-4 mr-2" />
              Editar
            </Button>
            <Button
              variant="danger" 
              size="sm"
              onClick={handleDelete}
              loading={isDeleting} 
              className="flex-1"
            >
              <Trash2 className="h-4 w-4 mr-2" />
              Excluir
            </Button>
          </div>
        )}
      </div>
    </div>
  );
}
