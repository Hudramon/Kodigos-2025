
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["ApiOS/ApiOS.csproj", "ApiOS/"]
RUN dotnet restore "ApiOS/ApiOS.csproj"
COPY . .
WORKDIR "/app/ApiOS"
RUN dotnet publish "ApiOS.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "ApiOS.dll"]
