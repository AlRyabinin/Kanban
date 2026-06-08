import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../lib/api';
import { Button } from '../../components/ui/Button';
import { X } from 'lucide-react';
import { toast } from 'sonner';

interface CreateBoardModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export function CreateBoardModal({ isOpen, onClose }: CreateBoardModalProps) {
  const queryClient = useQueryClient();
  const [name, setName] = useState('');

  const createMutation = useMutation({
    mutationFn: async (data: { name: string }) => {
      await api.post('/Boards', data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['boards'] });
      toast.success('Доска создана!');
      setName('');
      onClose();
    },
    onError: () => {
      toast.error('Не удалось создать доску');
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) return;

    createMutation.mutate({ name });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
        <div className="mb-4 flex items-center justify-between">
          <h2 className="text-lg font-semibold">Новая доска</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-gray-600">
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Название *
            </label>
            <input
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              placeholder="Введите название доски"
              required
              autoFocus
            />
          </div>

          <div className="flex justify-end gap-2">
            <Button type="button" variant="secondary" onClick={onClose}>
              Отмена
            </Button>
            <Button type="submit" variant="primary" disabled={createMutation.isPending}>
              {createMutation.isPending ? 'Создание...' : 'Создать'}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}