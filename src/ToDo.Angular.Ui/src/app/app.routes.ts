import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'tasks' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then((m) => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/register/register.component').then((m) => m.RegisterComponent)
  },
  {
    path: 'tasks',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/tasks/task-list/task-list.component').then((m) => m.TaskListComponent)
  },
  {
    path: 'tasks/new',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/tasks/task-form/task-form.component').then((m) => m.TaskFormComponent)
  },
  {
    path: 'tasks/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/tasks/task-detail/task-detail.component').then((m) => m.TaskDetailComponent)
  },
  {
    path: 'tasks/:id/edit',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/tasks/task-form/task-form.component').then((m) => m.TaskFormComponent)
  },
  {
    path: 'categories',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/categories/category-list/category-list.component').then(
        (m) => m.CategoryListComponent
      )
  },
  { path: '**', redirectTo: 'tasks' }
];
