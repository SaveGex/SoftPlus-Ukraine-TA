export enum RecurrenceType {
  None = 0,
  Daily = 1,
  Weekly = 2,
  Monthly = 3,
  Yearly = 4
}

export const RecurrenceLabels: Record<RecurrenceType, string> = {
  [RecurrenceType.None]: 'None',
  [RecurrenceType.Daily]: 'Daily',
  [RecurrenceType.Weekly]: 'Weekly',
  [RecurrenceType.Monthly]: 'Monthly',
  [RecurrenceType.Yearly]: 'Yearly'
};

export interface RegisterRequestDTO {
  email: string;
  password: string;
}

export interface LoginRequestDTO {
  email: string;
  password: string;
}

export interface AuthResponseDTO{
  isSuccess: boolean;
  token: string;
  message: string;
}


export interface ToDoCategoryCreateDTO {
  name: string;
  icon: File | null;
}

export interface ToDoCategoryUpdateDTO {
  name: string;
  icon: File | null;
}

export interface ToDoCategoryResponseDTO {
  id: string;
  name: string;
  icon: string | null;
  tasks: ToDoTaskResponseDTO[];
}

export interface ToDoStepCreateDTO {
  title: string;
  todoTaskId: string;
}

export interface ToDoStepUpdateDTO {
  title: string;
  isCompleted: boolean;
}

export interface ToDoStepResponseDTO {
  id: string;
  title: string;
  isCompleted: boolean;
  todoTaskId: string;
}

export interface ToDoTaskCreateDTO {
  title: string;
  note: string | null;
  isImportant: boolean;
  isMyDay: boolean;
  reminderAt: string | null;
  dueDate: string | null;
  recurrence: RecurrenceType;
  categoryId: string | null;
}

export interface ToDoTaskUpdateDTO {
  title: string;
  note: string | null;
  isCompleted: boolean;
  isImportant: boolean;
  isMyDay: boolean;
  reminderAt: string | null;
  dueDate: string | null;
  recurrence: RecurrenceType;
  categoryId: string | null;
}

export interface ToDoTaskResponseDTO {
  id: string;
  title: string;
  note: string | null;
  isCompleted: boolean;
  isImportant: boolean;
  isMyDay: boolean;
  reminderAt: string | null;
  dueDate: string | null;
  createdAt: string;
  recurrence: RecurrenceType;
  categoryId: string | null;
  userId: string;
  steps: ToDoStepResponseDTO[];
}
