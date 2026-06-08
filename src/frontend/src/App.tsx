import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './contexts/AuthContext';
import Login from './components/pages/Login';
import Register from './components/pages/Register';
import ProtectedRoute from './components/ProtectedRoute';
import Layout from './components/Layout';
import BoardsListPage from './features/workspace/BoardsListPage';
import { BoardPage } from './features/board/BoardPage'

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          
          <Route 
            element={
              <ProtectedRoute>
                <Layout />
              </ProtectedRoute>
            }
          >
            <Route path="/boards" element={<BoardsListPage />} />
            <Route path="/board/:boardId" element={<BoardPage />} />
          </Route>
          
          <Route path="/" element={<Navigate to="/boards" replace />} />
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;