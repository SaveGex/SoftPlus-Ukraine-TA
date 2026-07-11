import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from './core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="min-h-screen flex flex-col">
      @if (auth.isAuthenticated()) {
        <nav class="bg-indigo-600 text-white px-6 py-3 flex items-center gap-6 shadow">
          <span class="font-semibold text-lg">📝 ToDo</span>
          <a routerLink="/tasks" routerLinkActive="underline font-semibold" class="hover:underline">Tasks</a>
          <a routerLink="/categories" routerLinkActive="underline font-semibold" class="hover:underline">Categories</a>
          <button (click)="logout()" class="ml-auto bg-indigo-800 hover:bg-indigo-900 px-3 py-1.5 rounded text-sm">
            Log out
          </button>
        </nav>
      }
      <main class="flex-1 max-w-5xl w-full mx-auto p-4">
        <router-outlet />
      </main>
    </div>
  `
})
export class AppComponent {
  constructor(public auth: AuthService, private router: Router) {}

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
