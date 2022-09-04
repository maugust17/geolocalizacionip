FROM mcr.microsoft.com/dotnet/runtime:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["geolocalizacionip.csproj", "./"]
RUN dotnet restore "geolocalizacionip.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "geolocalizacionip.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "geolocalizacionip.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "geolocalizacionip.dll"]