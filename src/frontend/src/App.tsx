import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { QueryProvider } from './providers/QueryProvider';
import { BoardPage } from './features/board/BoardPage';
import { BoardsListPage } from './features/workspace/BoardsListPage';
import { Toaster } from 'sonner';

/**
 * Корневой компонент приложения.
 * Настраивает роутинг и провайдеры.
 */
function App() {
  return (
    <QueryProvider>
      <BrowserRouter>
        <Routes>
          {/* Главная страница - список досок */}
          <Route path="/" element={<BoardsListPage />} />
          
          {/* Страница конкретной доски */}
          <Route path="/board/:boardId" element={<BoardPage />} />
          
          {/* Редирект для неизвестных маршрутов */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
        <Toaster position="top-right" richColors />
      </BrowserRouter>
    </QueryProvider>
  );
}

export default App;