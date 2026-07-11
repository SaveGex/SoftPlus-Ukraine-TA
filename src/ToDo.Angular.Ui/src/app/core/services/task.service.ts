import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ToDoTaskCreateDTO, ToDoTaskResponseDTO, ToDoTaskUpdateDTO } from '../models/models';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly baseUrl = `${environment.apiUrl}/ToDoTasks`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ToDoTaskResponseDTO[]> {
    return this.http.get<ToDoTaskResponseDTO[]>(this.baseUrl);
  }

  getMyDay(): Observable<ToDoTaskResponseDTO[]> {
    return this.http.get<ToDoTaskResponseDTO[]>(`${this.baseUrl}/my-day`);
  }

  getById(id: string): Observable<ToDoTaskResponseDTO> {
    return this.http.get<ToDoTaskResponseDTO>(`${this.baseUrl}/${id}`);
  }

  getDetails(id: string): Observable<ToDoTaskResponseDTO> {
    return this.http.get<ToDoTaskResponseDTO>(`${this.baseUrl}/${id}/details`);
  }

  create(payload: ToDoTaskCreateDTO): Observable<ToDoTaskResponseDTO> {
    return this.http.post<ToDoTaskResponseDTO>(this.baseUrl, payload);
  }

  update(id: string, payload: ToDoTaskUpdateDTO): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
