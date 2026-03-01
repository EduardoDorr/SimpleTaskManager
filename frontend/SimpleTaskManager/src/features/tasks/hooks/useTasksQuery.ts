import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/shared/constants/queryKeys";
import { getTasks } from "../services/taskService";
import type { TasksFilters } from "../types/taskTypes";

export function useTasksQuery(filters: TasksFilters) {
  return useQuery({
    queryKey: queryKeys.tasks(filters),
    queryFn: () => getTasks(filters),
    placeholderData: keepPreviousData,
  });
}