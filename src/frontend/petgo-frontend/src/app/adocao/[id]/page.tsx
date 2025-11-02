'use client';

export const dynamic = "force-dynamic";
import { useEffect, useState } from 'react';
import { useParams } from 'next/navigation';
import axios from 'axios';

export default function PetDetalhes() {
  const { id } = useParams();
  const [pet, setPet] = useState<any>(null);

  useEffect(() => {
    axios.get(`${process.env.NEXT_PUBLIC_API_URL}/api/Produtos/${id}`)
      .then((res) => setPet(res.data))
      .catch((err) => console.error('Erro ao carregar pet:', err));
  }, [id]);

  if (!pet) return <p className="p-6">Carregando detalhes...</p>;

  return (
    <div className="p-6">
      <img src={pet.imagens?.[0]} alt={pet.nome} className="w-full max-w-md rounded-2xl shadow-lg" />
      <h1 className="text-3xl font-bold mt-4">{pet.nome}</h1>
      <p className="mt-2 text-gray-600">{pet.descricao}</p>
      <p className="mt-4"><b>Espécie:</b> {pet.especie}</p>
      <p><b>Raça:</b> {pet.raca}</p>
      <p><b>Castrado:</b> {pet.castrado ? 'Sim' : 'Não'}</p>
      <p><b>Vacinado:</b> {pet.vacinado ? 'Sim' : 'Não'}</p>

      <button
        onClick={() => alert('Solicitação de adoção enviada! 💖')}
        className="mt-6 bg-green-600 text-white px-6 py-2 rounded-xl hover:bg-green-700 transition"
      >
        Adotar
      </button>
    </div>
  );
}
