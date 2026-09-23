FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Screening.Api/Screening.Api.csproj", "./Screening.Api/"]
COPY ["src/BuildingBlocks/BuildingBlocks.csproj", "./BuildingBlocks/"]
RUN dotnet restore "Screening.Api/Screening.Api.csproj"


COPY src/ ./
RUN dotnet publish "Screening.Api/Screening.Api.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Screening.Api.dll"]