FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY apps/api/Virenza.Api.csproj apps/api/
RUN dotnet restore apps/api/Virenza.Api.csproj

COPY apps/api/ apps/api/
WORKDIR /src/apps/api

RUN dotnet publish Virenza.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:10000

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Virenza.Api.dll"]
