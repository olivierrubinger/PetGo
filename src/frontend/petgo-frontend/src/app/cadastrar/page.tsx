"use client";

import { Button } from "@/components/ui/Button";
import { CadastroFormData, useCadastroForm } from "./_components/CadastroForm";
import { extractPhoneNumber } from "@/lib/utils";
import { FormField } from "@/components/ui/FormField";
import { PhoneField } from "@/components/ui/PhoneField";
import { Clipboard } from "lucide-react";
import { ProfilePhotoInput } from "@/components/ProfilePhotoInput";
import { TipoUsuario } from "@/types";
import { useCadastroUsuario } from "@/hooks/useUsuario";
import { useRouter } from "next/navigation";


export default function CadastroPage() {
  const form = useCadastroForm();
  const router = useRouter();

  const registerMutation = useCadastroUsuario();
  const fotoPerfilUrl = form.watch("fotoPerfil") as string | null;

  const onFormSubmit = async (data: CadastroFormData) => {
    const extractValue = extractPhoneNumber(data.telefone || "");
    const { confirmarSenha, ...apiData } = data;

    const finalApiData = {
      ...apiData,
      telefone: extractValue,
    };

    try {
      await registerMutation.mutateAsync(finalApiData);

      form.reset();
        setTimeout(() => {
        router.push("/login");
      }, 3000);
    } catch (err) {
      console.error("Erro capturado no componente:", err);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
      <div className="flex justify-center items-center pt-5 space-x-3">
        <div className="inline-flex items-center justify-center w-16 h-16 bg-gradient-to-r from-blue-500 to-indigo-600 rounded-full shadow-lg">
          <Clipboard size={32} className="text-white" />
        </div>

        <h1 className="text-4xl font-bold bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent text-center ">
          Cadastre-se!
        </h1>
      </div>

      <div className=" flex items-center justify-center p-4">
        <form
          onSubmit={form.handleSubmit(onFormSubmit)}
          className="w-full max-w-md p-6 bg-white rounded-xl shadow-lg space-y-4"
        >
          <ProfilePhotoInput
            name="fotoPerfil"
            currentUrl={fotoPerfilUrl}
            setValue={form.setValue}
          />

          <FormField
            label="Nome Completo"
            name="nome"
            register={form.register}
            errors={form.formState.errors}
            placeholder="Digite seu nome completo"
          />

          <FormField
            label="Email"
            name="email"
            type="email"
            register={form.register}
            errors={form.formState.errors}
            placeholder="example@email.com"
          />

          <PhoneField
            label="Telefone"
            name="telefone"
            control={form.control}
            errors={form.formState.errors}
          />

          <div>
            <label
              htmlFor="tipoUsuario"
              className="block text-sm font-medium text-gray-900 mb-1"
            >
              Tipo de Cadastro *
            </label>

            <select
              id="tipoUsuario"
              {...form.register("tipoUsuario", { valueAsNumber: true })}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-gray-900 focus:ring-2 focus:ring-blue-500"
            >
              <option value={TipoUsuario.CLIENTE}>Sou Cliente</option>

              <option value={TipoUsuario.PASSEADOR}>Sou Passeador</option>
            </select>

            {form.formState.errors.tipoUsuario && (
              <p className="mt-1 text-sm text-red-600">
                {form.formState.errors.tipoUsuario.message}
              </p>
            )}
          </div>

          <FormField
            label="Senha"
            name="senha"
            type="password"
            register={form.register}
            errors={form.formState.errors}
            placeholder="Digite uma senha"
          />

          <FormField
            label="Confirmar Senha"
            name="confirmarSenha"
            type="password"
            register={form.register}
            errors={form.formState.errors}
            placeholder="Confirmar senha"
          />

          <Button
            type="submit"
            className="w-full  bg-gradient-to-r from-blue-500 to-indigo-600 text-white py-2 rounded-lg hover:to-indigo-700 shadow-lg"
            disabled={registerMutation.isPending}
          >
            {registerMutation.isPending ? "Carregando..." : "Cadastrar"}
          </Button>
        </form>
      </div>
    </div>
  );
}
