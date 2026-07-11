import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <div class="max-w-sm mx-auto mt-16 bg-white rounded-lg shadow p-6">
      <h1 class="text-2xl font-bold mb-4 text-slate-800">Create account</h1>
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
        @if (success()) {
          <p class="text-green-600 text-sm">Account created — you can log in now.</p>
        }
        <button
          type="submit"
          [disabled]="form.invalid || loading()"
          class="bg-indigo-600 disabled:opacity-50 text-white rounded py-2 mt-2 hover:bg-indigo-700"
        >
          {{ loading() ? 'Creating…' : 'Register' }}
        </button>
      </form>
      <p class="text-sm text-slate-500 mt-4">
        Already have an account?
        <a routerLink="/login" class="text-indigo-600 hover:underline">Log in</a>
      </p>
    </div>
  `
})
export class RegisterComponent {
  loading = signal(false);
  error = signal<string | null>(null);
  success = signal(false);

  private fb = inject(FormBuilder);

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  constructor(private auth: AuthService, private router: Router) {}

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.error.set(null);
    const { email, password } = this.form.getRawValue();
    this.auth.register({ email: email!, password: password! }).subscribe({
      next: () => {
        this.loading.set(false);
        this.success.set(true);
        setTimeout(() => this.router.navigate(['/login']), 1200);
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Registration failed. Try a different email.');
      }
    });
  }
}
