# Usar a imagem base oficial do .NET Core SDK
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["api_receita/api_receita.csproj", "api_receita/"]
RUN dotnet restore "api_receita/api_receita.csproj"
COPY . .
WORKDIR "/src/api_receita"
RUN dotnet build "api_receita.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api_receita.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api_receita.dll"]
