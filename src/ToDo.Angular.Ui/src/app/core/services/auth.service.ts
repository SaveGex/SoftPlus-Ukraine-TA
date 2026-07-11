import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponseDTO, LoginRequestDTO, RegisterRequestDTO } from '../models/models';

const TOKEN_KEY = 'todo_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = `${environment.apiUrl}/Auth`;

  token = signal<string | null>(localStorage.getItem(TOKEN_KEY));

  isAuthenticated = computed(() => !!this.token());

  constructor(private http: HttpClient) {}


  register(payload: RegisterRequestDTO): Observable<AuthResponseDTO> {
    return this.http.post<AuthResponseDTO>(`${this.baseUrl}/register`, payload).pipe(
      tap(response => {
        if (response.isSuccess && response.token) {
          this.saveToken(response.token);
        }
      })
    );
  }

  login(payload: LoginRequestDTO): Observable<AuthResponseDTO> {
    return this.http.post<AuthResponseDTO>(`${this.baseUrl}/login`, payload).pipe(
      tap(response => {
        if (response.isSuccess && response.token) {
          this.saveToken(response.token);
        }
      })
    );
  }

  logout(): void {
    this.token.set(null);
    localStorage.removeItem(TOKEN_KEY);
  }

  private saveToken(token: string): void {
    this.token.set(token);
    localStorage.setItem(TOKEN_KEY, token);
  }
}
