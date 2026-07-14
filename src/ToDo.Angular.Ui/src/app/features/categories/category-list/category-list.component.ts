import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../../core/services/category.service';
import { ToDoCategoryResponseDTO } from '../../../core/models/models';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [FormsModule, CommonModule],
  template: `
    <h1 class="text-2xl font-bold text-slate-800 mb-4">Categories</h1>

    <div class="flex gap-2 mb-4">
      <label class="border rounded px-3 py-2 text-sm bg-slate-50 cursor-pointer hover:bg-slate-100 flex items-center justify-center w-32 h-10 text-slate-600 border-dashed relative overflow-hidden">
        @if (newIconPreview()) {
          <img [src]="newIconPreview()" class="w-full h-full object-cover absolute inset-0" />
        } @else {
          <span>📎 Choose Icon</span>
        }
        <input type="file" accept="image/*" (change)="onNewIconChange($event)" class="hidden" />
      </label>

      <input
        [(ngModel)]="newName"
        placeholder="New category name…"
        class="flex-1 border rounded px-3 py-2 text-sm"
        (keyup.enter)="create()"
      />
      <button (click)="create()" class="bg-indigo-600 text-white rounded px-4 py-2 text-sm hover:bg-indigo-700">
        Add
      </button>
    </div>

    <ul class="flex flex-col gap-2">
      @for (c of categories(); track c.id) {
        <li class="bg-white rounded shadow-sm border p-3 flex items-center gap-3">
          @if (editingId() === c.id) {
            <label class="border rounded px-2 py-1 text-sm bg-slate-50 cursor-pointer hover:bg-slate-100 text-slate-600 border-dashed relative w-24 h-10 overflow-hidden flex items-center justify-center">
              @if (editIconPreview()) {
                <img [src]="editIconPreview()" class="w-full h-full object-cover absolute inset-0" />
              } @else {
                <span class="text-xs text-center">Change Icon</span>
              }
              <input type="file" accept="image/*" (change)="onEditIconChange($event)" class="hidden" />
            </label>
            <input [(ngModel)]="editName" class="flex-1 border rounded px-2 py-1 text-sm" />
            <button (click)="saveEdit(c)" class="text-sm text-indigo-600 hover:underline font-bold">Save</button>
            <button (click)="cancelEdit()" class="text-sm text-slate-500 hover:underline">Cancel</button>
          } @else {
            <div class="w-8 h-8 flex items-center justify-center bg-slate-100 rounded overflow-hidden">
              @if (c.icon) {
                <img [src]="getIconUrl(c.icon)" class="w-full h-full object-cover" alt="category icon" />
              } @else {
                <span class="text-lg">📁</span>
              }
            </div>
            <span class="flex-1 font-medium text-slate-700">{{ c.name }}</span>
            <span class="text-xs bg-slate-100 text-slate-600 px-2 py-1 rounded-full">{{ c.tasks.length }} tasks</span>
            <button (click)="startEdit(c)" class="text-sm text-indigo-600 hover:underline">Edit</button>
            <button (click)="remove(c)" class="text-sm text-red-600 hover:underline">Delete</button>
          }
        </li>
      } @empty {
        <p class="text-slate-400 text-sm">No categories yet.</p>
      }
    </ul>
  `
})
export class CategoryListComponent implements OnInit {
  private sanitizer = inject(DomSanitizer);
  private categoryService: CategoryService = inject(CategoryService);

  categories = signal<ToDoCategoryResponseDTO[]>([]);
  newName = '';
  newIcon: File | null = null;
  newIconPreview = signal<string | SafeUrl | null>(null);

  editingId = signal<string | null>(null);
  editName = '';
  editIcon: File | null = null;
  editIconPreview = signal<string | SafeUrl | null>(null);
  
  private rawPreviewUrl: string | null = null;
  private rawPreviewEditUrl: string | null = null;


  constructor() {}

  ngOnInit(): void {
    this.refresh();
  }

  refresh(): void {
    this.categoryService.getAll().subscribe((cats) => this.categories.set(cats));
  }

  getIconUrl(iconPath: string | null | undefined): string {
    if (!iconPath) {
      return 'assets/default-icon.png';
    }
    if (iconPath.startsWith('http://') || iconPath.startsWith('https://') || iconPath.startsWith('assets/')) {
      return iconPath;
    }
    return `${environment.apiUrl}/${iconPath}`;
  }

  onNewIconChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.newIcon = input.files[0];

      this.revokeOldUrl();
      this.rawPreviewUrl = URL.createObjectURL(this.newIcon);

      const safeUrl = this.sanitizer.bypassSecurityTrustUrl(this.rawPreviewUrl);
      this.newIconPreview.set(safeUrl);

      input.value = '';
    }
  }

  onEditIconChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.editIcon = input.files[0];

      this.revokeOldUrl();
      this.rawPreviewEditUrl = URL.createObjectURL(this.editIcon);

      const safeUrl = this.sanitizer.bypassSecurityTrustUrl(this.rawPreviewEditUrl);
      this.editIconPreview.set(safeUrl);

      input.value = '';
    }
  }

  create(): void {
    const name = this.newName.trim();
    if (!name) return;

    this.categoryService.create({ name, icon: this.newIcon }).subscribe(() => {
      this.newName = '';
      this.newIcon = null;
      this.newIconPreview.set(null);
      this.refresh();
    });
  }

  startEdit(c: ToDoCategoryResponseDTO): void {
    this.editingId.set(c.id);
    this.editName = c.name;
    this.editIcon = null;
    this.editIconPreview.set(c.icon ? this.getIconUrl(c.icon) : null);
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.editIconPreview.set(null);
  }

  saveEdit(c: ToDoCategoryResponseDTO): void {
    const name = this.editName.trim();
    if (!name) return;

    this.categoryService.update(c.id, { name, icon: this.editIcon }).subscribe(() => {
      this.editingId.set(null);
      this.editIconPreview.set(null);
      this.refresh();
    });
  }

  remove(c: ToDoCategoryResponseDTO): void {
    if (!confirm(`Delete category "${c.name}"?`)) return;
    this.categoryService.delete(c.id).subscribe(() => this.refresh());
  }

  private revokeOldUrl(): void {
    if (this.rawPreviewUrl) {
      URL.revokeObjectURL(this.rawPreviewUrl);
      this.rawPreviewUrl = null;
    }
  }

  ngOnDestroy(): void {
    this.revokeOldUrl();
  }
}