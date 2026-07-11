import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ToDoStepCreateDTO, ToDoStepResponseDTO, ToDoStepUpdateDTO } from '../models/models';

@Injectable({ providedIn: 'root' })
export class StepService {
  private readonly baseUrl = `${environment.apiUrl}/ToDoSteps`;

  constructor(private http: HttpClient) {}

  getByTask(taskId: string): Observable<ToDoStepResponseDTO[]> {
    return this.http.get<ToDoStepResponseDTO[]>(`${this.baseUrl}/task/${taskId}`);
  }

  getById(id: string): Observable<ToDoStepResponseDTO> {
    return this.http.get<ToDoStepResponseDTO>(`${this.baseUrl}/${id}`);
  }

  create(payload: ToDoStepCreateDTO): Observable<ToDoStepResponseDTO> {
    return this.http.post<ToDoStepResponseDTO>(this.baseUrl, payload);
  }

  update(id: string, payload: ToDoStepUpdateDTO): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
