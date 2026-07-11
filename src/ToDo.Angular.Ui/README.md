# ToDo Client (Angular)

Angular 17 (standalone components) client for the To-Do REST API described in
`v1.json` (the OpenAPI schema you shared), matching the SoftPlus-Ukraine-TA task brief.

## Stack
- Angular 17, standalone components, `@if`/`@for` control flow
- Tailwind CSS (via CDN in `index.html` — no extra build config needed)
- Reactive Forms
- Signals for local state

## Features → where they live
- **Login / Register**: `src/app/features/auth/*`
- **Task CRUD**: `src/app/features/tasks/task-list`, `task-form`, `task-detail`
- **Steps CRUD** (sub-items of a task): inside `task-detail`
- **Category CRUD**: `src/app/features/categories/category-list`
- **Pagination**: `src/app/shared/components/pagination` (client-side, page size 6)
- **Search + filter by category**: in `task-list`, over the full task list
  returned by `GET /api/ToDoTasks`

## A note on auth
`/api/Auth/register` and `/api/Auth/login` are documented with a bare `200 OK`
and no response schema — no token is returned in the body. This strongly
suggests the backend uses cookie-based auth (ASP.NET Identity cookies), so the
client sends `withCredentials: true` on every request (see
`core/interceptors/credentials.interceptor.ts`) instead of storing/attaching a
bearer token. `AuthService` only keeps a local boolean flag (in
`localStorage`) to drive the route guard and nav bar — the real session lives
in the cookie. If the backend actually returns a JWT in a header/body that
just isn't documented in the schema, swap that flag logic for storing/
attaching the token — everything else (services, models, components) stays
the same.

## A note on pagination/search/filtering
The schema's `GET /api/ToDoTasks` takes no query parameters, so there's no
server-side paging/search/filter to call into. The client fetches the full
list once and paginates/searches/filters in memory (`task-list.component.ts`).
If the backend later adds query params (e.g. `?page=&pageSize=&search=&categoryId=`),
move the logic from the computed signals in `task-list.component.ts` into
`TaskService.getAll()` — the rest of the app doesn't need to change.

## Run it
```bash
npm install
npm start        # ng serve, http://localhost:4200
```

Make sure the API from the schema is running at `https://localhost:7046` (see
`src/environments/environment.ts` — change `apiUrl` if yours runs elsewhere),
and that CORS on the API allows credentials from `http://localhost:4200`.

## Build
```bash
npm run build
```
