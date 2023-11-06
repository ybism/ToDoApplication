FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ToDoApplication/ToDoApplication.csproj", "ToDoApplication/"]
RUN dotnet restore "ToDoApplication/ToDoApplication.csproj"
COPY . .
WORKDIR "/src/ToDoApplication"
RUN dotnet build "ToDoApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToDoApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoApplication.dll"]
