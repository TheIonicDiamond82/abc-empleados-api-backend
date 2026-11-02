# Etapa 1: Compilación (SDK de .NET 10)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar las dependencias
COPY ["ABCEmpleadosAPI/ABCEmpleadosAPI.csproj", "ABCEmpleadosAPI/"]
RUN dotnet restore "ABCEmpleadosAPI/ABCEmpleadosAPI.csproj"

# Copiar el resto del código fuente del proyecto
COPY . .
WORKDIR "/src/ABCEmpleadosAPI"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Ejecución (Runtime ligero de ASP.NET 10)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Copiar los binarios compilados desde la primera etapa
COPY --from=build /app/publish .

# Comando de inicio que levanta la API
ENTRYPOINT ["dotnet", "ABCEmpleadosAPI.dll"]