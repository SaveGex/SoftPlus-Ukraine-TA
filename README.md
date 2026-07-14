# To-Do Application

A To-Do application (like Microsoft To-Do) built as a full stack test task.


# Run in one command

```bash
git clone https://github.com/SaveGex/SoftPlus-Ukraine-TA.git
&& cd SoftPlus-Ukraine-TA
&& npm run start
```

## Clear everything

```bash
npm run clear
```

<<<<<<< HEAD
=======
## Run with Docker

Each part of the app is packaged as its own image with a single responsibility
(`src/ToDo.Api/Dockerfile` — backend, `src/ToDo.Angular.Ui/Dockerfile` — frontend),
and `docker-compose.yml` wires them together with a SQL Server database.

```bash
docker compose up --build
```

- Frontend: http://localhost:4200
- Backend API: http://localhost:8080/api (Scalar docs at http://localhost:8080/scalar)
- SQL Server: localhost:1433 (sa / see `MSSQL_SA_PASSWORD`)

The API container applies pending EF Core migrations automatically on startup, so
the database schema is created for you on first run. Optionally copy `.env.example`
to `.env` to override the SA password and JWT key.

```bash
docker compose down        # stop
docker compose down -v     # stop and wipe the DB volume
```

>>>>>>> 59dc70c (feat: dockerize application stack and implement API resource ownership validation)

## Task

```
To-Do application (like Microsoft To-Do)

Features:
1. Creating, Viewing, Editing, Deleting tasks
2. Adding Categories for tasks
3. Log in/log out
4. Pagination for the list of tasks
5. Implement searching and filtering by categories

Technologies:
1. Back - REST API on .NET Core with any relational DB (ex., MS SQL)
2. Use 4 levels architecture: controllers, services, interfaces and data access
3. Use EF Core, Dependency Injection
4. Front - Angular (use bootstrap or tailwind)
```

## Stack

- **Backend**: ASP.NET Core Web API, EF Core, MS SQL Server, 4-layer architecture
  (Controllers → Services → Interfaces → Data Access), Dependency Injection
- **Frontend**: Angular, Tailwind CSS
