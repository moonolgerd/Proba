FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Proba.Client/Proba.csproj", "Proba.Client/"]
RUN dotnet restore "Proba.Client/Proba.csproj"
COPY . .
WORKDIR "/src/Proba.Client"
RUN dotnet build "Proba.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Proba.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proba.Client.dll"]