FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["60-buttons-api.csproj", "./"]
RUN dotnet restore "60-buttons-api.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "60-buttons-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "60-buttons-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "60-buttons-api.dll"]
