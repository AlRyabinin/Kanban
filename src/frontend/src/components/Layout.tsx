import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { Outlet, Link, useLocation, useParams } from 'react-router-dom';
import api from '../lib/api';

export default function Layout() {
  const { user, logout } = useAuth();
  const location = useLocation();
  const { boardId } = useParams<{ boardId: string }>();
  const [boardName, setBoardName] = useState('');
  
  const showNewBoardButton = location.pathname === '/boards';
  const isBoardPage = location.pathname.startsWith('/board/');

  useEffect(() => {
    if (isBoardPage && boardId) {
      api.get(`/boards/${boardId}/full`)
        .then(response => {
          setBoardName(response.data.name || 'Без названия');
        })
        .catch(() => {
          setBoardName('');
        });
    } else {
      setBoardName('');
    }
  }, [isBoardPage, boardId]);

  return (
    <div className="h-screen flex flex-col overflow-hidden bg-gray-50">
      {/* Header - фиксированная высота */}
      <header className="bg-white border-b border-gray-200 flex-shrink-0 h-16">
        <div className="max-w-full px-4 sm:px-6 lg:px-8 h-full">
          <div className="flex items-center justify-between h-full">
            <div className="flex items-center space-x-3">
              <Link to="/boards" className="flex items-center space-x-2">
                <span className="text-2xl">📋</span>
                <span className="text-xl font-bold text-gray-900">KanbanFlow</span>
              </Link>
              
              {isBoardPage && (
                <>
                  <span className="text-gray-300">/</span>
                  <Link to="/boards" className="text-sm text-gray-500 hover:text-gray-700">
                    Все доски
                  </Link>
                  <span className="text-gray-300">/</span>
                  <span className="text-sm font-semibold text-gray-900">
                    {boardName || 'Загрузка...'}
                  </span>
                </>
              )}
            </div>
            
            <div className="flex items-center space-x-3">
              {showNewBoardButton && (
                <button
                  onClick={() => {
                    const event = new CustomEvent('openCreateBoardModal');
                    window.dispatchEvent(event);
                  }}
                  className="px-3 py-1.5 text-sm font-medium text-white bg-blue-600 rounded hover:bg-blue-700 transition-colors"
                >
                  + Новая доска
                </button>
              )}
              
              {user && (
                <span className="text-sm text-gray-600 hidden sm:inline">
                  {user.firstName} {user.lastName}
                </span>
              )}
              
              <button
                onClick={logout}
                className="px-3 py-1.5 text-sm font-medium text-white bg-red-600 rounded hover:bg-red-700 transition-colors"
                title="Выйти"
              >
                Выйти
              </button>
            </div>
          </div>
        </div>
      </header>

      {/* Main Content - занимает всё оставшееся место без скролла */}
      <main className="flex-1 overflow-hidden">
        <div className="h-full w-full">
          <Outlet />
        </div>
      </main>
    </div>
  );
}