import { useState } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import api from '../../lib/api';
import { Button } from '../../components/ui/Button';
import { X } from 'lucide-react';
import { toast } from 'sonner';

interface CreateTaskModalProps {
  isOpen: boolean;
  onClose: () => void;
  columnId: string;
}

export function CreateTaskModal({ isOpen, onClose, columnId }: CreateTaskModalProps) {
  const { boardId } = useParams();
  const queryClient = useQueryClient();
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [dueDate, setDueDate] = useState(''); // ← Добавьте это

  const createMutation = useMutation({
    mutationFn: async (data: { 
      title: string; 
      description?: string; 
      columnId: string; 
      orderIndex: number;
      dueDate?: string;
    }) => {
      await api.post('/Tasks', data);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['board', boardId] });
      toast.success('Задача создана!');
      setTitle('');
      setDescription('');
      setDueDate(''); // ← Очистите дату
      onClose();
    },
    onError: () => {
      toast.error('Не удалось создать задачу');
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) return;

    createMutation.mutate({
      title,
      description: description || undefined,
      columnId,
      orderIndex: Date.now(),
      dueDate: dueDate || undefined, // ← Добавьте дату
    });
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
        <div className="mb-4 flex items-center justify-between">
          <h2 className="text-lg font-semibold">Новая задача</h2>
          <button onClick={onClose} className="text-gray-400 hover:text-gray-600">
            <X className="h-5 w-5" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Заголовок *
            </label>
            <input
              type="text"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              placeholder="Введите заголовок задачи"
              required
            />
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Описание
            </label>
            <textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
              placeholder="Описание задачи (необязательно)"
              rows={3}
            />
          </div>

          {/* Добавьте это поле для даты */}
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Срок выполнения
            </label>
            <input
              type="datetime-local"
              value={dueDate}
              onChange={(e) => setDueDate(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
            />
            <p className="mt-1 text-xs text-gray-500">
              Оставьте пустым, если нет дедлайна
            </p>
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