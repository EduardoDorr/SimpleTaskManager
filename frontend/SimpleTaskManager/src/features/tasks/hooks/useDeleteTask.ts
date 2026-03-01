import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { queryKeys } from "@/shared/constants/queryKeys";
import { deleteTask } from "../services/taskService";

export function useDeleteTask() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: deleteTask,
    onSuccess: async () => {
      toast.success("Task deleted successfully.");
      await queryClient.invalidateQueries({ queryKey: queryKeys.tasksRoot });
    },
  });
}