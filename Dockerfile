FROM node:20 AS client-build
WORKDIR /app/client


COPY Frontend/frontend/package*.json ./
RUN npm install

COPY Frontend/frontend/ ./
RUN npm run build
# Vite buduje domyœlnie do folderu 'dist'


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS server-build
WORKDIR /app/server

COPY ["CleanCharge Optimizer/CleanCharge_Backend.csproj", "./CleanCharge Optimizer/"]

RUN dotnet restore "./CleanCharge Optimizer/CleanCharge_Backend.csproj"

COPY ["CleanCharge Optimizer/", "./CleanCharge Optimizer/"]

WORKDIR "/app/server/CleanCharge Optimizer"
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=server-build /app/publish .

COPY --from=client-build /app/client/dist/ ./wwwroot/

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CleanCharge_Backend.dll"]