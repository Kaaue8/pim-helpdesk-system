# Use a imagem base do SDK do .NET para construir a aplica��o
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /srcz

# Copia o arquivo de projeto e restaura as depend�ncias
# O caminho � relativo ao Root Directory do Render (backend/HelpDesk.Api)
COPY ["HelpDesk.Api.csproj", "./"]
RUN dotnet restore "HelpDesk.Api.csproj"

# Copia o restante dos arquivos do projeto e constr�i
COPY . .
# O WORKDIR � o diret�rio raiz do projeto dentro do cont�iner
WORKDIR "/src"
RUN dotnet build "HelpDesk.Api.csproj" -c Release -o /app/build

# Publica a aplica��o
FROM build AS publish
RUN dotnet publish "HelpDesk.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use a imagem de runtime do .NET para rodar a aplica��o
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define a porta de escuta para 8080, que � o padr�o do Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Define o ponto de entrada da aplica��o
ENTRYPOINT ["dotnet", "HelpDesk.Api.dll"]
