import { useMemo, useState } from "react";
import type { TaskPriority, TaskStatus, TasksFilterDraft, TasksFilters } from "../types/taskTypes";

const defaultPageSize = 10;

const initialDraft: TasksFilterDraft = {
  title: "",
  status: "",
  priority: "",
  isActive: "true",
  isDescending: true,
};

const defaultFilters: TasksFilters = {
  page: 1,
  pageSize: defaultPageSize,
  isDescending: initialDraft.isDescending,
  isActive: true,
};

function parseIsActive(value: TasksFilterDraft["isActive"]): boolean | undefined {
  if (value === "true") {
    return true;
  }

  if (value === "false") {
    return false;
  }

  return undefined;
}

function buildFiltersFromDraft(draft: TasksFilterDraft, pageSize: number): TasksFilters {
  return {
    page: 1,
    pageSize,
    title: draft.title.trim() || undefined,
    status: (draft.status || undefined) as TaskStatus | undefined,
    priority: (draft.priority || undefined) as TaskPriority | undefined,
    isDescending: draft.isDescending,
    isActive: parseIsActive(draft.isActive),
  };
}

export function useTasksFilters() {
  const [draft, setDraft] = useState<TasksFilterDraft>(initialDraft);
  const [filters, setFilters] = useState<TasksFilters>(defaultFilters);

  const updateDraft = <K extends keyof TasksFilterDraft>(
    key: K,
    value: TasksFilterDraft[K],
  ) => {
    setDraft((currentValue) => ({
      ...currentValue,
      [key]: value,
    }));
  };

  const applyFilters = () => {
    setFilters((currentValue) => buildFiltersFromDraft(draft, currentValue.pageSize));
  };

  const clearFilters = () => {
    setDraft(initialDraft);
    setFilters(defaultFilters);
  };

  const setPage = (page: number) => {
    setFilters((currentValue) => ({
      ...currentValue,
      page,
    }));
  };

  const setPageSize = (pageSize: number) => {
    setFilters((currentValue) => ({
      ...currentValue,
      page: 1,
      pageSize,
    }));
  };

  const hasActiveFilters = useMemo(() => {
    return (
      Boolean(filters.title) ||
      Boolean(filters.status) ||
      Boolean(filters.priority) ||
      filters.isActive !== true ||
      filters.isDescending !== true
    );
  }, [filters]);

  return {
    draft,
    filters,
    hasActiveFilters,
    updateDraft,
    applyFilters,
    clearFilters,
    setPage,
    setPageSize,
  };
}