import { useState, useEffect } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../lib/api';
import type { TaskDto } from '../../types';
import { Button } from '../../components/ui/Button';
import { X } from 'lucide-react';
import { toast } from 'sonner';

interface EditTaskModalProps {
  isOpen: boolean;
  onClose: () => void;
  task: TaskDto | null;
}

export function EditTaskModal({ isOpen, onClose, task }: EditTaskModalProps) {
  const queryClient = useQueryClient();
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [dueDate, setDueDate] = useState('');

  useEffect(() => {
    if (task) {
      setTitle(task.title);
      setDescription(task.description || '');
      setDueDate(task.dueDate ? task.dueDate.slice(0, 16) : '');
    }
  }, [task]);

  const updateMutation = useMutation({
    mutationFn: async (data: { title: string; description?: string; dueDate?: string }) => {
      if (!task) return;
      await api.put(`/Tasks/${task.id}`, {
        taskId: task.id,
        ...data,
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['board'] });
      toast.success('Задача обновлена!');
      onClose();
    },
    onError: () => {
      toast.error('Не удалось обновить задачу');
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) return;

    updateMutation.mutate({
      title,
      description: description || undefined,
      dueDate: dueDate || undefined,
    });
  };

  if (!isOpen || !task) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
        <div className="mb-4 flex items-center justify-between">
          <h2 className="text-lg font-semibold">Редактировать задачу</h2>
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
            <Button type="submit" variant="primary" disabled={updateMutation.isPending}>
              {updateMutation.isPending ? 'Сохранение...' : 'Сохранить'}
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}