export type TaskStatus = "Backlog" | "Completed";
export type TaskPriority = "Low" | "Medium" | "High" | "Critical";

export type TaskItem = {
  id: number;
  title: string;
  description?: string;
  status: TaskStatus;
  priority: TaskPriority;
  dueDate: string;
  createdAt: string;
  updatedAt?: string;
  isActive: boolean;
};

export type TasksFilters = {
  page: number;
  pageSize: number;
  title?: string;
  status?: TaskStatus;
  priority?: TaskPriority;
  isDescending?: boolean;
  isActive?: boolean;
};

export type TasksFilterDraft = {
  title: string;
  status: "" | TaskStatus;
  priority: "" | TaskPriority;
  isDescending: boolean;
  isActive: "" | "true" | "false";
};

export type CreateTaskInput = {
  title: string;
  description?: string;
  priority: TaskPriority;
  dueDate: string;
};

export type PaginatedResponse<T> = {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  data: T[];
};