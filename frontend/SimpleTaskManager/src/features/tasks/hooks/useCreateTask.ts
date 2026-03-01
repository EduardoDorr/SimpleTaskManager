import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { queryKeys } from "@/shared/constants/queryKeys";
import { createTask } from "../services/taskService";

export function useCreateTask() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createTask,
    onSuccess: async () => {
      toast.success("Task created successfully.");
      await queryClient.invalidateQueries({ queryKey: queryKeys.tasksRoot });
    },
  });
}