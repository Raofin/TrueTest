# 🐳 Base image with Node.js
FROM node:23-alpine AS base

# 🔧 Install dependencies
FROM base AS deps
RUN apk add --no-cache libc6-compat
WORKDIR /app

# 📦 Install npm dependencies
COPY package.json package-lock.json* .npmrc* ./
RUN npm ci

# ⚙️ Build the application
FROM base AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY . .
RUN npm run build

# 🚀 Production image
FROM base AS runner
WORKDIR /app

# 🌍 Set environment variables
ENV NODE_ENV=production
ENV PORT=9999
ENV HOSTNAME="0.0.0.0"

# 📦 Copy build artifacts to reduce image size
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

# 🔐 Create a non-root user and group
RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

# 🔒 Switch to the non-root user
USER nextjs

EXPOSE 9999

# 🚀 Start the app
CMD ["node", "server.js"]