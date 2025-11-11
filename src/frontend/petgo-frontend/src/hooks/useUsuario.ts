import { useMutation} from "@tanstack/react-query";
import { usuarioService } from "../services/usuario.service"; 
import { Usuario, ApiError } from "../types"; 
import { CadastroFormData } from "@/app/cadastrar/_components/CadastroForm";
import { toast } from "sonner";

export const USUARIO_QUERY_KEYS = {
    all: ["usuarios"] as const,
    login: ["login"] as const, 
};


export function useCadastroUsuario() {

    return useMutation({
        mutationFn: (data: Omit<CadastroFormData, "confirmarSenha">) => 
            usuarioService.register(data),
        
        onSuccess: (newUsuario: Usuario) => {
            console.log("✅ Usuário registrado com sucesso:", newUsuario.nome);

            toast.success(`Bem-vindo(a), ${newUsuario.nome}! Cadastro concluído.`);
        
        },
        
        onError: (error: ApiError) => {
            console.error("🚨 Erro de Registro (API):", error);
            
            const errorMessage = 
                error.response?.data?.message || 
                error.message || 
                "Ocorreu um erro no servidor ao tentar registrar.";
                
            toast.error(errorMessage);
            
            throw new Error(errorMessage);
        },
    });
}