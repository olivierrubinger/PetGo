"use client"

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";

// Schema de validação
const cadastroFormSchema = z.object({
    nome: z.string().min(1, "Nome é obrigatório").max(100, "Nome muito longo"),
    email: z.email().min(1, "O formato do e-mail é inválido"),
    telefone: z.string()
    .min(15, "O telefone é obrigatório (com DDD)"),
    senha: z.string()
    .min(6, "A senha deve ter no mínimo 6 caracteres"),
    confirmarSenha: z
      .string()
      .min(6, "A confirmação da senha é obrigatória"),
})
.refine((data) => data.senha === data.confirmarSenha, {
    message: "As senhas não conferem",
    path: ["confirmarSenha"], 
  });

export type CadastroFormData = z.infer<typeof cadastroFormSchema>;

export type CadastroApiData = Omit<CadastroFormData, "confirmarSenha">;

export interface CadastroFormProps {
  onSubmit: (data: CadastroApiData) => void;
  isLoading?: boolean;
  title?: string;
}

export function useCadastroForm() {
  return useForm<CadastroFormData>({
    resolver: zodResolver(cadastroFormSchema),
    defaultValues: {
      nome: "",
      email: "",
      telefone: "",
      senha: "",
      confirmarSenha: ""
    }
  })
}