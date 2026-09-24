FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Screenings.Api/Screenings.Api.csproj", "./Screenings.Api/"]
COPY ["src/BuildingBlocks/BuildingBlocks.csproj", "./BuildingBlocks/"]
RUN dotnet restore "Screenings.Api/Screenings.Api.csproj"


COPY src/ ./
RUN dotnet publish "Screenings.Api/Screenings.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Screenings.Api.dll"]