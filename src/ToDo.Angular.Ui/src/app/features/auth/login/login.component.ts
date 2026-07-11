import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <div class="max-w-sm mx-auto mt-16 bg-white rounded-lg shadow p-6">
      <h1 class="text-2xl font-bold mb-4 text-slate-800">Log in</h1>
      <form [formGroup]="form" (ngSubmit)="submit()" class="flex flex-col gap-3">
        <label class="text-sm font-medium text-slate-600">
          Email
          <input formControlName="email" type="email" class="mt-1 w-full border rounded px-3 py-2" />
        </label>
        <label class="text-sm font-medium text-slate-600">
          Password
          <input formControlName="password" type="password" class="mt-1 w-full border rounded px-3 py-2" />
        </label>
        @if (error()) {
          <p class="text-red-600 text-sm">{{ error() }}</p>
        }
        <button
          type="submit"
          [disabled]="form.invalid || loading()"
          class="bg-indigo-600 disabled:opacity-50 text-white rounded py-2 mt-2 hover:bg-indigo-700"
        >
          {{ loading() ? 'Logging in…' : 'Log in' }}
        </button>
      </form>
      <p class="text-sm text-slate-500 mt-4">
        No account?
        <a routerLink="/register" class="text-indigo-600 hover:underline">Register</a>
      </p>
    </div>
  `
})
export class LoginComponent {
  loading = signal(false);
  error = signal<string | null>(null);

  private fb = inject(FormBuilder);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  constructor(private auth: AuthService, private router: Router) {}

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.error.set(null);
    const { email, password } = this.form.getRawValue();
    this.auth.login({ email: email!, password: password! }).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/tasks']);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Invalid email or password.');
      }
    });
  }
}
