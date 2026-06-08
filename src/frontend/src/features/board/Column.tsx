import { useDroppable } from '@dnd-kit/core';
import {
  SortableContext,
  verticalListSortingStrategy,
} from '@dnd-kit/sortable';
import type { ColumnWithTasksDto, TaskDto } from '../../types';
import { TaskCardDraggable } from './TaskCardDraggable';
import { Plus, MoreVertical, Pencil, Trash2, GripVertical } from 'lucide-react';
import { useState, useRef, useEffect, type HTMLAttributes, useContext } from 'react';
import { ActiveColumnContext } from './BoardDndContext';

interface ColumnProps {
  column: ColumnWithTasksDto;
  onCreateTask: () => void;
  onDeleteTask: (taskId: string) => void;
  onEditTask: (task: TaskDto) => void;
  onEditColumn: (column: ColumnWithTasksDto) => void;
  onDeleteColumn: (columnId: string) => void;
  columnDragAttributes?: HTMLAttributes<HTMLElement>;
  columnDragListeners?: HTMLAttributes<HTMLElement>;
}

export function Column({ 
  column, 
  onCreateTask, 
  onDeleteTask, 
  onEditTask,
  onEditColumn,
  onDeleteColumn,
  columnDragAttributes = {},
  columnDragListeners = {},
}: ColumnProps) {
  const { setNodeRef, isOver } = useDroppable({
    id: column.id,
    data: {
      type: 'column',
      columnId: column.id,
    },
  });

  const activeColumnId = useContext(ActiveColumnContext);
  const isColumnActive = isOver || activeColumnId === column.id;

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

  return (
    <div 
      className="flex w-80 flex-col"
      style={{ height: '100%' }}
    >
      {/* Header с drag handle */}
      <div 
        className="mb-3 flex cursor-grab items-center justify-between px-2 pt-2 active:cursor-grabbing"
        {...columnDragAttributes}
        {...columnDragListeners}
      >
        <div className="flex items-center gap-2">
          <GripVertical className="h-4 w-4 text-gray-400" />
          {column.color && (
            <div
              className="h-3 w-3 rounded-full"
              style={{ backgroundColor: column.color }}
            />
          )}
          <h2 className="text-sm font-semibold text-gray-700">{column.name}</h2>
        </div>
        <div className="flex items-center gap-2">
          <span className="rounded-full bg-gray-200 px-2 py-0.5 text-xs font-medium text-gray-600">
            {column.tasks.length}
          </span>
          
          <div className="relative" ref={menuRef}>
            <button
              onClick={() => setShowMenu(!showMenu)}
              className="rounded p-1 text-gray-400 hover:bg-gray-200 hover:text-gray-600"
            >
              <MoreVertical className="h-4 w-4" />
            </button>

            {showMenu && (
              <div className="absolute right-0 top-8 z-10 w-40 rounded-lg border border-gray-200 bg-white py-1 shadow-lg">
                <button
                  onClick={() => {
                    onEditColumn(column);
                    setShowMenu(false);
                  }}
                  className="flex w-full items-center gap-2 px-3 py-2 text-sm text-gray-700 hover:bg-gray-50"
                >
                  <Pencil className="h-4 w-4" />
                  Редактировать
                </button>
                <button
                  onClick={() => {
                    onDeleteColumn(column.id);
                    setShowMenu(false);
                  }}
                  className="flex w-full items-center gap-2 px-3 py-2 text-sm text-red-600 hover:bg-red-50"
                >
                  <Trash2 className="h-4 w-4" />
                  Удалить
                </button>
              </div>
            )}
          </div>

          <button
            onClick={onCreateTask}
            className="rounded p-1 text-gray-400 hover:bg-gray-200 hover:text-gray-600"
            title="Добавить задачу"
          >
            <Plus className="h-4 w-4" />
          </button>
        </div>
      </div>

      {/* Tasks Container с подсветкой */}
      <div 
        ref={setNodeRef}
        className={`flex-1 overflow-y-auto kanban-scrollbar rounded-lg p-2 transition-colors ${
          isColumnActive ? 'bg-blue-50 ring-2 ring-blue-300' : 'bg-gray-100'
        }`}
        style={{ minHeight: 'calc(100vh - 220px)' }}
      >
        <SortableContext
          items={column.tasks.map((t) => t.id)}
          strategy={verticalListSortingStrategy}
        >
          <div className="flex flex-col gap-2">
            {column.tasks.map((task) => (
              <TaskCardDraggable 
                key={task.id} 
                task={task} 
                onDelete={onDeleteTask}
                onEdit={onEditTask}
              />
            ))}
            
            {column.tasks.length === 0 && (
              <div 
                className="h-32 rounded border-2 border-dashed border-gray-300"
                style={{ pointerEvents: 'none' }}
              />
            )}
          </div>
        </SortableContext>
      </div>
    </div>
  );
}