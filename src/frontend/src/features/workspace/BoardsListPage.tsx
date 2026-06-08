import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import CreateBoardModal from './CreateBoardModal';
import api from '../../lib/api';

interface Board {
  id: string;
  name: string;
  createdAt: string;
}

export default function BoardsListPage() {
  const [boards, setBoards] = useState<Board[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);

  useEffect(() => {
    loadBoards();
    
    const handleOpenModal = () => setIsCreateModalOpen(true);
    window.addEventListener('openCreateBoardModal', handleOpenModal);
    
    return () => {
      window.removeEventListener('openCreateBoardModal', handleOpenModal);
    };
  }, []);

  const loadBoards = async () => {
    try {
      const response = await api.get('/boards');
      setBoards(response.data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Ошибка загрузки досок');
    } finally {
      setLoading(false);
    }
  };

  const handleBoardCreated = (boardId: string) => {
    window.location.href = `/board/${boardId}`;
  };

  const handleDeleteBoard = async (boardId: string, boardName: string) => {
    const confirmed = window.confirm(
      `Вы уверены, что хотите удалить доску "${boardName}"? Все колонки и задачи будут удалены.`
    );
    
    if (!confirmed) return;
    
    try {
      await api.delete(`/boards/${boardId}`);
      setBoards(boards.filter(b => b.id !== boardId));
    } catch (err: any) {
      alert(err.response?.data?.message || 'Ошибка при удалении доски');
    }
  };

  if (loading) {
    return (
      <div className="p-6">
        <p className="text-gray-500">Загрузка...</p>
      </div>
    );
  }

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold text-gray-900 mb-6">Мои доски</h1>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      {boards.length === 0 ? (
        <div className="text-center py-12 border-2 border-dashed border-gray-300 rounded-lg">
          <div className="text-4xl mb-4">📋</div>
          <h3 className="text-lg font-medium text-gray-900 mb-2">Нет досок</h3>
          <p className="text-gray-500">Создайте первую доску, чтобы начать работу</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
          {boards.map((board) => (
            <div
              key={board.id}
              className="relative group bg-white border border-gray-200 rounded-lg hover:border-gray-300 hover:shadow-sm transition-all"
            >
              <Link to={`/board/${board.id}`} className="block p-4">
                <h3 className="text-base font-semibold text-gray-900 mb-1">
                  {board.name}
                </h3>
                <p className="text-sm text-gray-500">
                  Открыть доску →
                </p>
              </Link>
              
              {/* Кнопка удаления */}
              <button
                onClick={(e) => {
                  e.preventDefault();
                  e.stopPropagation();
                  handleDeleteBoard(board.id, board.name);
                }}
                className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 p-1.5 text-gray-400 hover:text-red-600 hover:bg-red-50 rounded transition-all"
                title="Удалить доску"
              >
                <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                </svg>
              </button>
            </div>
          ))}
        </div>
      )}

      <CreateBoardModal
        isOpen={isCreateModalOpen}
        onClose={() => setIsCreateModalOpen(false)}
        onSuccess={handleBoardCreated}
      />
    </div>
  );
}