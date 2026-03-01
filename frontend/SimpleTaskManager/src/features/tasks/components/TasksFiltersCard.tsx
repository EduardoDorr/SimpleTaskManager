import { Box, Button, Card, CardContent, MenuItem, Stack, TextField, Typography } from "@mui/material";
import { surfaceCardSx } from "@/shared/styles/surfaceCards";
import type { TasksFilterDraft } from "../types/taskTypes";

type OnFieldChange = <K extends keyof TasksFilterDraft>(
  field: K,
  value: TasksFilterDraft[K],
) => void;

type TasksFiltersCardProps = {
  draft: TasksFilterDraft;
  loading?: boolean;
  hasActiveFilters: boolean;
  onFieldChange: OnFieldChange;
  onSearch: () => void;
  onClear: () => void;
};

export function TasksFiltersCard({
  draft,
  loading = false,
  hasActiveFilters,
  onFieldChange,
  onSearch,
  onClear,
}: TasksFiltersCardProps) {
  return (
    <Card elevation={0} sx={surfaceCardSx}>
      <CardContent>
        <Stack spacing={2.5}>
          <Typography fontWeight={600} variant="h6">
            Filters
          </Typography>

          <Box
            sx={{
              columnGap: 2,
              display: "grid",
              maxWidth: "100%",
              gridTemplateColumns: {
                xs: "1fr",
                md: "repeat(2, minmax(0, 1fr))",
                lg: "minmax(280px, 2fr) repeat(4, minmax(140px, 1fr))",
              },
              rowGap: 2,
            }}
          >
            <TextField
              fullWidth
              label="Title"
              onChange={(event) => onFieldChange("title", event.target.value)}
              size="small"
              value={draft.title}
            />

            <TextField
              fullWidth
              label="Status"
              onChange={(event) => onFieldChange("status", event.target.value as TasksFilterDraft["status"])}
              select
              size="small"
              value={draft.status}
            >
              <MenuItem value="">All</MenuItem>
              <MenuItem value="Backlog">Backlog</MenuItem>
              <MenuItem value="Completed">Completed</MenuItem>
            </TextField>

            <TextField
              fullWidth
              label="Priority"
              onChange={(event) => onFieldChange("priority", event.target.value as TasksFilterDraft["priority"])}
              select
              size="small"
              value={draft.priority}
            >
              <MenuItem value="">All</MenuItem>
              <MenuItem value="Low">Low</MenuItem>
              <MenuItem value="Medium">Medium</MenuItem>
              <MenuItem value="High">High</MenuItem>
              <MenuItem value="Critical">Critical</MenuItem>
            </TextField>

            <TextField
              fullWidth
              label="Visibility"
              onChange={(event) => onFieldChange("isActive", event.target.value as TasksFilterDraft["isActive"])}
              select
              size="small"
              value={draft.isActive}
            >
              <MenuItem value="true">Active only</MenuItem>
              <MenuItem value="">All</MenuItem>
              <MenuItem value="false">Inactive only</MenuItem>
            </TextField>

            <TextField
              fullWidth
              label="Sort"
              onChange={(event) => onFieldChange("isDescending", event.target.value === "true")}
              select
              size="small"
              value={String(draft.isDescending)}
            >
              <MenuItem value="true">Newest first</MenuItem>
              <MenuItem value="false">Oldest first</MenuItem>
            </TextField>
          </Box>

          <Stack
            direction={{ xs: "column", sm: "row" }}
            justifyContent="flex-end"
            spacing={1.5}
          >
            <Button disabled={loading || !hasActiveFilters} onClick={onClear} variant="outlined">
              Clear
            </Button>
            <Button disabled={loading} onClick={onSearch} variant="contained">
              Search
            </Button>
          </Stack>
        </Stack>
      </CardContent>
    </Card>
  );
}