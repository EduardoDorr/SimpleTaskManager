# DDS.SimpleTaskManager

## Project And Objective
`DDS.SimpleTaskManager` is a full-stack solution built for a technical assessment.

The objective is to deliver a clean, production-minded implementation of task management with:
- task creation
- task listing
- task status toggle
- frontend/backend communication with predictable contracts

## Current Delivery (Phase 1 - Implemented)
### Backend
- .NET 9 Minimal API
- EF Core + MySQL
- Domain rules inside `TaskItem`
- Consistent response envelope using `Result` + `ApiResult`
- Pagination, filters and soft delete

### Frontend
- React + TypeScript + Vite
- MUI + React Query + Axios
- Feature-First module organization (`features/tasks`, `shared`)
- Tasks table with status toggle through a checkbox control
- Unit/component test suite with Vitest + Testing Library

### Implemented API Endpoints
Base route: `/api/v1/tasks`

1. `GET /`
- Returns `200 OK` with paginated tasks in `value`.
- Query params:
  - `page` (`> 0`)
  - `pageSize` (`> 0` and `<= 1000`)
  - `title`
  - `status`
  - `priority`
  - `isDescending`
  - `isActive`

2. `POST /`
- Creates a task.
- Returns `201 Created` with `id` in `value`.
- Body:
  - `title`
  - `description` (optional)
  - `priority`
  - `dueDate`

3. `PATCH /{id}/status`
- Toggles task status.
- Returns `200 OK`.

4. `DELETE /{id}`
- Soft deletes task (`IsDeleted = true`).
- Returns `200 OK`.

### Response Contract
All API responses use `ApiResult`:
- `success`
- `status`
- `errors`
- `infos`
- `value` (when applicable)

## Assumptions, Inferences And Trade-Offs
### Why MySQL instead of SQLite?
The decision was practical. MySQL is a real relational database, easy to run via Docker, license-free for this scope, and simple to provision with the API in one compose file.

Trade-off accepted: slightly higher operational setup than SQLite, in exchange for a more production-like environment.

### Why `Result` + `ApiResult` instead of only `ProblemDetails`?
Using only `ProblemDetails` usually means one shape for errors and another for success responses.

With `ApiResult`, the contract is always the same. Clients can consistently:
- check `success`
- consume domain error codes/messages
- map error codes to client-specific messages if needed

Trade-off accepted: less adherence to default RFC-style error-only shape, in exchange for a single, predictable API envelope.

### Why include soft delete, filters and pagination?
The goal was to demonstrate engineering standards beyond the bare minimum.

- Soft delete preserves historical data.
- Pagination avoids large payloads as data grows.
- Filters improve usability and reduce unnecessary traffic.

### Where could this be simplified without losing core challenge features?
Possible simplifications:
- remove pagination
- remove filters
- remove audit fields (`CreatedAt`, `UpdatedAt`)
- remove `dueDate`
- remove soft delete
- remove EF interceptors related to audit/delete

This would reduce complexity and data footprint while keeping create/list/toggle.

## Phase 1 Design Choices That Already Support Scale
Even in Phase 1, the solution was structured to support growth:
- feature isolation in frontend (`Feature-First`)
- clear backend boundaries (API/Application/Domain/Infrastructure)
- consistent result handling and centralized error contract
- reusable cross-cutting infrastructure (interceptors, middleware, pagination primitives)
- task status model designed to evolve from 2-state to richer workflows

## Next Steps (Planned Phases)
Roadmap is documented in [doc/SimpleTaskManager.drawio](doc/SimpleTaskManager.drawio), with tabs explicitly marked as:
- `Phase 1: Core (Implemented)`
- `Phase 2: Projects and Users (Planned)`
- `Phase 3: Notifications (Planned)`

### Phase 2 (Planned)
- Users
- Projects
- Authentication/authorization with JWT transported via secure cookies (`HttpOnly`, `Secure`, `SameSite`) to reduce token exposure in browser JavaScript and mitigate CSRF risk
- Expanded task workflow:
  - `Backlog -> InProgress`
  - `Backlog -> Canceled`
  - `InProgress -> Completed`
  - `InProgress -> Canceled`
- Status update API evolution to support explicit state transitions:
  - current `PATCH /api/v1/tasks/{id}/status` is toggle-based
  - new versioned contract in `v2` should receive the target status in request body (breaking-change-safe evolution path)

Domain modeling for this phase includes Value Objects (for example `Cpf`, `Address`) to keep validation localized, reusable and consistent.

### Phase 3 (Planned)
- Domain events + notifications module
- Outbox pattern for reliable event publication
- Asynchronous messaging backbone (for example RabbitMQ, Azure Service Bus or AWS SQS)
- Full observability maturity:
  - structured logs
  - distributed tracing
  - metrics and dashboards

## Prerequisites
- .NET SDK `9.x`
- Node.js `22+`
- Docker Desktop (recommended)
- Git

## Run Locally
### Option A: Full stack with Docker Compose
Before running Docker Compose, create a `.env` file at the repository root with the required variables:

- `MYSQL_ROOT_PASSWORD=root`
- `MYSQL_DATABASE=TaskManagerDB`
- `MYSQL_USER=admin`
- `MYSQL_PASSWORD=Admin123?`
- `ASPNETCORE_ENVIRONMENT=Development`
- `API_PORT=8080`
- `FRONTEND_PORT=5173`
- `DB_PORT=3307`
- `VITE_API_BASE_URL=http://localhost:8080`


```powershell
docker compose up -d --build
```

Expected services:
- API: `http://localhost:8080/swagger`
- Frontend: `http://localhost:5173`

### Option B: Backend + Frontend separately
For this option, a MySQL instance is still required. You can start it with Docker Compose or with a direct `docker run` command.

Using Docker Compose (recommended):

```powershell
docker compose up -d taskmanagerdb
```

Using `docker run`:

```bash
docker run \
  -d --name taskmanagerdb-local \
  -p 3307:3306 \
  -e MYSQL_ROOT_PASSWORD=root \
  -e MYSQL_DATABASE=TaskManagerDB \
  -e MYSQL_USER=admin \
  -e MYSQL_PASSWORD=Admin123? \
  mysql:8.0.41
```

Backend:
```powershell
$env:ConnectionStrings__TaskManagerDbConnection="Server=localhost;Port=3307;Database=TaskManagerDB;Uid=admin;Pwd=Admin123?;AllowPublicKeyRetrieval=True;"
dotnet run --project backend/DDS.SimpleTaskManager.API/DDS.SimpleTaskManager.API.csproj
```

Frontend:
```powershell
cd frontend/SimpleTaskManager
npm install
npm run dev
```

## Testing
Backend tests:
The backend test suite includes integration tests with MySQL Testcontainers, so Docker must be running.

```powershell
dotnet test backend/DDS.SimpleTaskManager.sln --no-restore
```

Frontend production build:
```powershell
cd frontend/SimpleTaskManager
npm run build
```

Frontend unit/component tests:
```powershell
cd frontend/SimpleTaskManager
npm run test
```

Frontend coverage report:
```powershell
cd frontend/SimpleTaskManager
npm run test:coverage
```

## Project Structure
- `backend/DDS.SimpleTaskManager.API`
  - Endpoints
  - Application handlers
  - Domain (`TaskItem`)
  - Infrastructure (EF Core, repositories, migrations)
- `backend/DDS.SimpleTaskManager.Core`
  - Shared primitives (`Result`, errors, pagination, middleware, telemetry, etc.)
- `frontend/SimpleTaskManager`
  - `app/` bootstrap, routes, theme
  - `features/tasks/` feature modules
  - `shared/` reusable components, HTTP layer, layout
