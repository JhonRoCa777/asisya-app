# Stage 1: Build
FROM node:20-bullseye-slim AS builder

WORKDIR /app

# Instalar dependencias del sistema necesarias para Vite y TypeScript
RUN apt-get update && apt-get install -y git python3 build-essential && rm -rf /var/lib/apt/lists/*

# Copiar package.json y package-lock.json
COPY package*.json ./

# Instalar dependencias
RUN npm ci

# Copiar todos los archivos
COPY . .

# Build de Vite + TypeScript
RUN npm run build

# Stage 2: Servir con nginx
FROM nginx:alpine

COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]