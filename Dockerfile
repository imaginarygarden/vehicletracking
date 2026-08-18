FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY VehicleTracking.sln ./
COPY VehicleTracking.Domain/VehicleTracking.Domain.csproj VehicleTracking.Domain/
COPY VehicleTracking.Application/VehicleTracking.Application.csproj VehicleTracking.Application/
COPY VehicleTracking.Persistence/VehicleTracking.Persistence.csproj VehicleTracking.Persistence/
COPY VehicleTracking.Web/VehicleTracking.Web.csproj VehicleTracking.Web/
RUN dotnet restore VehicleTracking.Web/VehicleTracking.Web.csproj

COPY . .
RUN dotnet publish VehicleTracking.Web/VehicleTracking.Web.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID
ENTRYPOINT ["dotnet", "VehicleTracking.Web.dll"]
