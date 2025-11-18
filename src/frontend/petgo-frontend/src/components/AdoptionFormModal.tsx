import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { X } from 'lucide-react';
import { z } from 'zod'; 


const adoptionSchema = z.object({
  nome: z.string().min(3, "Nome muito curto"),
  email: z.string().email("Email inválido"),
  telefone: z.string().min(10, "Telefone inválido"),
  motivo: z.string().min(20, "Conte-nos um pouco mais sobre por que quer adotar"),
});

type AdoptionFormData = z.infer<typeof adoptionSchema>;


interface AdoptionFormModalProps {
  petId: number;
  onClose: () => void;
}

export default function AdoptionFormModal({ petId, onClose }: AdoptionFormModalProps) {
  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<AdoptionFormData>({
    resolver: zodResolver(adoptionSchema) 
  });

  const onSubmit = async (data: AdoptionFormData) => {
    try {
      alert("Interesse registrado com sucesso!");
      onClose();
    } catch (error) {
      alert("Erro ao enviar.");
    }
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
      <div className="bg-white p-6 rounded-lg max-w-md w-full relative">
        
        <button onClick={onClose} className="absolute top-4 right-4 text-gray-500"><X size={24} /></button>
        
        <h2 className="text-xl font-bold mb-4">Interesse em Adotar</h2>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          
          <div>
            <label className="block text-sm font-medium">Seu Nome</label>
            <input {...register('nome')} className="w-full border p-2 rounded" />
            {errors.nome && <span className="text-red-500 text-xs">{errors.nome.message}</span>}
          </div>

          {/* ... Outros campos (Email, Telefone, Motivo) ... */}
          
          <div>
             <label className="block text-sm font-medium">Email</label>
             <input {...register('email')} className="w-full border p-2 rounded" />
             {errors.email && <span className="text-red-500 text-xs">{errors.email.message}</span>}
          </div>
          
          <div>
             <label className="block text-sm font-medium">Telefone</label>
             <input {...register('telefone')} className="w-full border p-2 rounded" />
             {errors.telefone && <span className="text-red-500 text-xs">{errors.telefone.message}</span>}
          </div>
          
          <div>
             <label className="block text-sm font-medium">Motivo</label>
             <textarea {...register('motivo')} className="w-full border p-2 rounded" />
             {errors.motivo && <span className="text-red-500 text-xs">{errors.motivo.message}</span>}
          </div>

          <div className="flex justify-end gap-2 mt-4">
            <button type="button" onClick={onClose} className="px-4 py-2 text-gray-600">Cancelar</button>
            <button 
              type="submit" 
              disabled={isSubmitting}
              className="px-4 py-2 bg-purple-600 text-white rounded disabled:opacity-50"
            >
              {isSubmitting ? 'Enviando...' : 'Enviar Interesse'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}