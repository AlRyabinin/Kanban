import { useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import api from '../../lib/api';
import type { BoardWithColumnsDto, ColumnWithTasksDto, TaskDto } from '../../types';
import { ColumnDraggable } from './ColumnDraggable';
import { CreateTaskModal } from './CreateTaskModal';
import { EditTaskModal } from './EditTaskModal';
import { CreateColumnModal } from './CreateColumnModal';
import { EditColumnModal } from './EditColumnModal';
import { BoardDndContext } from './BoardDndContext';
import { Toaster, toast } from 'sonner';
import { Plus } from 'lucide-react';

export function BoardPage() {
  const { boardId } = useParams<{ boardId: string }>();
  const queryClient = useQueryClient();
  const [isTaskModalOpen, setIsTaskModalOpen] = useState(false);
  const [selectedColumnId, setSelectedColumnId] = useState<string>('');
  const [editingTask, setEditingTask] = useState<TaskDto | null>(null);
  const [isColumnModalOpen, setIsColumnModalOpen] = useState(false);
  const [editingColumn, setEditingColumn] = useState<ColumnWithTasksDto | null>(null);

  const { data: board, isLoading, error } = useQuery<BoardWithColumnsDto>({
    queryKey: ['board', boardId],
    queryFn: async () => {
      const response = await api.get<BoardWithColumnsDto>(`/Boards/${boardId}/full`);
      return response.data;
    },
    enabled: !!boardId,
    select: (data) => ({
      ...data,
      columns: data.columns
        .sort((a, b) => a.orderIndex - b.orderIndex)
        .map((col) => ({
          ...col,
          tasks: col.tasks.sort((x, y) => x.orderIndex - y.orderIndex),
        })),
    }),
  });

  const handleCreateTask = (columnId: string) => {
    setSelectedColumnId(columnId);
    setIsTaskModalOpen(true);
  };

  const handleTaskDragEnd = async (
  taskId: string,
  newColumnId: string,
  newOrderIndex: number
) => {
  try {
    await api.put('/Tasks/position', {
      taskId,
      newColumnId,
      newOrderIndex,
    });
    await queryClient.invalidateQueries({ queryKey: ['board', boardId] });
  } catch (error) {
    console.error('Ошибка при перемещении задачи:', error);
    toast.error('Не удалось переместить задачу');
  }
};

  const handleColumnDragEnd = async (newColumns: ColumnWithTasksDto[]) => {
    try {
      const updatePromises = newColumns.map((column, index) =>
        api.put(`/Columns/${column.id}/position`, {
          columnId: column.id,
          newOrderIndex: index,
        })
      );

      await Promise.all(updatePromises);
      await queryClient.invalidateQueries({ queryKey: ['board', boardId] });
      toast.success('Колонки перемещены');
    } catch (error) {
      console.error('Ошибка при перемещении колонок:', error);
      toast.error('Не удалось переместить колонки');
    }
  };

  const handleDeleteTask = async (taskId: string) => {
    try {
      await api.delete(`/Tasks/${taskId}`);
      await queryClient.invalidateQueries({ queryKey: ['board', boardId] });
      toast.success('Задача удалена');
    } catch (error) {
      console.error('Ошибка при удалении:', error);
      toast.error('Не удалось удалить задачу');
    }
  };

  const handleEditTask = (task: TaskDto) => {
    setEditingTask(task);
  };

  const handleEditColumn = (column: ColumnWithTasksDto) => {
    setEditingColumn(column);
  };

  const handleDeleteColumn = async (columnId: string) => {
    if (window.confirm('Вы уверены, что хотите удалить эту колонку? Все задачи в ней будут удалены.')) {
      try {
        await api.delete(`/Columns/${columnId}`);
        await queryClient.invalidateQueries({ queryKey: ['board', boardId] });
        toast.success('Колонка удалена');
      } catch (error) {
        console.error('Ошибка при удалении:', error);
        toast.error('Не удалось удалить колонку');
      }
    }
  };

  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p className="text-gray-500">Загрузка доски...</p>
      </div>
    );
  }

  if (error || !board) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <p className="text-red-500">Ошибка загрузки доски</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Toaster position="top-right" richColors />

      <main className="h-full overflow-x-auto overflow-y-auto p-6">
        <BoardDndContext
          board={board}
          onTaskDragEnd={handleTaskDragEnd}
          onColumnDragEnd={handleColumnDragEnd}
        >
          <div className="flex gap-6 h-full">
            {board.columns.map((column) => (
              <ColumnDraggable
                key={column.id}
                column={column}
                onCreateTask={() => handleCreateTask(column.id)}
                onDeleteTask={handleDeleteTask}
                onEditTask={handleEditTask}
                onEditColumn={handleEditColumn}
                onDeleteColumn={handleDeleteColumn}
              />
            ))}

            <button
              onClick={() => setIsColumnModalOpen(true)}
              className="flex-shrink-0 w-80 h-14 rounded-lg border-2 border-dashed border-gray-300 bg-gray-50 text-gray-500 transition-colors hover:border-blue-400 hover:bg-blue-50 hover:text-blue-600 flex items-center justify-center gap-2"
            >
              <Plus className="h-5 w-5" />
              <span className="font-medium">Добавить колонку</span>
            </button>
          </div>
        </BoardDndContext>
      </main>

      <CreateTaskModal
        isOpen={isTaskModalOpen}
        onClose={() => setIsTaskModalOpen(false)}
        columnId={selectedColumnId}
      />

      <EditTaskModal
        isOpen={!!editingTask}
        onClose={() => setEditingTask(null)}
        task={editingTask}
      />

      <CreateColumnModal
        isOpen={isColumnModalOpen}
        onClose={() => setIsColumnModalOpen(false)}
        boardId={boardId || ''}
      />

      <EditColumnModal
        isOpen={!!editingColumn}
        onClose={() => setEditingColumn(null)}
        column={editingColumn}
      />
    </div>
  );
}