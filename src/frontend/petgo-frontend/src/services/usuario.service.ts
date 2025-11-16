import { CadastroFormData } from "@/app/cadastrar/_components/CadastroForm";
import api from "../lib/api"; 
import {
    Usuario, 
    LoginDto,
    LoginResponseDto,
    ApiError,
    TipoUsuario,
} from "../types"; 
import { UpdateProfileFormData } from "@/app/perfil/_components/PerfilForm";

export type CadastroApiPayload = {
  nome: string;
  email: string;
  telefone: string;
  senha: string;
  tipoUsuario: TipoUsuario;
  fotoPerfil: string | null; 
};

class UsuarioService {
    private readonly baseUrl = "/api/usuarios"; 

    async register(data: Omit<CadastroFormData, "confirmarSenha">): Promise<Usuario> {
        try {
            const response = await api.post(`${this.baseUrl}/registrar`, data); 
            return response.data; 
        } catch (error) {
            const apiError = error as ApiError;
            console.error("❌ Erro ao registrar usuário:", apiError);
            throw apiError; 
        }
    }

    async login(loginData: LoginDto): Promise<LoginResponseDto> {
        try {
            const response = await api.post(`${this.baseUrl}/login`, loginData);
         
            return response.data; 
        } catch (error) {
            const apiError = error as ApiError;
            console.error("❌ Erro no login:", apiError);
            throw apiError;
        }
    }

   async update(id: number, data: UpdateProfileFormData): Promise<Usuario> {
        try {
            // O endpoint é /api/Usuarios/{id} (PUT)
            const response = await api.put<Usuario>(`${this.baseUrl}/${id}`, data);
            
            return response.data; 
        } catch (error) {
            const apiError = error as ApiError;
            console.error("❌ Erro ao atualizar perfil:", apiError);
            throw apiError;
        }
    }
}
    

export const usuarioService = new UsuarioService();