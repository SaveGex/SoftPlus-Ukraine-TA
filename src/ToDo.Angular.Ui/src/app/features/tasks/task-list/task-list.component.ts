import { Component, OnInit, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { CategoryService } from '../../../core/services/category.service';
import { ToDoCategoryResponseDTO, ToDoTaskResponseDTO } from '../../../core/models/models';
import { PaginationComponent } from '../../../shared/components/pagination/pagination.component';

const PAGE_SIZE = 6;

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [FormsModule, RouterLink, PaginationComponent],
  template: `
    <div class="flex items-center justify-between mb-4">
      <h1 class="text-2xl font-bold text-slate-800">My Tasks</h1>
      <a
        routerLink="/tasks/new"
        class="bg-indigo-600 hover:bg-indigo-700 text-white text-sm font-medium px-4 py-2 rounded"
      >
        + New task
      </a>
    </div>

    <div class="flex flex-wrap gap-3 mb-4">
      <input
        type="text"
        placeholder="Search tasks…"
        class="flex-1 min-w-[200px] border rounded px-3 py-2 text-sm"
        [ngModel]="search()"
        (ngModelChange)="onSearchChange($event)"
      />
      <select
        class="border rounded px-3 py-2 text-sm"
        [ngModel]="categoryFilter()"
        (ngModelChange)="onCategoryFilterChange($event)"
      >
        <option [ngValue]="null">All categories</option>
        @for (c of categories(); track c.id) {
          <option [ngValue]="c.id">{{ c.name }}</option>
        }
      </select>
    </div>

    @if (loading()) {
      <p class="text-slate-500">Loading tasks…</p>
    } @else if (pagedTasks().length === 0) {
      <p class="text-slate-500">No tasks match your search.</p>
    } @else {
      <ul class="flex flex-col gap-2">
        @for (t of pagedTasks(); track t.id) {
          <li
            class="bg-white rounded shadow-sm border p-3 flex items-center justify-between hover:shadow transition"
          >
            <a [routerLink]="['/tasks', t.id]" class="flex-1 flex items-center gap-3 min-w-0">
              <span
                class="w-2.5 h-2.5 rounded-full shrink-0"
                [class.bg-green-500]="t.isCompleted"
                [class.bg-amber-400]="!t.isCompleted"
              ></span>
              <span class="truncate" [class.line-through]="t.isCompleted" [class.text-slate-400]="t.isCompleted">
                {{ t.title }}
              </span>
              @if (t.isImportant) {
                <span class="text-amber-500 text-xs">★</span>
              }
            </a>
            <span class="text-xs text-slate-400 shrink-0 ml-3">
              {{ categoryName(t.categoryId) }}
            </span>
          </li>
        }
      </ul>

      <app-pagination
        [page]="page()"
        [pageSize]="pageSize"
        [totalItems]="filteredTasks().length"
        (pageChange)="page.set($event)"
      />
    }
  `
})
export class TaskListComponent implements OnInit {
  pageSize = PAGE_SIZE;

  allTasks = signal<ToDoTaskResponseDTO[]>([]);
  categories = signal<ToDoCategoryResponseDTO[]>([]);
  loading = signal(true);

  search = signal('');
  categoryFilter = signal<string | null>(null);
  page = signal(1);

  filteredTasks = computed(() => {
    const term = this.search().trim().toLowerCase();
    const catId = this.categoryFilter();
    return this.allTasks().filter((t) => {
      const matchesSearch =
        !term ||
        t.title.toLowerCase().includes(term) ||
        (t.note ?? '').toLowerCase().includes(term);
      const matchesCategory = !catId || t.categoryId === catId;
      return matchesSearch && matchesCategory;
    });
  });

  pagedTasks = computed(() => {
    const start = (this.page() - 1) * this.pageSize;
    return this.filteredTasks().slice(start, start + this.pageSize);
  });

  constructor(private taskService: TaskService, private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.categoryService.getAll().subscribe((cats) => this.categories.set(cats));
    this.taskService.getAll().subscribe({
      next: (tasks) => {
        this.allTasks.set(tasks);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  onSearchChange(value: string): void {
    this.search.set(value);
    this.page.set(1);
  }

  onCategoryFilterChange(value: string | null): void {
    this.categoryFilter.set(value);
    this.page.set(1);
  }

  categoryName(categoryId: string | null): string {
    if (!categoryId) return '';
    return this.categories().find((c) => c.id === categoryId)?.name ?? '';
  }
}
