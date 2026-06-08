import { useState, useCallback, createContext } from 'react';
import type { ReactNode } from 'react';
import {
  DndContext,
  DragOverlay,
  pointerWithin,
  KeyboardSensor,
  PointerSensor,
  TouchSensor,
  useSensor,
  useSensors,
  type DragEndEvent,
  type DragStartEvent,
  type DragOverEvent,
} from '@dnd-kit/core';
import {
  arrayMove,
  sortableKeyboardCoordinates,
} from '@dnd-kit/sortable';
import type { BoardWithColumnsDto, ColumnWithTasksDto, TaskDto } from '../../types';

export const ActiveColumnContext = createContext<string | null>(null);

interface BoardDndContextProps {
  board: BoardWithColumnsDto;
  children: ReactNode;
  onTaskDragEnd: (taskId: string, newColumnId: string, newOrderIndex: number) => Promise<void>;
  onColumnDragEnd: (newColumnOrder: ColumnWithTasksDto[]) => Promise<void>;
}

export function BoardDndContext({
  board,
  children,
  onTaskDragEnd,
  onColumnDragEnd,
}: BoardDndContextProps) {
  const [activeTask, setActiveTask] = useState<TaskDto | null>(null);
  const [activeColumnId, setActiveColumnId] = useState<string | null>(null);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 8,
      },
    }),
    useSensor(TouchSensor, {
      activationConstraint: {
        delay: 250,
        tolerance: 5,
      },
    }),
    useSensor(KeyboardSensor, {
      coordinateGetter: sortableKeyboardCoordinates,
    })
  );

  const handleDragStart = (event: DragStartEvent) => {
    const id = event.active.id as string;
    
    const isColumn = board.columns.some((col) => col.id === id);
    
    if (!isColumn) {
      for (const column of board.columns) {
        const task = column.tasks.find((t) => t.id === id);
        if (task) {
          setActiveTask(task);
          break;
        }
      }
    }
  };

  const handleDragOver = (event: DragOverEvent) => {
    const { over } = event;
    if (!over) {
      setActiveColumnId(null);
      return;
    }

    const overId = over.id as string;
    const cleanOverId = overId.replace('-empty', '');

    const isOverColumn = board.columns.some((col) => col.id === cleanOverId);
    
    if (isOverColumn) {
      setActiveColumnId(cleanOverId);
    } else {
      // over — это задача, находим её колонку
      for (const col of board.columns) {
        const task = col.tasks.find((t) => t.id === cleanOverId);
        if (task) {
          setActiveColumnId(col.id);
          break;
        }
      }
    }
  };

  const handleDragEnd = useCallback(async (event: DragEndEvent) => {
    const { active, over } = event;
    setActiveTask(null);
    setActiveColumnId(null);

    if (!over) return;

    const activeId = active.id as string;
    const overId = over.id as string;

    if (activeId === overId) return;

    const isColumn = board.columns.some((col) => col.id === activeId);
    
    if (isColumn) {
      const oldIndex = board.columns.findIndex((col) => col.id === activeId);
      const newIndex = board.columns.findIndex((col) => col.id === overId);

      if (oldIndex !== -1 && newIndex !== -1 && oldIndex !== newIndex) {
        const newColumns = arrayMove(board.columns, oldIndex, newIndex);
        await onColumnDragEnd(newColumns);
      }
      return;
    }

    const sourceColumn = board.columns.find((col) =>
      col.tasks.some((t) => t.id === activeId)
    );
    
    if (!sourceColumn) return;

    const cleanOverId = overId.replace('-empty', '');

    let targetColumn: ColumnWithTasksDto | undefined;
    let targetTaskIndex = -1;

    const isOverColumn = board.columns.some((col) => col.id === cleanOverId);
    
    if (isOverColumn) {
      targetColumn = board.columns.find((col) => col.id === cleanOverId);
      targetTaskIndex = targetColumn?.tasks.length ?? 0;
    } else {
      for (const col of board.columns) {
        const idx = col.tasks.findIndex((t) => t.id === cleanOverId);
        if (idx !== -1) {
          targetColumn = col;
          
          const sourceIdx = sourceColumn.tasks.findIndex((t) => t.id === activeId);
          
          if (sourceColumn.id === col.id) {
            if (sourceIdx < idx) {
              targetTaskIndex = idx + 1;
            } else {
              targetTaskIndex = idx;
            }
          } else {
            targetTaskIndex = idx;
          }
          
          break;
        }
      }
    }

    if (!targetColumn) return;

    await onTaskDragEnd(activeId, targetColumn.id, targetTaskIndex);
  }, [board.columns, onTaskDragEnd, onColumnDragEnd]);

  return (
    <ActiveColumnContext.Provider value={activeColumnId}>
      <DndContext
        sensors={sensors}
        collisionDetection={pointerWithin}
        onDragStart={handleDragStart}
        onDragOver={handleDragOver}
        onDragEnd={handleDragEnd}
      >
        {children}
        <DragOverlay dropAnimation={null}>
          {activeTask ? (
            <div className="w-72 rotate-2 opacity-90 shadow-2xl">
              <div className="rounded-lg border border-gray-200 bg-white p-3">
                <h3 className="text-sm font-semibold text-gray-900">{activeTask.title}</h3>
                {activeTask.description && (
                  <p className="mt-1 text-xs text-gray-600 line-clamp-2">
                    {activeTask.description}
                  </p>
                )}
              </div>
            </div>
          ) : null}
        </DragOverlay>
      </DndContext>
    </ActiveColumnContext.Provider>
  );
}