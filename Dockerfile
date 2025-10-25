# --- Estágio 1: O Construtor (Usa o .NET SDK completo) ---
# Usamos uma imagem oficial da Microsoft que tem todas as ferramentas para COMPILAR o projeto.
# Damos a este estágio o apelido "build".
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia o arquivo de projeto (.csproj) primeiro.
# O Docker é inteligente: se este arquivo não mudar, ele reutiliza o cache, acelerando builds futuros.
COPY ["ApiOS/ApiOS.csproj", "ApiOS/"]

# Restaura todas as dependências do projeto (os pacotes NuGet).
RUN dotnet restore "ApiOS/ApiOS.csproj"

# Copia todo o resto do código-fonte do projeto.
COPY . .

# Navega para a pasta da API e publica o projeto.
# "Publicar" cria uma versão otimizada e auto-contida da sua API, pronta para produção.
WORKDIR "/app/ApiOS"
RUN dotnet publish "ApiOS.csproj" -c Release -o /app/publish

# --- Estágio 2: O Corredor (Usa apenas o .NET Runtime, muito menor) ---
# Agora, usamos uma imagem muito mais leve que só sabe EXECUTAR uma API .NET, mas não compilá-la.
# Isso torna nossa imagem final muito menor e mais segura.
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
# Copia APENAS o resultado otimizado do estágio "build" para a nossa imagem final.
COPY --from=build /app/publish .

# Expõe a porta 8080. Dentro do container, a API vai rodar na porta 8080.
# O seu amigo vai "conectar" uma porta do computador dele a esta porta do container.
EXPOSE 8080

# O comando final que é executado quando o container inicia.
# Ele simplesmente executa a DLL da nossa API.
ENTRYPOINT ["dotnet", "ApiOS.dll"]