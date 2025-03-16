import { NextConfig } from 'next'
// eslint-disable-next-line @typescript-eslint/no-require-imports
const path = require('path')

const nextConfig: NextConfig = {
  webpack: (config) => {
    config.resolve.alias['@app'] = path.resolve(__dirname, 'src/Client/app')
    return config
  },
  reactStrictMode: true,
}

export default nextConfig
