# ===========================
# STAGE 1 – BUILD DO PROJETO
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de solução (se existir)
COPY *.sln ./

# Copia os arquivos do projeto
COPY HelpDesk.Api/*.csproj HelpDesk.Api/

# Restaura dependências
RUN dotnet restore

# Copia todo o restante do código
COPY . .

# Publica em modo Release
RUN dotnet publish HelpDesk.Api/HelpDesk.Api.csproj -c Release -o /app/publish


# ===========================
# STAGE 2 – RUNTIME LEVE
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Render usa a porta 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando de inicialização
ENTRYPOINT ["dotnet", "HelpDesk.Api.dll"]
