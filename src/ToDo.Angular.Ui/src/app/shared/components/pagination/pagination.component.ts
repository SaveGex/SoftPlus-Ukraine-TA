import { Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  template: `
    <div class="flex items-center justify-between mt-4 text-sm">
      <span class="text-slate-500">
        Page {{ page() }} of {{ totalPages() }} ({{ totalItems() }} items)
      </span>
      <div class="flex gap-2">
        <button
          class="px-3 py-1 rounded border disabled:opacity-40"
          [disabled]="page() <= 1"
          (click)="pageChange.emit(page() - 1)"
        >
          Prev
        </button>
        <button
          class="px-3 py-1 rounded border disabled:opacity-40"
          [disabled]="page() >= totalPages()"
          (click)="pageChange.emit(page() + 1)"
        >
          Next
        </button>
      </div>
    </div>
  `
})
export class PaginationComponent {
  page = input.required<number>();
  pageSize = input.required<number>();
  totalItems = input.required<number>();
  pageChange = output<number>();

  totalPages = computed(() => Math.max(1, Math.ceil(this.totalItems() / this.pageSize())));
}
