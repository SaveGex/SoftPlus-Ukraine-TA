FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/ToDo.Api/To-Do application.csproj", "ToDo.Api/"]
COPY ["src/ToDo.Application/Application.csproj", "ToDo.Application/"]
COPY ["src/ToDo.Infrastructure/Infrastructure.csproj", "ToDo.Infrastructure/"]
COPY ["src/ToDo.Domain/Domain.csproj", "ToDo.Domain/"]

RUN dotnet restore "ToDo.Api/To-Do application.csproj"

COPY src/ .

RUN dotnet publish "ToDo.Api/To-Do application.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "To-Do application.dll"]
