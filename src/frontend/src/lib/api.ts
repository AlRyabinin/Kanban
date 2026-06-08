import axios from 'axios';

/**
 * Настроенный экземпляр Axios для взаимодействия с backend API.
 * Автоматически добавляет базовый URL и заголовки.
 */
const api = axios.create({
  baseURL: '/api', // Проксируется через Vite на backend
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Интерсептор запросов: добавляет JWT-токен (когда будет аутентификация).
 */
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

/**
 * Интерсептор ответов: глобальная обработка ошибок.
 */
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // TODO: Редирект на страницу логина
      console.warn('Неавторизован. Требуется вход.');
    }
    return Promise.reject(error);
  }
);

export default api;