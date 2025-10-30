import Image from 'next/image';
import { Pet } from '@/types/pet';
import api from '@/lib/api';
import AdoptionForm from './components/AdoptionForm'; // Importa o formulário
import { notFound } from 'next/navigation';

interface DetailPageProps {
  params: { id: string };
}

// Função para buscar um único pet
async function getPetById(id: string): Promise<Pet | null> {
  try {
    const response = await api.get(`/pets/${id}`);
    return response.data;
  } catch (error) {
    console.error('Falha ao buscar pet:', error);
    return null;
  }
}

export default async function PetDetailPage({ params }: DetailPageProps) {
  const pet = await getPetById(params.id);

  if (!pet) {
    notFound();
  }

  return (
    <div className="container mx-auto px-4 py-12">
      <div className="grid grid-cols-1 md:grid-cols-2 gap-12 items-start">
        {/* Lado da Imagem */}
        <div className="relative w-full h-96 rounded-lg shadow-xl overflow-hidden">
          <Image
            src={pet.imageUrl || '/placeholder-pet.jpg'}
            alt={`Foto de ${pet.name}`}
            layout="fill"
            objectFit="cover"
            priority
          />
        </div>
        {/* Lado dos Detalhes e Formulário */}
        <div className="flex flex-col gap-6">
          <h1 className="text-5xl font-bold text-gray-900">{pet.name}</h1>
          <div className="text-lg text-gray-700 space-y-2">
            <p><strong>Raça:</strong> {pet.breed}</p>
            <p><strong>Idade:</strong> {pet.age} anos</p>
            <p className="pt-2">{pet.description}</p>
          </div>
          {pet.available ? (
            <AdoptionForm petId={pet.id} petName={pet.name} />
          ) : (
            <div className="p-4 bg-yellow-100 text-yellow-800 rounded-lg">
              Este pet já foi adotado ou o processo está em andamento.
            </div>
          )}
        </div>
      </div>
    </div>
  );
}