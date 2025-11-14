# Use a imagem base do SDK do .NET para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura as dependências
COPY ["backend/HelpDesk.Api/HelpDesk.Api.csproj", "backend/HelpDesk.Api/"]
RUN dotnet restore "backend/HelpDesk.Api/HelpDesk.Api.csproj"

# Copia o restante dos arquivos do projeto e constrói
COPY . .
WORKDIR "/src/backend/HelpDesk.Api"
RUN dotnet build "HelpDesk.Api.csproj" -c Release -o /app/build

# Publica a aplicação
FROM build AS publish
RUN dotnet publish "HelpDesk.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use a imagem de runtime do .NET para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define a porta de escuta para 8080, que é o padrão do Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Define o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "HelpDesk.Api.dll"]
