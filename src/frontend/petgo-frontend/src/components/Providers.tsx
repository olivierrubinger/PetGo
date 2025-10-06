"use client";

import React from "react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import ErrorBoundary from "./ErrorBoundary";

// Configurar QueryClient com tratamento de erros melhorado
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5, // 5 minutos
      gcTime: 1000 * 60 * 10, // 10 minutos
      refetchOnWindowFocus: false,
      retry: (failureCount, error: any) => {
        // Não fazer retry para erros 4xx
        if (error?.status >= 400 && error?.status < 500) {
          console.error("❌ Client error, não fazendo retry:", error);
          return false;
        }
        // Máximo 3 tentativas para erros de rede
        return failureCount < 3;
      },
      retryDelay: (attemptIndex) => Math.min(1000 * 2 ** attemptIndex, 30000),
    },
    mutations: {
      retry: (failureCount, error: any) => {
        if (error?.status >= 400 && error?.status < 500) {
          return false;
        }
        return failureCount < 2;
      },
      onError: (error: any) => {
        console.error("🚨 Mutation error:", error);
      },
    },
  },
});

// Global error handler para queries
queryClient.setMutationDefaults(["produtos"], {
  onError: (error: any) => {
    console.error("🚨 Produto mutation error:", error);
  },
});

interface ProvidersProps {
  children: React.ReactNode;
}

export function Providers({ children }: ProvidersProps) {
  return (
    <ErrorBoundary>
      <QueryClientProvider client={queryClient}>
        {children}
        {/* Mostrar DevTools apenas em desenvolvimento */}
        {process.env.NODE_ENV === "development" && (
          <ReactQueryDevtools initialIsOpen={false} />
        )}
      </QueryClientProvider>
    </ErrorBoundary>
  );
}
