import type { NextConfig } from "next";

const backendUrl =
  process.env.VIRENZA_API_URL || "http://localhost:5179";

const nextConfig: NextConfig = {
  async rewrites() {
    return [
      {
        source: "/api/backend/health",
        destination: `${backendUrl}/health`,
      },
      {
        source: "/api/backend/:path*",
        destination: `${backendUrl}/api/:path*`,
      },
    ];
  },
};

export default nextConfig;
