import { Button, Dialog, DialogActions, DialogContent, DialogTitle, MenuItem, Stack, TextField } from "@mui/material";
import { useTaskForm } from "../hooks/useTaskForm";
import type { CreateTaskInput, TaskPriority } from "../types/taskTypes";

type CreateTaskModalProps = {
  open: boolean;
  loading?: boolean;
  onClose: () => void;
  onCreate: (input: CreateTaskInput) => Promise<void>;
};

const priorities: TaskPriority[] = ["Low", "Medium", "High", "Critical"];

function getTodayDateString() {
  return new Date().toISOString().split("T")[0];
}

export function CreateTaskModal({ open, loading = false, onClose, onCreate }: CreateTaskModalProps) {
  const {
    values,
    errors,
    isDirty,
    isValid,
    setFieldValue,
    setFieldTouched,
    showError,
  } = useTaskForm({
    title: "",
    description: "",
    dueDate: getTodayDateString(),
    priority: "",
  });

  const submitTask = async () => {
    setFieldTouched("title");
    setFieldTouched("description");
    setFieldTouched("dueDate");
    setFieldTouched("priority");

    if (!isValid) {
      return;
    }

    await onCreate({
      title: values.title.trim(),
      description: values.description.trim() || undefined,
      dueDate: values.dueDate,
      priority: values.priority as TaskPriority,
    });
  };

  return (
    <Dialog fullWidth maxWidth="sm" onClose={loading ? undefined : onClose} open={open}>
      <DialogTitle>Create New Task</DialogTitle>
      <DialogContent>
        <Stack sx={{ mt: 1 }} spacing={2}>
          <TextField
            error={showError("title")}
            fullWidth
            helperText={showError("title") ? errors.title : ""}
            label="Title"
            onBlur={() => setFieldTouched("title")}
            onChange={(event) => setFieldValue("title", event.target.value)}
            value={values.title}
          />

          <TextField
            error={showError("description")}
            fullWidth
            helperText={showError("description") ? errors.description : ""}
            label="Description"
            multiline
            onBlur={() => setFieldTouched("description")}
            onChange={(event) => setFieldValue("description", event.target.value)}
            rows={3}
            value={values.description}
          />

          <TextField
            error={showError("priority")}
            fullWidth
            helperText={showError("priority") ? errors.priority : ""}
            label="Priority"
            onBlur={() => setFieldTouched("priority")}
            onChange={(event) => setFieldValue("priority", event.target.value as TaskPriority)}
            select
            value={values.priority}
          >
            {priorities.map((priorityOption) => (
              <MenuItem key={priorityOption} value={priorityOption}>
                {priorityOption}
              </MenuItem>
            ))}
          </TextField>

          <TextField
            error={showError("dueDate")}
            fullWidth
            helperText={showError("dueDate") ? errors.dueDate : ""}
            label="Due date"
            onBlur={() => setFieldTouched("dueDate")}
            onChange={(event) => setFieldValue("dueDate", event.target.value)}
            slotProps={{ inputLabel: { shrink: true } }}
            type="date"
            value={values.dueDate}
          />
        </Stack>
      </DialogContent>
      <DialogActions sx={{ mb: 2, mr: 2 }}>
        <Button 
          disabled={loading}
          onClick={onClose}
          variant="outlined"
        >
          Cancel
        </Button>
        <Button
          disabled={loading || !isDirty || !isValid}
          onClick={submitTask}
          variant="contained"
        >
          Create Task
        </Button>
      </DialogActions>
    </Dialog>
  );
}