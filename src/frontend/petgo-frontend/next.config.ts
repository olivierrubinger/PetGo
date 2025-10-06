import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  // Experimental features para melhor performance
  experimental: {
    optimizePackageImports: ["lucide-react", "@tanstack/react-query"],
  },

  // Configurações de imagem
  images: {
    domains: ["localhost"],
    formats: ["image/webp", "image/avif"],
  },

  // Rewrites para proxy da API
  async rewrites() {
    return [
      {
        source: "/api/:path*",
        destination: "http://localhost:5021/api/:path*",
      },
    ];
  },

  // Headers de segurança
  async headers() {
    return [
      {
        source: "/(.*)",
        headers: [
          {
            key: "X-Frame-Options",
            value: "DENY",
          },
          {
            key: "X-Content-Type-Options",
            value: "nosniff",
          },
          {
            key: "Referrer-Policy",
            value: "origin-when-cross-origin",
          },
        ],
      },
    ];
  },
};

export default nextConfig;
