FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src/
COPY ["Broker.Consumer/Broker.Consumer.csproj", "."]
RUN dotnet restore "./Broker.Consumer.csproj"
COPY . .
WORKDIR "/src/Broker.Consumer/."
RUN dotnet build "Broker.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Broker.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Broker.Consumer.dll"]