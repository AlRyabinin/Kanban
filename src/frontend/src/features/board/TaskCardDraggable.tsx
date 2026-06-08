import { useSortable } from '@dnd-kit/sortable';
import { CSS } from '@dnd-kit/utilities';
import type { TaskDto } from '../../types';
import { TaskCard } from './TaskCard';

interface TaskCardDraggableProps {
  task: TaskDto;
  onDelete?: (taskId: string) => void;
  onEdit?: (task: TaskDto) => void;
}

export function TaskCardDraggable({ task, onDelete, onEdit }: TaskCardDraggableProps) {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id: task.id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
    opacity: isDragging ? 0.5 : 1,
  };

  return (
    <div ref={setNodeRef} style={style} {...attributes} {...listeners}>
      <TaskCard 
        task={task} 
        isDragging={isDragging} 
        onDelete={onDelete}
        onEdit={onEdit}
      />
    </div>
  );
}