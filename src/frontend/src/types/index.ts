export interface TaskDto {
  id: string;
  title: string;
  description: string | null;
  columnId: string;
  orderIndex: number;
  dueDate: string | null;
}

export interface ColumnWithTasksDto {
  id: string;
  name: string;
  color: string | null;
  orderIndex: number;
  tasks: TaskDto[];
}

export interface BoardWithColumnsDto {
  id: string;
  name: string;
  workspaceId: string;
  columns: ColumnWithTasksDto[];
}

export interface CreateTaskRequest {
  title: string;
  description?: string;
  columnId: string;
  orderIndex: number;
  dueDate?: string;
}

export interface UpdateTaskPositionRequest {
  taskId: string;
  newColumnId: string;
  newOrderIndex: number;
}

export interface BoardInfo {
  id: string;
  name: string;
}