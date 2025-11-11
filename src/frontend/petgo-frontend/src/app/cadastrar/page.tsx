"use client";

import { Button } from "@/components/ui/Button";
import { CadastroFormData, useCadastroForm } from "./_components/CadastroForm";

export default function CadastroPage() {
  const form = useCadastroForm();

  const onFormSubmit = (data: CadastroFormData) => {
    const { confirmarSenha, ...apiData } = data;

    console.log(apiData);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-indigo-50 to-purple-50">
        <h1 className="text-4xl font-bold pt-6 bg-gradient-to-r from-blue-600 to-indigo-600 bg-clip-text text-transparent text-center ">
          Cadastre-se!
        </h1>
      <div className=" flex items-center justify-center p-4">
        <form
          onSubmit={form.handleSubmit(onFormSubmit)}
          className="w-full max-w-md p-6 bg-white rounded-xl shadow-lg space-y-4"
        >
          <div>
            <label className="block text-sm font-medium text-gray-900 mb-1">
              Nome Completo
            </label>
            <input
              {...form.register("nome")}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-gray-900"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-900 mb-1">
              Email
            </label>
            <input
              {...form.register("email")}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-gray-900"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-900 mb-1">
              Telefone
            </label>
            <input
              {...form.register("telefone")}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-gray-900"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-900 mb-1">
              Senha
            </label>
            <input
              {...form.register("senha")}
              type="password"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-gray-900"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-900 mb-1">
              Confirmar Senha
            </label>
            <input
              {...form.register("confirmarSenha")}
              type="password"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg text-gray-900"
            />
          </div>

          <Button
            type="submit"
            className="w-full  bg-gradient-to-r from-blue-500 to-indigo-600 text-white py-2 rounded-lg hover:to-indigo-700 shadow-lg"
          >
            Cadastrar
          </Button>
        </form>
      </div>
    </div>
  );
}
