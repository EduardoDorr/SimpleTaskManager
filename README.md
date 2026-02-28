# DDS.SimpleTaskManager

## Overview
This repository contains **Phase 1 (Core)** of the Simple Task Manager solution.

The current delivery focuses on a backend API to manage tasks with a simple completion toggle flow:
- `Backlog <-> Completed`

This choice aligns with the requirement to toggle task completion while keeping the first delivery compact and easy to evolve.

The future roadmap is already modeled in [doc/SimpleTaskManager.drawio](doc/SimpleTaskManager.drawio):
- **Phase 2:** Users and Projects
- **Phase 3:** Domain Events and Notification Module

## Phase 1 Scope (Current)
- Backend API (`.NET 9`, Minimal API, EF Core, MySQL)
- Task CRUD-like flow:
  - List tasks
  - Create task
  - Toggle completion status
  - Soft delete task
- Unified response contract with `ApiResult`
- Domain validation for task creation rules
- Reusable pagination validation in `Core`

## Current Domain Model (Phase 1)
`TaskItem` includes:
- `Id`
- `Title`
- `Description`
- `Priority` (`Low`, `Medium`, `High`, `Critical`)
- `Status` (`Backlog`, `Completed`)
- `DueDate`
- Audit/soft-delete fields inherited from base entity

## API Endpoints
Base route: `/api/v1/tasks`

1. `GET /`
- Returns paginated tasks.
- Query params:
  - `page` (must be `> 0`)
  - `pageSize` (must be `> 0` and `<= 1000`)
  - `title`
  - `status`
  - `priority`
  - `isDescending`
  - `isActive`

2. `POST /`
- Creates a new task.
- Body:
  - `title`
  - `description` (optional)
  - `priority`
  - `dueDate`

3. `PATCH /{id}/status`
- Toggles task status between `Backlog` and `Completed`.
- No request body.

4. `DELETE /{id}`
- Soft deletes a task (`IsDeleted = true`).

## Response Contract
All endpoints return the same envelope style (`ApiResult`):
- `success`
- `status`
- `errors`
- `infos`
- `value` (when applicable)

This keeps frontend consumption consistent for both success and failure paths.

## Architecture
Project organization:
- `backend/DDS.SimpleTaskManager.API`
  - Endpoints
  - Application handlers
  - Domain (`TaskItem`)
  - Infrastructure (EF Core, repository, migrations)
- `backend/DDS.SimpleTaskManager.Core`
  - Shared primitives (`Result`, errors, pagination, middleware, cross-cutting utilities)

## Prerequisites
- .NET SDK `9.x`
- Docker Desktop (recommended) or a local MySQL instance
- Git

## Setup and Run
### Option A: Run API locally with `dotnet` (recommended for development)
1. Provide a valid connection string via appsettings, secrets or environment variable;
2. Start MySQL (Docker or local);
3. Run API;
4. Open Swagger:
- `http://localhost:5027/swagger` (or the URL shown in console)

### Option B: Run full stack with Docker Compose
```powershell
docker compose up -d --build
```
API will be exposed at:
- `http://localhost:8080/swagger`
- `https://localhost:8081/swagger`

If the DB container stays unhealthy, verify the MySQL healthcheck credentials in `docker-compose.yml` and align them with `MYSQL_PASSWORD`.

## How to Test (Current)
### 1) Build
```powershell
dotnet build backend/DDS.SimpleTaskManager.API/DDS.SimpleTaskManager.API.csproj
```

### 2) Manual API smoke tests (Swagger or HTTP client)
1. `POST /api/v1/tasks` create a task.
2. `GET /api/v1/tasks` ensure it appears.
3. `PATCH /api/v1/tasks/{id}/status` and confirm status toggles.
4. `PATCH` again and confirm it toggles back.
5. `DELETE /api/v1/tasks/{id}` and confirm it no longer appears in default list.

### 3) Automated tests
Unit and integration tests are planned as the next step (currently not included in this Phase 1 delivery).

## Assumptions
- This phase is backend-first; frontend is delivered in a separate step.
- Authentication/authorization is intentionally out of scope in Phase 1.
- `DueDate` must be today or a future date (UTC date check).
- Task completion is a binary business decision for this phase (`Backlog`/`Completed`).
- Data removal is soft-delete.
- Runtime configuration provides the DB connection string (secrets/env/compose override).

## What Is Prepared for Expansion
The solution is intentionally structured to support later phases:

1. **Phase 2: Users and Projects**
- Add `User` and `Project` aggregates.
- Introduce ownership/relations (`TaskItem -> UserId`, `TaskItem -> ProjectId`).
- Expand endpoints for users/projects as designed in the diagram.

2. **Phase 3: Domain Events and Notifications**
- Raise domain events from aggregate changes (e.g., task completion).
- Add notification module integration.
- Keep API contract stable while expanding side effects.

These phases are documented in [doc/SimpleTaskManager.drawio](doc/SimpleTaskManager.drawio).