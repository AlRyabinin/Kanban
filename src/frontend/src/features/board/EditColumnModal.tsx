import { useState, useEffect } from 'react';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import api from '../../lib/api';
import type { ColumnWithTasksDto } from '../../types';
import { Button } from '../../components/ui/Button';
import { X } from 'lucide-react';
import { toast } from 'sonner';

const PRESET_COLORS = [
  { name: 'Нет', value: null },
  { name: 'Синий', value: '#3b82f6' },
  { name: 'Зелёный', value: '#10b981' },
  { name: 'Красный', value: '#ef4444' },
  { name: 'Оранжевый', value: '#f59e0b' },
  { name: 'Фиолетовый', value: '#8b5cf6' },
  { name: 'Розовый', value: '#ec4899' },
  { name: 'Серый', value: '#6b7280' },
];

interface EditColumnModalProps {
  isOpen: boolean;
  onClose: () => void;
  column: ColumnWithTasksDto | null;
}

export function EditColumnModal({ isOpen, onClose, column }: EditColumnModalProps) {
  const queryClient = useQueryClient();
  const [name, setName] = useState('');
  const [selectedColor, setSelectedColor] = useState<string | null>(null);

  useEffect(() => {
    if (column) {
      setName(column.name);
      setSelectedColor(column.color);
    }
  }, [column]);

  const updateMutation = useMutation({
    mutationFn: async (data: { columnId: string; name: string; color: string | null }) => {
      await api.put(`/Columns/${data.columnId}`, {
        columnId: data.columnId,
        name: data.name,
        color: data.color,
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['board'] });
      toast.success('Колонка обновлена!');
      onClose();
    },
    onError: () => {
      toast.error('Не удалось обновить колонку');
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim() || !column) return;

    updateMutation.mutate({
      columnId: column.id,
      name,
      color: selectedColor,
    });
  };

  if (!isOpen || !column) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
      <div className="w-full max-w-md rounded-xl bg-white p-6 shadow-xl">
        <div className="mb-4 flex items-center justify-between">
          <h2 className="text-lg font-semibold">Редактировать колонку</h2>
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
              required
              autoFocus
            />
          </div>

          <div>
            <label className="mb-2 block text-sm font-medium text-gray-700">
              Цвет заголовка
            </label>
            <div className="grid grid-cols-4 gap-2">
              {PRESET_COLORS.map((color) => (
                <button
                  key={color.value ?? 'none'}
                  type="button"
                  onClick={() => setSelectedColor(color.value)}
                  className={`flex items-center justify-center rounded-lg border-2 p-2 transition-all ${
                    selectedColor === color.value
                      ? 'border-blue-500 bg-blue-50'
                      : 'border-gray-200 hover:border-gray-300'
                  }`}
                >
                  {color.value ? (
                    <div
                      className="h-6 w-6 rounded-full"
                      style={{ backgroundColor: color.value }}
                    />
                  ) : (
                    <div className="h-6 w-6 rounded-full border-2 border-dashed border-gray-400" />
                  )}
                </button>
              ))}
            </div>
            <p className="mt-1 text-xs text-gray-500">
              {selectedColor ? 'Выбран цвет' : 'Без цвета'}
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