export interface Pet {
  id: string;
  name: string;
  breed: string; // Raça, idade e descrição do pet.
  age: number;
  description: string;
  imageUrl: string;
  available: boolean;
}

// formulário de adoção :) 
export interface AdoptionFormPayload {
  petId: string;
  name: string;
  email: string;
  message: string;
}