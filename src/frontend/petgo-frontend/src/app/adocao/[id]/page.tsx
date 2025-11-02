"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import Image from "next/image";
import axios from "axios";
import { LoadingSpinner } from "@/components/ui/LoadingSpinner";

interface Pet {
  id: number;
  nome: string;
  descricao: string;
  imagens: string[];
  especie?: string;
  raca?: string;
  castrado?: boolean;
  vacinado?: boolean;
  idadeMeses?: number;
  porte?: string;
}

export default function PetDetalhes() {
  const params = useParams();
  const id = params?.id as string;
  const [pet, setPet] = useState<Pet | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;

    setLoading(true);
    axios
      .get(`${process.env.NEXT_PUBLIC_API_URL}/api/Produtos/${id}`)
      .then((res) => {
        setPet(res.data);
        setLoading(false);
      })
      .catch((err) => {
        console.error("Erro ao carregar pet:", err);
        setError(
          err?.response?.data?.message || "Erro ao carregar detalhes do pet"
        );
        setLoading(false);
      });
  }, [id]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center p-6">
        <div className="text-center">
          <LoadingSpinner size="lg" />
          <p className="mt-4 text-gray-600">Carregando detalhes...</p>
        </div>
      </div>
    );
  }

  if (error || !pet) {
    return (
      <div className="p-6">
        <div className="bg-red-50 border border-red-200 text-red-700 px-6 py-4 rounded-lg">
          <h3 className="font-semibold">Erro ao carregar pet</h3>
          <p>{error || "Pet não encontrado"}</p>
        </div>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-4xl mx-auto">
      <div className="bg-white rounded-2xl shadow-lg overflow-hidden">
        {pet.imagens && pet.imagens.length > 0 && (
          <div className="relative w-full h-96">
            <Image
              src={pet.imagens[0]}
              alt={pet.nome}
              fill
              className="object-cover"
              priority
            />
          </div>
        )}

        <div className="p-6">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">{pet.nome}</h1>
          <p className="text-gray-600 mb-6">{pet.descricao}</p>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
            {pet.especie && (
              <div className="flex items-center gap-2">
                <span className="font-semibold text-gray-700">Espécie:</span>
                <span className="text-gray-600">{pet.especie}</span>
              </div>
            )}
            {pet.raca && (
              <div className="flex items-center gap-2">
                <span className="font-semibold text-gray-700">Raça:</span>
                <span className="text-gray-600">{pet.raca}</span>
              </div>
            )}
            {pet.idadeMeses !== undefined && (
              <div className="flex items-center gap-2">
                <span className="font-semibold text-gray-700">Idade:</span>
                <span className="text-gray-600">
                  {pet.idadeMeses >= 12
                    ? `${Math.floor(pet.idadeMeses / 12)} anos`
                    : `${pet.idadeMeses} meses`}
                </span>
              </div>
            )}
            {pet.porte && (
              <div className="flex items-center gap-2">
                <span className="font-semibold text-gray-700">Porte:</span>
                <span className="text-gray-600">{pet.porte}</span>
              </div>
            )}
            {pet.castrado !== undefined && (
              <div className="flex items-center gap-2">
                <span className="font-semibold text-gray-700">Castrado:</span>
                <span className="text-gray-600">
                  {pet.castrado ? "Sim" : "Não"}
                </span>
              </div>
            )}
            {pet.vacinado !== undefined && (
              <div className="flex items-center gap-2">
                <span className="font-semibold text-gray-700">Vacinado:</span>
                <span className="text-gray-600">
                  {pet.vacinado ? "Sim" : "Não"}
                </span>
              </div>
            )}
          </div>

          <button
            onClick={() => alert("Solicitação de adoção enviada! 💖")}
            className="w-full md:w-auto bg-green-600 text-white px-8 py-3 rounded-xl hover:bg-green-700 transition-colors font-semibold shadow-md"
          >
            Adotar
          </button>
        </div>
      </div>
    </div>
  );
}
