import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import type { ReactNode } from 'react';

/**
 * Клиент React Query с настройками кэширования.
 */
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5, // Данные считаются свежими 5 минут
      retry: 1, // Повторять запрос при ошибке 1 раз
      refetchOnWindowFocus: false, // Не обновлять данные при фокусе окна
    },
  },
});

interface QueryProviderProps {
  children: ReactNode;
}

/**
 * Провайдер React Query для всего приложения.
 */
export function QueryProvider({ children }: QueryProviderProps) {
  return (
    <QueryClientProvider client={queryClient}>
      {children}
    </QueryClientProvider>
  );
}