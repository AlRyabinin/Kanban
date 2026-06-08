import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import type { ColumnWithTasksDto, TaskDto } from '../../types';
import { Column } from './Column';

interface ColumnDraggableProps {
  column: ColumnWithTasksDto;
  onCreateTask: () => void;
  onDeleteTask: (taskId: string) => void;
  onEditTask: (task: TaskDto) => void;
  onEditColumn: (column: ColumnWithTasksDto) => void;
  onDeleteColumn: (columnId: string) => void;
}

export function ColumnDraggable({
  column,
  onCreateTask,
  onDeleteTask,
  onEditTask,
  onEditColumn,
  onDeleteColumn,
}: ColumnDraggableProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ 
    id: column.id,
    data: { type: 'column' }
  });

  const style = {
    transform: CSS.Translate.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div 
      ref={setNodeRef} 
      style={{ ...style, height: '100%' }}
      className="flex-shrink-0"
    >
      {/* Передаём listeners и attributes в Column для заголовка */}
      <Column
        column={column}
        onCreateTask={onCreateTask}
        onDeleteTask={onDeleteTask}
        onEditTask={onEditTask}
        onEditColumn={onEditColumn}
        onDeleteColumn={onDeleteColumn}
        //  Передаём drag handle только для заголовка
        columnDragAttributes={attributes}
        columnDragListeners={listeners}
      />
    </div>
  );
}