import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { CategoryService } from '../../../core/services/category.service';
import { RecurrenceLabels, RecurrenceType, ToDoCategoryResponseDTO } from '../../../core/models/models';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="max-w-lg mx-auto bg-white rounded-lg shadow p-6">
      <h1 class="text-xl font-bold mb-4 text-slate-800">{{ isEdit() ? 'Edit task' : 'New task' }}</h1>
      <form [formGroup]="form" (ngSubmit)="submit()" class="flex flex-col gap-3">
        <label class="text-sm font-medium text-slate-600">
          Title
          <input formControlName="title" class="mt-1 w-full border rounded px-3 py-2" />
        </label>

        <label class="text-sm font-medium text-slate-600">
          Note
          <textarea formControlName="note" rows="3" class="mt-1 w-full border rounded px-3 py-2"></textarea>
        </label>

        <label class="text-sm font-medium text-slate-600">
          Category
          <select formControlName="categoryId" class="mt-1 w-full border rounded px-3 py-2">
            <option [ngValue]="null">No category</option>
            @for (c of categories(); track c.id) {
              <option [ngValue]="c.id">{{ c.name }}</option>
            }
          </select>
        </label>

        <div class="grid grid-cols-2 gap-3">
          <label class="text-sm font-medium text-slate-600">
            Due date
            <input formControlName="dueDate" type="date" class="mt-1 w-full border rounded px-3 py-2" />
          </label>
          <label class="text-sm font-medium text-slate-600">
            Reminder
            <input formControlName="reminderAt" type="datetime-local" class="mt-1 w-full border rounded px-3 py-2" />
          </label>
        </div>

        <label class="text-sm font-medium text-slate-600">
          Recurrence
          <select formControlName="recurrence" class="mt-1 w-full border rounded px-3 py-2">
            @for (opt of recurrenceOptions; track opt.value) {
              <option [ngValue]="opt.value">{{ opt.label }}</option>
            }
          </select>
        </label>

        <div class="flex gap-6 mt-1">
          <label class="flex items-center gap-2 text-sm text-slate-600">
            <input type="checkbox" formControlName="isImportant" /> Important
          </label>
          <label class="flex items-center gap-2 text-sm text-slate-600">
            <input type="checkbox" formControlName="isMyDay" /> My Day
          </label>
          @if (isEdit()) {
            <label class="flex items-center gap-2 text-sm text-slate-600">
              <input type="checkbox" formControlName="isCompleted" /> Completed
            </label>
          }
        </div>

        @if (error()) {
          <p class="text-red-600 text-sm">{{ error() }}</p>
        }

        <div class="flex gap-2 mt-2">
          <button
            type="submit"
            [disabled]="form.invalid || saving()"
            class="bg-indigo-600 disabled:opacity-50 text-white rounded px-4 py-2 hover:bg-indigo-700"
          >
            {{ saving() ? 'Saving…' : 'Save' }}
          </button>
          <button type="button" (click)="cancel()" class="border rounded px-4 py-2 text-slate-600">
            Cancel
          </button>
        </div>
      </form>
    </div>
  `
})
export class TaskFormComponent implements OnInit {
  categories = signal<ToDoCategoryResponseDTO[]>([]);
  saving = signal(false);
  error = signal<string | null>(null);
  isEdit = signal(false);
  private taskId: string | null = null;

  recurrenceOptions = Object.values(RecurrenceType)
    .filter((v) => typeof v === 'number')
    .map((v) => ({ value: v as RecurrenceType, label: RecurrenceLabels[v as RecurrenceType] }));

  private fb = inject(FormBuilder);

  form = this.fb.group({
    title: ['', Validators.required],
    note: [''],
    categoryId: this.fb.control<string | null>(null),
    dueDate: [''],
    reminderAt: [''],
    recurrence: [RecurrenceType.None, Validators.required],
    isImportant: [false],
    isMyDay: [false],
    isCompleted: [false]
  });

  constructor(
    private taskService: TaskService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.categoryService.getAll().subscribe((cats) => this.categories.set(cats));

    this.taskId = this.route.snapshot.paramMap.get('id');
    if (this.taskId) {
      this.isEdit.set(true);
      this.taskService.getById(this.taskId).subscribe((task) => {
        this.form.patchValue({
          title: task.title,
          note: task.note ?? '',
          categoryId: task.categoryId,
          dueDate: task.dueDate ? task.dueDate.substring(0, 10) : '',
          reminderAt: task.reminderAt ? task.reminderAt.substring(0, 16) : '',
          recurrence: task.recurrence,
          isImportant: task.isImportant,
          isMyDay: task.isMyDay,
          isCompleted: task.isCompleted
        });
      });
    }
  }

  submit(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    this.error.set(null);
    const v = this.form.getRawValue();

    const dueDate = v.dueDate ? new Date(v.dueDate).toISOString() : null;
    const reminderAt = v.reminderAt ? new Date(v.reminderAt).toISOString() : null;

    if (this.isEdit() && this.taskId) {
      this.taskService
        .update(this.taskId, {
          title: v.title!,
          note: v.note || null,
          isCompleted: !!v.isCompleted,
          isImportant: !!v.isImportant,
          isMyDay: !!v.isMyDay,
          reminderAt,
          dueDate,
          recurrence: v.recurrence!,
          categoryId: v.categoryId ?? null
        })
        .subscribe({
          next: () => this.router.navigate(['/tasks', this.taskId]),
          error: () => this.onError()
        });
    } else {
      this.taskService
        .create({
          title: v.title!,
          note: v.note || null,
          isImportant: !!v.isImportant,
          isMyDay: !!v.isMyDay,
          reminderAt,
          dueDate,
          recurrence: v.recurrence!,
          categoryId: v.categoryId ?? null
        })
        .subscribe({
          next: (created) => this.router.navigate(['/tasks', created.id]),
          error: () => this.onError()
        });
    }
  }

  private onError(): void {
    this.saving.set(false);
    this.error.set('Could not save the task. Please try again.');
  }

  cancel(): void {
    this.router.navigate(['/tasks']);
  }
}
