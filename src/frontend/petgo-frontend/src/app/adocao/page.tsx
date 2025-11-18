import Link from 'next/link';
import Image from 'next/image';
import api from '@/lib/api';


interface PetDisplay {
  id: number;
  name: string;
  breed: string;
  age: number | string;
  imageUrl: string;
}


const ADOCAO_CATEGORY_ID = 4;

async function getPets(): Promise<PetDisplay[]> {
  try {
 
    const response = await api.get('/Produtos');

    
    const data = response.data
      .filter((item: any) => item.categoriaId === ADOCAO_CATEGORY_ID)
      .map((item: any) => ({
        id: item.id,
        name: item.nome,       
        breed: item.raca || 'Raça não informada',       
        age: 0,                 
        imageUrl: item.imagens && item.imagens.length > 0 ? item.imagens[0] : '/placeholder-pet.jpg'
      }));

    return data;
  } catch (error) {
    console.error('Falha ao buscar pets:', error);
    return [];
  }
}

// Componente da Página
export default async function AdocaoPage() {
  const pets = await getPets();

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-4xl font-bold text-center mb-12">
        Pets Disponíveis para Adoção
      </h1>

      {pets.length === 0 ? (
        <div className="text-center py-10">
          <p className="text-xl text-gray-600">
            Nenhum pet encontrado nesta categoria no momento.
          </p>
          <p className="text-sm text-gray-400 mt-2">
            Verifique se o backend está rodando e se há pets cadastrados com Categoria ID 4.
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
          {pets.map((pet) => (
            <Link
              href={`/adocao/${pet.id}`}
              key={pet.id}
              className="block bg-white rounded-lg shadow-lg overflow-hidden transition-transform duration-300 hover:scale-105 hover:shadow-xl border border-gray-100"
            >
              <div className="relative h-60 w-full bg-gray-200">
                <Image
                  src={pet.imageUrl}
                  alt={`Foto de ${pet.name}`}
                  fill
                  className="object-cover"
                  sizes="(max-width: 768px) 100vw, (max-width: 1200px) 50vw, 33vw"
                />
              </div>
              <div className="p-6">
                <h2 className="text-2xl font-bold text-gray-800 mb-2">
                  {pet.name}
                </h2>
                <p className="text-gray-600 mb-1">
                  <span className="font-semibold text-purple-600">Raça:</span> {pet.breed}
                </p>
                <p className="text-gray-600">
                  <span className="font-semibold text-purple-600">Idade:</span> {pet.age === 0 ? 'Não informada' : `${pet.age} anos`}
                </p>
              </div>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
}