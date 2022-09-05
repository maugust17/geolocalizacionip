﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
WORKDIR /api
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["estadisiticasapi/estadisticasapi.csproj", "./"]
RUN dotnet restore "estadisticasapi.csproj"
COPY ["estadisiticasapi/.", "."]
WORKDIR "/src/."
RUN dotnet build "estadisticasapi.csproj" -c Release -o /api/build
WORKDIR /src2
COPY ["geolocalizacionip/geolocalizacionip.csproj", "./"]
RUN dotnet restore "geolocalizacionip.csproj"
COPY ["geolocalizacionip/.", "."]
RUN ["chmod", "775", "consulta_ip_*"]
WORKDIR "/src2/."
RUN dotnet build "geolocalizacionip.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/."
RUN dotnet publish "estadisticasapi.csproj" -c Release -o /api/publish
WORKDIR "/src2/."
RUN dotnet publish "geolocalizacionip.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /api
COPY --from=publish /api/publish .
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "/api/estadisticasapi.dll"]
