import AddRoundedIcon from "@mui/icons-material/AddRounded";
import AssignmentTurnedInRoundedIcon from "@mui/icons-material/AssignmentTurnedInRounded";
import RefreshRoundedIcon from "@mui/icons-material/RefreshRounded";
import { useState } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { Box, Button, Stack } from "@mui/material";
import { queryKeys } from "@/shared/constants/queryKeys";
import { ConfirmDialog } from "@/shared/components/ConfirmDialog";
import { EmptyState } from "@/shared/components/EmptyState";
import { PageHeader } from "@/shared/components/PageHeader";
import { SplashScreen } from "@/shared/components/SplashScreen";
import { CreateTaskModal } from "./components/CreateTaskModal";
import { TasksFiltersCard } from "./components/TasksFiltersCard";
import { TasksTable } from "./components/TasksTable";
import { useCreateTask } from "./hooks/useCreateTask";
import { useDeleteTask } from "./hooks/useDeleteTask";
import { useTasksFilters } from "./hooks/useTasksFilters";
import { useTasksQuery } from "./hooks/useTasksQuery";
import { useToggleTaskStatus } from "./hooks/useToggleTaskStatus";
import type { CreateTaskInput } from "./types/taskTypes";

export function TasksPage() {
  const queryClient = useQueryClient();
  const [isCreateModalOpen, setCreateModalOpen] = useState(false);
  const [taskIdToDelete, setTaskIdToDelete] = useState<number | null>(null);

  const tasksFilters = useTasksFilters();
  const tasksQuery = useTasksQuery(tasksFilters.filters);

  const createTaskMutation = useCreateTask();
  const deleteTaskMutation = useDeleteTask();
  const toggleTaskStatusMutation = useToggleTaskStatus();

  const rows = tasksQuery.data?.data ?? [];
  const totalCount = tasksQuery.data?.totalCount ?? 0;

  const openCreateModal = () => {
    setCreateModalOpen(true);
  };

  const closeCreateModal = () => {
    if (!createTaskMutation.isPending) {
      setCreateModalOpen(false);
    }
  };

  const handleCreateTask = async (input: CreateTaskInput) => {
    await createTaskMutation.mutateAsync(input);
    setCreateModalOpen(false);
  };

  const refreshTasks = async () => {
    await queryClient.invalidateQueries({ queryKey: queryKeys.tasksRoot });
  };

  const handleToggleStatus = (taskId: number) => {
    toggleTaskStatusMutation.mutate(taskId);
  };

  const handleOpenDelete = (taskId: number) => {
    setTaskIdToDelete(taskId);
  };

  const handleCloseDelete = () => {
    if (!deleteTaskMutation.isPending) {
      setTaskIdToDelete(null);
    }
  };

  const handleConfirmDelete = async () => {
    if (taskIdToDelete === null) {
      return;
    }

    await deleteTaskMutation.mutateAsync(taskIdToDelete);
    setTaskIdToDelete(null);
  };

  const toggleTaskIdInProgress = toggleTaskStatusMutation.isPending
    ? toggleTaskStatusMutation.variables
    : undefined;

  const deleteTaskIdInProgress = deleteTaskMutation.isPending
    ? deleteTaskMutation.variables
    : undefined;

  if (tasksQuery.isLoading && !tasksQuery.data) {
    return <SplashScreen message="Loading tasks..." />;
  }

  return (
    <Stack spacing={3} sx={{ height: "100%", minHeight: 0, minWidth: 0 }}>
      <PageHeader
        actions={
          <Stack
            direction={{ xs: "column", sm: "row" }}
            spacing={1}
            sx={{ width: { xs: "100%", sm: "auto" } }}
          >
            <Button
              onClick={openCreateModal}
              startIcon={<AddRoundedIcon />}
              sx={{ width: { xs: "100%", sm: "auto" } }}
              variant="contained"
            >
              New Task
            </Button>
            <Button
              onClick={refreshTasks}
              startIcon={<RefreshRoundedIcon />}
              sx={{ width: { xs: "100%", sm: "auto" } }}
              variant="outlined"
            >
              Refresh
            </Button>
          </Stack>
        }
        icon={<AssignmentTurnedInRoundedIcon color="primary" fontSize="large"/>}
        subtitle="Track your work with consistent filtering and status updates."
        title="Tasks"
      />

      <TasksFiltersCard
        draft={tasksFilters.draft}
        hasActiveFilters={tasksFilters.hasActiveFilters}
        loading={tasksQuery.isFetching}
        onClear={tasksFilters.clearFilters}
        onFieldChange={tasksFilters.updateDraft}
        onSearch={tasksFilters.applyFilters}
      />

      {tasksQuery.isError && rows.length === 0 ? (
        <EmptyState
          actionLabel="Try again"
          description="We could not load tasks from the API."
          onAction={refreshTasks}
          title="Failed to load tasks"
        />
      ) : rows.length === 0 ? (
        <EmptyState
          actionLabel={tasksFilters.hasActiveFilters ? "Clear filters" : "Create a task"}
          description={
            tasksFilters.hasActiveFilters
              ? "No tasks match the selected filters."
              : "Create your first task to start managing work."
          }
          onAction={tasksFilters.hasActiveFilters ? tasksFilters.clearFilters : openCreateModal}
          title="No tasks found"
        />
      ) : (
        <Box sx={{ flex: 1, maxWidth: "100%", minHeight: 0, minWidth: 0 }}>
          <TasksTable
            deletingTaskId={deleteTaskIdInProgress}
            loading={tasksQuery.isFetching}
            onDelete={handleOpenDelete}
            onPageChange={tasksFilters.setPage}
            onPageSizeChange={tasksFilters.setPageSize}
            onToggleStatus={handleToggleStatus}
            page={tasksFilters.filters.page}
            pageSize={tasksFilters.filters.pageSize}
            rows={rows}
            togglingTaskId={toggleTaskIdInProgress}
            totalCount={totalCount}
          />
        </Box>
      )}

      {isCreateModalOpen ? (
        <CreateTaskModal
          loading={createTaskMutation.isPending}
          onClose={closeCreateModal}
          onCreate={handleCreateTask}
          open={isCreateModalOpen}
        />
      ) : null}

      <ConfirmDialog
        cancelLabel="Cancel"
        confirmColor="error"
        confirmLabel="Delete task"
        description="This action will soft-delete the task and remove it from the active list."
        loading={deleteTaskMutation.isPending}
        onCancel={handleCloseDelete}
        onConfirm={handleConfirmDelete}
        open={taskIdToDelete !== null}
        title="Delete task?"
      />
    </Stack>
  );
}