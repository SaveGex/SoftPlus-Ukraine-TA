import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { TaskService } from '../../../core/services/task.service';
import { StepService } from '../../../core/services/step.service';
import { RecurrenceLabels, ToDoStepResponseDTO, ToDoTaskResponseDTO } from '../../../core/models/models';

@Component({
  selector: 'app-task-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],

  template: `
    @if (task(); as t) {
      <div class="bg-white rounded-lg shadow p-6 max-w-xl mx-auto">
        <div class="flex items-start justify-between">
          <div>
            <h1 class="text-2xl font-bold text-slate-800" [class.line-through]="t.isCompleted">
              {{ t.title }}
            </h1>
            @if (t.note) {
              <p class="text-slate-500 mt-1">{{ t.note }}</p>
            }
          </div>
          <div class="flex gap-2 shrink-0">
            <a [routerLink]="['/tasks', t.id, 'edit']" class="text-sm text-indigo-600 hover:underline">Edit</a>
            <button (click)="deleteTask()" class="text-sm text-red-600 hover:underline">Delete</button>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-2 mt-4 text-sm text-slate-600">
          <p>Important: {{ t.isImportant ? 'Yes' : 'No' }}</p>
          <p>My Day: {{ t.isMyDay ? 'Yes' : 'No' }}</p>
          <p>Due: {{ t.dueDate ? (t.dueDate | slice: 0:10) : '—' }}</p>
          <p>Reminder: {{ t.reminderAt ? (t.reminderAt | slice: 0:16) : '—' }}</p>
          <p>Recurrence: {{ recurrenceLabel(t.recurrence) }}</p>
          <p>Completed: {{ t.isCompleted ? 'Yes' : 'No' }}</p>
        </div>

        <hr class="my-4" />

        <h2 class="font-semibold text-slate-700 mb-2">Steps</h2>
        <ul class="flex flex-col gap-2 mb-3">
          @for (s of steps(); track s.id) {
            <li class="flex items-center gap-2 border rounded px-3 py-2">
              <input
                type="checkbox"
                [checked]="s.isCompleted"
                (change)="toggleStep(s)"
              />
              <span class="flex-1" [class.line-through]="s.isCompleted" [class.text-slate-400]="s.isCompleted">
                {{ s.title }}
              </span>
              <button (click)="deleteStep(s)" class="text-red-500 text-xs hover:underline">Remove</button>
            </li>
          }
          @empty {
            <p class="text-slate-400 text-sm">No steps yet.</p>
          }
        </ul>

        <div class="flex gap-2">
          <input
            [(ngModel)]="newStepTitle"
            placeholder="Add a step…"
            class="flex-1 border rounded px-3 py-2 text-sm"
            (keyup.enter)="addStep()"
          />
          <button (click)="addStep()" class="bg-indigo-600 text-white rounded px-4 py-2 text-sm hover:bg-indigo-700">
            Add
          </button>
        </div>
      </div>
    } @else {
      <p class="text-slate-500">Loading task…</p>
    }
  `
})
export class TaskDetailComponent implements OnInit {
  task = signal<ToDoTaskResponseDTO | null>(null);
  steps = signal<ToDoStepResponseDTO[]>([]);
  newStepTitle = '';
  private taskId = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private taskService: TaskService,
    private stepService: StepService
  ) {}

  ngOnInit(): void {
    this.taskId = this.route.snapshot.paramMap.get('id')!;
    this.loadTask();
  }

  loadTask(): void {
    this.taskService.getDetails(this.taskId).subscribe((t) => {
      this.task.set(t);
      this.steps.set(t.steps ?? []);
    });
  }

  recurrenceLabel(value: number): string {
    return RecurrenceLabels[value as keyof typeof RecurrenceLabels] ?? 'None';
  }

  addStep(): void {
    const title = this.newStepTitle.trim();
    if (!title) return;
    this.stepService.create({ title, todoTaskId: this.taskId }).subscribe((step) => {
      this.steps.update((list) => [...list, step]);
      this.newStepTitle = '';
    });
  }

  toggleStep(step: ToDoStepResponseDTO): void {
    const updated = { title: step.title, isCompleted: !step.isCompleted };
    this.stepService.update(step.id, updated).subscribe(() => {
      this.steps.update((list) =>
        list.map((s) => (s.id === step.id ? { ...s, isCompleted: updated.isCompleted } : s))
      );
    });
  }

  deleteStep(step: ToDoStepResponseDTO): void {
    this.stepService.delete(step.id).subscribe(() => {
      this.steps.update((list) => list.filter((s) => s.id !== step.id));
    });
  }

  deleteTask(): void {
    if (!confirm('Delete this task?')) return;
    this.taskService.delete(this.taskId).subscribe(() => this.router.navigate(['/tasks']));
  }
}
