import api from "../lib/api";
import {
  Produto,
  CreateProdutoInput,
  UpdateProdutoInput,
  PaginatedResponse,
} from "../types";

class ProdutoService {
  private readonly baseUrl = "/api/produtos";

  async getAll(page = 1, pageSize = 10): Promise<Produto[]> {
    try {
      const response = await api.get(
        `${this.baseUrl}?page=${page}&pageSize=${pageSize}`
      );
      return response.data;
    } catch (error) {
      console.error("Erro ao buscar produtos:", error);
      throw error;
    }
  }

  async getById(id: number): Promise<Produto> {
    try {
      const response = await api.get(`${this.baseUrl}/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Erro ao buscar produto ${id}:`, error);
      throw error;
    }
  }

  async create(produto: CreateProdutoInput): Promise<Produto> {
    try {
      const response = await api.post(this.baseUrl, produto);
      return response.data;
    } catch (error) {
      console.error("Erro ao criar produto:", error);
      throw error;
    }
  }

  async update(id: number, produto: UpdateProdutoInput): Promise<Produto> {
    try {
      const response = await api.put(`${this.baseUrl}/${id}`, produto);
      return response.data;
    } catch (error) {
      console.error(`Erro ao atualizar produto ${id}:`, error);
      throw error;
    }
  }

  async delete(id: number): Promise<void> {
    try {
      await api.delete(`${this.baseUrl}/${id}`);
    } catch (error) {
      console.error(`Erro ao deletar produto ${id}:`, error);
      throw error;
    }
  }

  async searchByName(name: string): Promise<Produto[]> {
    try {
      const response = await api.get(
        `${this.baseUrl}/search?name=${encodeURIComponent(name)}`
      );
      return response.data;
    } catch (error) {
      console.error("Erro ao buscar produtos por nome:", error);
      throw error;
    }
  }

  async getByCategory(categoryId: number): Promise<Produto[]> {
    try {
      const response = await api.get(`${this.baseUrl}/category/${categoryId}`);
      return response.data;
    } catch (error) {
      console.error(
        `Erro ao buscar produtos da categoria ${categoryId}:`,
        error
      );
      throw error;
    }
  }
}

export const produtoService = new ProdutoService();
