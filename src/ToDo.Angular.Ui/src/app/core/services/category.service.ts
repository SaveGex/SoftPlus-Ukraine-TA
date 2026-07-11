import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ToDoCategoryCreateDTO,
  ToDoCategoryResponseDTO,
  ToDoCategoryUpdateDTO
} from '../models/models';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly baseUrl = `${environment.apiUrl}/ToDoCategories`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<ToDoCategoryResponseDTO[]> {
    return this.http.get<ToDoCategoryResponseDTO[]>(this.baseUrl);
  }

  getById(id: string): Observable<ToDoCategoryResponseDTO> {
    return this.http.get<ToDoCategoryResponseDTO>(`${this.baseUrl}/${id}`);
  }

  getTasksForCategory(id: string): Observable<ToDoCategoryResponseDTO> {
    return this.http.get<ToDoCategoryResponseDTO>(`${this.baseUrl}/${id}/tasks`);
  }

  create(dto: ToDoCategoryCreateDTO): Observable<ToDoCategoryResponseDTO> {
    const formData = new FormData();
    formData.append('name', dto.name);
    if (dto.icon) {
      formData.append('icon', dto.icon, dto.icon.name);
    }
    return this.http.post<ToDoCategoryResponseDTO>(this.baseUrl, formData);
  }

  update(id: string, dto: ToDoCategoryUpdateDTO): Observable<void> {
    const formData = new FormData();
    formData.append('name', dto.name);
    if (dto.icon) {
      formData.append('icon', dto.icon, dto.icon.name);
    }
    return this.http.put<void>(`${this.baseUrl}/${id}`, formData);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
