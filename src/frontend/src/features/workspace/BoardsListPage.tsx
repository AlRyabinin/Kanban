import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import api from '../../lib/api';
import type { BoardInfo } from '../../types';
import { Card, CardHeader, CardTitle } from '../../components/ui/Card';
import { Button } from '../../components/ui/Button';
import { LayoutGrid, Plus, Trash2 } from 'lucide-react';
import { CreateBoardModal } from './CreateBoardModal';
import { Toaster, toast } from 'sonner';

/**
 * Страница со списком всех досок.
 * Загружает доски с backend и отображает их в виде карточек.
 */
export function BoardsListPage() {
  const queryClient = useQueryClient();
  const [isModalOpen, setIsModalOpen] = useState(false);

  const { data: boards, isLoading, error } = useQuery<BoardInfo[]>({
    queryKey: ['boards'],
    queryFn: async () => {
      const response = await api.get<BoardInfo[]>('/Boards');
      return response.data;
    },
  });

  // Мутация удаления доски
  const deleteMutation = useMutation({
    mutationFn: async (boardId: string) => {
      await api.delete(`/Boards/${boardId}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['boards'] });
      toast.success('Доска удалена');
    },
    onError: () => {
      toast.error('Не удалось удалить доску');
    },
  });

  const handleDelete = (e: React.MouseEvent, boardId: string, boardName: string) => {
    e.preventDefault();
    e.stopPropagation();
    if (window.confirm(`Вы уверены, что хотите удалить доску "${boardName}"?`)) {
      deleteMutation.mutate(boardId);
    }
  };

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p className="text-gray-500">Загрузка досок...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p className="text-red-500">Ошибка загрузки досок. Убедитесь, что backend запущен.</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Toaster position="top-right" richColors />

      {/* Header */}
      <header className="border-b border-gray-200 bg-white">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-4">
          <div className="flex items-center gap-2">
            <LayoutGrid className="h-6 w-6 text-blue-600" />
            <h1 className="text-xl font-bold text-gray-900">KanbanFlow</h1>
          </div>
          <Button variant="primary" size="sm" onClick={() => setIsModalOpen(true)}>
            <Plus className="mr-2 h-4 w-4" />
            Новая доска
          </Button>
        </div>
      </header>

      {/* Content */}
      <main className="mx-auto max-w-7xl px-6 py-8">
        <h2 className="mb-6 text-2xl font-bold text-gray-900">Мои доски</h2>

        {boards && boards.length > 0 ? (
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {boards.map((board) => (
              <div key={board.id} className="group relative">
                <Link to={`/board/${board.id}`}>
                  <Card className="transition-all hover:shadow-md cursor-pointer">
                    <CardHeader>
                      <CardTitle>{board.name}</CardTitle>
                      <p className="text-sm text-gray-500">Открыть доску →</p>
                    </CardHeader>
                  </Card>
                </Link>

                {/* Кнопка удаления — появляется при наведении */}
                <button
                  onClick={(e) => handleDelete(e, board.id, board.name)}
                  className="absolute right-3 top-3 rounded p-1.5 text-gray-400 opacity-0 transition-opacity hover:bg-red-50 hover:text-red-600 group-hover:opacity-100"
                  title="Удалить доску"
                >
                  <Trash2 className="h-4 w-4" />
                </button>
              </div>
            ))}
          </div>
        ) : (
          <div className="rounded-lg border-2 border-dashed border-gray-300 p-12 text-center">
            <LayoutGrid className="mx-auto h-12 w-12 text-gray-400" />
            <h3 className="mt-4 text-lg font-semibold text-gray-900">Нет досок</h3>
            <p className="mt-2 text-gray-500">
              Создайте первую доску, чтобы начать работу
            </p>
          </div>
        )}
      </main>

      {/* Модальное окно создания доски */}
      <CreateBoardModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
      />
    </div>
  );
}