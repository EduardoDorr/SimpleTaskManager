export const queryKeys = {
  tasks: (filters: unknown) => ["tasks", filters] as const,
  tasksRoot: ["tasks"] as const,
};