# --- ETAP 1: Frontend (Node.js) ---
FROM node:20 AS client-build
WORKDIR /app/client

# Kopiujemy pliki zale¿noœci (zauwa¿ podwójne zagnie¿d¿enie Frontend/frontend)
COPY Frontend/frontend/package*.json ./
RUN npm install

# Kopiujemy resztê kodu frontendu
COPY Frontend/frontend/ ./
RUN npm run build
# Vite buduje domyœlnie do folderu 'dist'


# --- ETAP 2: Backend (.NET 8) ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS server-build
WORKDIR /app/server

# Kopiujemy plik projektu
COPY ["CleanCharge Optimizer/CleanCharge_Backend.csproj", "./CleanCharge Optimizer/"]

# Pobieramy zale¿noœci
RUN dotnet restore "./CleanCharge Optimizer/CleanCharge_Backend.csproj"

# Kopiujemy resztê kodu backendu
COPY ["CleanCharge Optimizer/", "./CleanCharge Optimizer/"]

# Budujemy aplikacjê
WORKDIR "/app/server/CleanCharge Optimizer"
RUN dotnet publish -c Release -o /app/publish


# --- ETAP 3: Finalny obraz (Runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Kopiujemy zbudowany backend
COPY --from=server-build /app/publish .

# Kopiujemy zbudowany frontend do folderu wwwroot
COPY --from=client-build /app/client/dist ./wwwroot

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CleanCharge_Backend.dll"]