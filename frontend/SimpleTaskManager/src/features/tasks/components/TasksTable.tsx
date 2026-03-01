import CheckCircleOutlineRoundedIcon from "@mui/icons-material/CheckCircleOutlineRounded";
import AutorenewRoundedIcon from "@mui/icons-material/AutorenewRounded";
import DeleteOutlineRoundedIcon from "@mui/icons-material/DeleteOutlineRounded";
import { Box, Chip, IconButton, Paper, Stack, Tooltip } from "@mui/material";
import { DataGrid, type GridColDef, type GridPaginationModel } from "@mui/x-data-grid";
import { formatDate, formatDateTime } from "@/shared/utils/dateFormat";
import { surfaceCardSx } from "@/shared/styles/surfaceCards";
import type { TaskItem, TaskPriority } from "../types/taskTypes";

type TasksTableProps = {
  rows: TaskItem[];
  loading: boolean;
  page: number;
  pageSize: number;
  totalCount: number;
  togglingTaskId?: number;
  deletingTaskId?: number;
  onPageChange: (page: number) => void;
  onPageSizeChange: (pageSize: number) => void;
  onToggleStatus: (taskId: number) => void;
  onDelete: (taskId: number) => void;
};

const priorityColorMap: Record<TaskPriority, "default" | "info" | "warning" | "error"> = {
  Low: "default",
  Medium: "info",
  High: "warning",
  Critical: "error",
};

export function TasksTable({
  rows,
  loading,
  page,
  pageSize,
  totalCount,
  togglingTaskId,
  deletingTaskId,
  onPageChange,
  onPageSizeChange,
  onToggleStatus,
  onDelete,
}: TasksTableProps) {
  const columns: GridColDef<TaskItem>[] = [
    {
      field: "title",
      headerName: "Title",
      minWidth: 220,
      flex: 1,
    },
    {
      field: "description",
      headerName: "Description",
      minWidth: 220,
      flex: 1.4,
      renderCell: (params) => (
        <>
          {params.row.description || "-"}
        </>
      ),
    },
    {
      field: "status",
      headerName: "Status",
      minWidth: 130,
      renderCell: (params) => (
        <Chip
          color={params.row.status === "Completed" ? "success" : "default"}
          label={params.row.status}
          size="small"
          variant={params.row.status === "Completed" ? "filled" : "outlined"}
        />
      ),
    },
    {
      field: "priority",
      headerName: "Priority",
      minWidth: 120,
      renderCell: (params) => (
        <Chip
          color={priorityColorMap[params.row.priority]}
          label={params.row.priority}
          size="small"
          variant="outlined"
        />
      ),
    },
    {
      field: "dueDate",
      headerName: "Due Date",
      minWidth: 140,
      valueFormatter: (value) => formatDate(value as string),
    },
    {
      field: "createdAt",
      headerName: "Created At",
      minWidth: 180,
      valueFormatter: (value) => formatDateTime(value as string),
    },
    {
      field: "actions",
      headerName: "Actions",
      minWidth: 130,
      sortable: false,
      align: "right",
      headerAlign: "right",
      renderCell: (params) => (
        <Stack direction="row" justifyContent="flex-end" spacing={0.5} sx={{ width: "100%" }}>
          <Tooltip title="Toggle status">
            <span>
              <IconButton
                aria-label={`Toggle status for ${params.row.title}`}
                color="primary"
                disabled={loading || togglingTaskId === params.row.id || deletingTaskId === params.row.id}
                onClick={() => onToggleStatus(params.row.id)}
                size="small"
              >
                {params.row.status === "Backlog" ? (
                  <CheckCircleOutlineRoundedIcon fontSize="small" />
                ) : (
                  <AutorenewRoundedIcon fontSize="small" />
                )}
              </IconButton>
            </span>
          </Tooltip>

          <Tooltip title="Delete task">
            <span>
              <IconButton
                aria-label={`Delete ${params.row.title}`}
                color="error"
                disabled={loading || deletingTaskId === params.row.id || togglingTaskId === params.row.id}
                onClick={() => onDelete(params.row.id)}
                size="small"
              >
                <DeleteOutlineRoundedIcon fontSize="small" />
              </IconButton>
            </span>
          </Tooltip>
        </Stack>
      ),
    },
  ];

  const paginationModel: GridPaginationModel = {
    page: Math.max(page - 1, 0),
    pageSize,
  };

  const handlePaginationModelChange = (nextPaginationModel: GridPaginationModel) => {
    const nextPage = nextPaginationModel.page + 1;

    if (nextPaginationModel.pageSize !== pageSize) {
      onPageSizeChange(nextPaginationModel.pageSize);
      return;
    }

    if (nextPage !== page) {
      onPageChange(nextPage);
    }
  };

  return (
    <Box sx={{ height: "100%", maxWidth: "100%", minHeight: 420, overflowX: "auto" }}>
      <Paper
        elevation={0}
        sx={{
          ...surfaceCardSx,
          border: "1px solid",
          borderColor: "divider",
          display: "flex",
          flexDirection: "column",
          height: "100%",
          minWidth: 0,
          overflow: "hidden",
        }}
      >
        <DataGrid
          columns={columns}
          disableColumnMenu
          disableRowSelectionOnClick
          loading={loading}
          onPaginationModelChange={handlePaginationModelChange}
          pageSizeOptions={[5, 10, 20, 50]}
          pagination
          paginationMode="server"
          paginationModel={paginationModel}
          rowCount={totalCount}
          rows={rows}
          sx={{ border: 0, height: "100%", minWidth: 980 }}
        />
      </Paper>
    </Box>
  );
}