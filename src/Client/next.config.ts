import type { NextConfig } from 'next';

const nextConfig: NextConfig = {
    webpack: (config, { dev }) => {
        if (dev) {
            config.watchOptions = {
                ignored: ["**/node_modules/**", "**/.next/**"],
                poll: 1000,
            };
        }
        return config;
    },
    reactStrictMode: true,
};

export default nextConfig;
