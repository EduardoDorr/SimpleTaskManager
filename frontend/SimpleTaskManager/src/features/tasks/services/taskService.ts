import { request } from "@/shared/http/request";
import type { CreateTaskInput, PaginatedResponse, TaskItem, TasksFilters } from "../types/taskTypes";

const tasksEndpoint = "/api/v1/tasks";

export async function getTasks(filters: TasksFilters): Promise<PaginatedResponse<TaskItem>> {
  return request<PaginatedResponse<TaskItem>>({
    method: "GET",
    url: `${tasksEndpoint}/`,
    params: {
      page: filters.page,
      pageSize: filters.pageSize,
      title: filters.title || undefined,
      status: filters.status || undefined,
      priority: filters.priority || undefined,
      isDescending: filters.isDescending,
      isActive: filters.isActive,
    },
  });
}

export async function createTask(input: CreateTaskInput): Promise<number> {
  return request<number>({
    method: "POST",
    url: `${tasksEndpoint}/`,
    data: {
      title: input.title,
      description: input.description || undefined,
      priority: input.priority,
      dueDate: input.dueDate,
    },
  });
}

export async function toggleTaskStatus(id: number): Promise<void> {
  await request<void>({
    method: "PATCH",
    url: `${tasksEndpoint}/${id}/status`,
  });
}

export async function deleteTask(id: number): Promise<void> {
  await request<void>({
    method: "DELETE",
    url: `${tasksEndpoint}/${id}`,
  });
}