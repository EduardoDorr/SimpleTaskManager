import { useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import { queryKeys } from "@/shared/constants/queryKeys";
import { toggleTaskStatus } from "../services/taskService";

export function useToggleTaskStatus() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: toggleTaskStatus,
    onSuccess: async () => {
      toast.success("Task status updated.");
      await queryClient.invalidateQueries({ queryKey: queryKeys.tasksRoot });
    },
  });
}