import Link from 'next/link';
import Image from 'next/image';
import { Pet } from '@/types/pet';
import api from '@/lib/api';

async function getPets(): Promise<Pet[]> {
  try {

const response = await api.get('/pets');
 return response.data; 
 } catch (error) {
 console.error('Falha ao buscar pets:', error);
 return [];
 }
}

// Componente da Página (Server Component)
export default async function AdocaoPage() {
 const pets = await getPets();

 return (
 <div className="container mx-auto px-4 py-8">
 <h1 className="text-4xl font-bold text-center mb-12">
 Pets Disponíveis para Adoção
 </h1>

 {pets.length === 0 ? (
 <p className="text-center text-gray-500">
Nenhum pet disponível no momento.
 </p>
) : (
 <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
 {pets.map((pet) => (
  <Link
  href={`/adocao/${pet.id}`}
  key={pet.id}
  className="block rounded-lg shadow-lg overflow-hidden transition-transform duration-300 hover:scale-105"
 >
   <div className="relative h-60 w-full">
 <Image
  src={pet.imageUrl || '/placeholder-pet.jpg'}
 alt={`Foto de ${pet.name}`}
 layout="fill"
 objectFit="cover"
 />
 </div>
 <div className="p-6 bg-white">
 <h2 className="text-2xl font-bold text-gray-800 mb-2">
  {pet.name}
</h2>
 <p className="text-gray-600 mb-1">
 <strong>Raça:</strong> {pet.breed}
 </p>
 <p className="text-gray-600">
 <strong>Idade:</strong> {pet.age} anos
 </p>
 </div>
 </Link>
))}
</div>
)}
 </div>
 );
}