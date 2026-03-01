# DDS.SimpleTaskManager

## Overview
`DDS.SimpleTaskManager` is a full-stack task management solution built for a technical assessment.

Current delivery includes:
- Backend API with domain-driven task rules and consistent API envelope responses.
- Frontend application with a Feature-First architecture and shared reusable UI components.

The system currently focuses on a simple and robust task lifecycle (`Backlog` and `Completed`) while keeping the architecture ready for the next phase of expansion.

The future roadmap is already modeled in [doc/SimpleTaskManager.drawio](doc/SimpleTaskManager.drawio):
- **Phase 2:** Users, Projects and Authentication
- **Phase 3:** Domain Events and Notification Module

## What The API Does Today (Phase 1)
Backend stack:
- .NET 9 Minimal API
- EF Core + MySQL
- Result pattern + `ApiResult` response envelope
- Domain validation in `TaskItem`

Current task capabilities:
- List tasks with pagination and filters
- Create task
- Change task status (toggle)
- Soft delete task

### Endpoints
Base route: `/api/v1/tasks`

1. `GET /`
- Returns `200 OK` with paginated tasks.
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
- Returns `201 Created` with `id`.
- Body:
  - `title`
  - `description` (optional)
  - `priority`
  - `dueDate`

1. `PATCH /{id}/status`
- Changes task status according to current phase rules.
- Returns `200 OK` with no `value` payload.

1. `DELETE /{id}`
- Soft deletes task (`IsDeleted = true`).
- Returns `200 OK` with no `value` payload.

### Response Contract
API responses follow `ApiResult`:
- `success`
- `status`
- `errors`
- `infos`
- `value` (when applicable)

## Current Status Model
Current implemented statuses:
- `Backlog`
- `Completed`

Current behavior:
- Status can toggle between `Backlog` and `Completed`.

## Planned Phase 2 Status Expansion
The domain was intentionally prepared to expand to a 4-state workflow with explicit transition rules:

Target statuses:
- `Backlog`
- `InProgress`
- `Completed`
- `Canceled`

Allowed transitions:
- `Backlog -> InProgress`
- `Backlog -> Canceled`
- `InProgress -> Completed`
- `InProgress -> Canceled`

This phase will also introduce:
- `User` entity
- `Project` entity
- Authentication
- Authorization and permission model

## Frontend Architecture (Phase 1)
Frontend stack:
- React + TypeScript + Vite
- MUI (Material UI)
- React Query
- Axios

The frontend is modeled using a **Feature-First** approach:
- Features are isolated by domain (`features/tasks`).
- Shared UI and infra live in `shared/` (layout, dialogs, HTTP, constants).
- This structure improves maintainability and makes new features easier to add without impacting existing modules.

### Why Feature-First Here
- Better isolation of business contexts.
- Easier scaling when new modules are introduced.
- Lower coupling between pages and shared infrastructure.
- Faster onboarding for reviewers/interviewers because responsibilities are explicit.

## Project Structure
- `backend/DDS.SimpleTaskManager.API`
  - Endpoints
  - Application handlers
  - Domain (`TaskItem`)
  - Infrastructure (EF Core, repository, migrations)
- `backend/DDS.SimpleTaskManager.Core`
  - Shared primitives (`Result`, errors, pagination, middleware, telemetry, etc.)
- `frontend/SimpleTaskManager`
  - `app/` bootstrapping, routes, theme
  - `features/tasks/` task feature modules
  - `shared/` reusable components, HTTP layer, layout

## Prerequisites
- .NET SDK `9.x`
- Node.js `22+`
- Docker Desktop (recommended)
- Git

## Run Locally
### Option A: Full stack with Docker Compose
```powershell
docker compose up -d --build
```

Expected services:
- API: `http://localhost:8080/swagger`
- Frontend: `http://localhost:5173`

### Option B: Backend + Frontend separately
Backend:
```powershell
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
```powershell
dotnet test backend/DDS.SimpleTaskManager.sln --no-restore
```

Frontend production build:
```powershell
cd frontend/SimpleTaskManager
npm run build
```

## Notes For Reviewers
This submission prioritizes:
- Clean architecture and separation of concerns
- Explicit domain rules
- Consistent API contract handling
- A frontend structure ready for incremental growth

The roadmap already includes phase-based evolution for richer status flow, user/project ownership, and access control.