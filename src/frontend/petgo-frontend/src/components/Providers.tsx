"use client";

import React from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";

// Configurar QueryClient com opções otimizadas
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      // Tempo que os dados ficam "frescos" (não refetch automático)
      staleTime: 1000 * 60 * 5, // 5 minutos
      // Tempo que os dados ficam no cache após saírem de uso
      gcTime: 1000 * 60 * 10, // 10 minutos (era cacheTime)
      // Não refetch quando a janela ganhar foco
      refetchOnWindowFocus: false,
      // Retry em caso de erro
      retry: (failureCount, error: any) => {
        // Não fazer retry para erros 4xx
        if (error?.status >= 400 && error?.status < 500) {
          return false;
        }
        // Máximo 3 tentativas
        return failureCount < 3;
      },
      // Delay entre retries (exponential backoff)
      retryDelay: (attemptIndex) => Math.min(1000 * 2 ** attemptIndex, 30000),
    },
    mutations: {
      // Retry para mutações apenas em erros de rede
      retry: (failureCount, error: any) => {
        if (error?.status >= 400 && error?.status < 500) {
          return false;
        }
        return failureCount < 2;
      },
    },
  },
});

interface ProvidersProps {
  children: React.ReactNode;
}

export function Providers({ children }: ProvidersProps) {
  return (
    <QueryClientProvider client={queryClient}>
      {children}
      {/* Mostrar DevTools apenas em desenvolvimento */}
      {process.env.NODE_ENV === "development" && (
        <ReactQueryDevtools initialIsOpen={false} />
      )}
    </QueryClientProvider>
  );
}
