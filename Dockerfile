FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем .csproj и восстанавливаем зависимости
COPY ./src/DestructionOfObjectsBackend/DestructionOfObjectsBackend.csproj ./DestructionOfObjectsBackend/
RUN dotnet restore ./DestructionOfObjectsBackend/DestructionOfObjectsBackend.csproj

COPY ./src/DestructionOfObjectsBackend/. ./DestructionOfObjectsBackend/

WORKDIR /src/DestructionOfObjectsBackend
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DestructionOfObjectsBackend.dll"]
