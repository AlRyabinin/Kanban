import { useState, useRef, useEffect  } from 'react';
import type { TaskDto } from '../../types';
import { Calendar, MoreVertical, Trash2, Pencil } from 'lucide-react';

interface TaskCardProps {
  task: TaskDto;
  isDragging?: boolean;
  onDelete?: (taskId: string) => void;
  onEdit?: (task: TaskDto) => void;  // ← Добавьте это
}

export function TaskCard({ task, isDragging, onDelete, onEdit }: TaskCardProps) {
  const [showMenu, setShowMenu] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setShowMenu(false);
      }
    }

    if (showMenu) {
      document.addEventListener('mousedown', handleClickOutside);
    }

    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [showMenu]);

  const formatDate = (dateString: string | null) => {
    if (!dateString) return null;
    const date = new Date(dateString);
    return date.toLocaleDateString('ru-RU', {
      day: 'numeric',
      month: 'short',
    });
  };

  const handleDelete = () => {
    if (onDelete) {
      onDelete(task.id);
    }
    setShowMenu(false);
  };

  const handleEdit = () => {
    if (onEdit) {
      onEdit(task);
    }
    setShowMenu(false);
  };

  return (
    <div
      className={`relative rounded-lg border border-gray-200 bg-white p-3 shadow-sm transition-shadow hover:shadow-md ${
        isDragging ? 'opacity-50 rotate-2' : ''
      }`}
    >
      {/* Меню действий */}
      <div className="absolute right-2 top-2" ref={menuRef}>
        <button
          onClick={() => setShowMenu(!showMenu)}
          className="rounded p-1 text-gray-400 hover:bg-gray-100 hover:text-gray-600"
        >
          <MoreVertical className="h-4 w-4" />
        </button>

        {showMenu && (
          <div className="absolute right-0 top-8 z-10 w-40 rounded-lg border border-gray-200 bg-white py-1 shadow-lg">
            <button
              onClick={handleEdit}
              className="flex w-full items-center gap-2 px-3 py-2 text-sm text-gray-700 hover:bg-gray-50"
            >
              <Pencil className="h-4 w-4" />
              Редактировать
            </button>
            <button
              onClick={handleDelete}
              className="flex w-full items-center gap-2 px-3 py-2 text-sm text-red-600 hover:bg-red-50"
            >
              <Trash2 className="h-4 w-4" />
              Удалить
            </button>
          </div>
        )}
      </div>

      {/* Заголовок */}
      <h3 className="mb-1 pr-6 text-sm font-semibold text-gray-900">{task.title}</h3>

      {/* Описание */}
      {task.description && (
        <p className="mb-2 text-xs text-gray-600 line-clamp-2">{task.description}</p>
      )}

      {/* Дедлайн */}
      {task.dueDate && (
        <div className="flex items-center gap-1 text-xs text-gray-500">
          <Calendar className="h-3 w-3" />
          <span>{formatDate(task.dueDate)}</span>
        </div>
      )}
    </div>
  );
}