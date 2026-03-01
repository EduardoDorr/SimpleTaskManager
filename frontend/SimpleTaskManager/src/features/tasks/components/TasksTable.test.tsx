import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { renderWithProviders } from "@/test/renderWithProviders";
import { TasksTable } from "./TasksTable";
import type { ReactNode } from "react";

vi.mock("@mui/x-data-grid", () => {
  return {
    DataGrid: ({
      rows,
      columns,
      onPaginationModelChange,
    }: {
      rows: Array<Record<string, unknown>>;
      columns: Array<{
        field: string;
        headerName: string;
        renderCell?: (params: { row: Record<string, unknown> }) => ReactNode;
      }>;
      onPaginationModelChange?: (value: { page: number; pageSize: number }) => void;
    }) => (
      <div>
        <button
          aria-label="Next page"
          onClick={() => onPaginationModelChange?.({ page: 1, pageSize: 10 })}
          type="button"
        >
          Next page
        </button>
        {rows.map((row) => (
          <div key={String(row.id)}>
            {columns.map((column) => (
              <div key={column.field}>
                {column.renderCell
                  ? column.renderCell({ row })
                  : String((row as Record<string, unknown>)[column.field] ?? "")}
              </div>
            ))}
          </div>
        ))}
      </div>
    ),
  };
});

describe("TasksTable", () => {
  it("calls row action handlers", async () => {
    const user = userEvent.setup();
    const onToggleStatus = vi.fn();
    const onDelete = vi.fn();
    const onPageChange = vi.fn();

    renderWithProviders(
      <TasksTable
        loading={false}
        onDelete={onDelete}
        onPageChange={onPageChange}
        onPageSizeChange={vi.fn()}
        onToggleStatus={onToggleStatus}
        page={1}
        pageSize={10}
        rows={[
          {
            id: 1,
            title: "Task A",
            description: "Description",
            status: "Backlog",
            priority: "High",
            dueDate: "2099-12-31",
            createdAt: "2026-02-27T18:45:00.000Z",
            updatedAt: undefined,
            isActive: true,
          },
        ]}
        totalCount={1}
      />,
    );

    await user.click(screen.getByRole("checkbox", { name: "Toggle status for Task A" }));
    await user.click(screen.getByRole("button", { name: "Delete Task A" }));
    await user.click(screen.getByRole("button", { name: "Next page" }));

    expect(onToggleStatus).toHaveBeenCalledWith(1);
    expect(onDelete).toHaveBeenCalledWith(1);
    expect(onPageChange).toHaveBeenCalledWith(2);
  });

  it("disables row actions while loading", () => {
    renderWithProviders(
      <TasksTable
        loading
        onDelete={vi.fn()}
        onPageChange={vi.fn()}
        onPageSizeChange={vi.fn()}
        onToggleStatus={vi.fn()}
        page={1}
        pageSize={10}
        rows={[
          {
            id: 1,
            title: "Task A",
            description: "Description",
            status: "Backlog",
            priority: "High",
            dueDate: "2099-12-31",
            createdAt: "2026-02-27T18:45:00.000Z",
            updatedAt: undefined,
            isActive: true,
          },
        ]}
        totalCount={1}
      />,
    );

    expect(screen.getByRole("checkbox", { name: "Toggle status for Task A" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Delete Task A" })).toBeDisabled();
  });
});
