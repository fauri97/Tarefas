# ---------- Build do Frontend ----------
FROM node:20 AS build-frontend
WORKDIR /app/frontend
COPY frontend/ ./
RUN npm install && npm run build

# ---------- Build do Backend ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-backend
WORKDIR /app

COPY backend/Tarefas.sln ./
COPY backend/Tarefa.API/*.csproj ./Tarefa.API/
COPY backend/Tarefa.Application/*.csproj ./Tarefa.Application/
COPY backend/Tarefa.Domain/*.csproj ./Tarefa.Domain/
COPY backend/Tarefa.Exception/*.csproj ./Tarefa.Exception/
COPY backend/Tarefa.Infra/*.csproj ./Tarefa.Infra/
RUN dotnet restore

COPY backend/. ./
COPY --from=build-frontend /app/frontend/dist ./Tarefa.API/wwwroot

WORKDIR /app/Tarefa.API
RUN dotnet publish -c Release -o /publish

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-backend /publish ./
ENTRYPOINT ["dotnet", "Tarefa.API.dll"]
