import { type ClassValue, clsx } from "clsx";
import { twMerge } from "tailwind-merge";

/**
 * Объединяет Tailwind-классы, корректно обрабатывая конфликты.
 * Использует clsx для условных классов и tailwind-merge для разрешения конфликтов.
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}