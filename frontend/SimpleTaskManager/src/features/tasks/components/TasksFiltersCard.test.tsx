import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { renderWithProviders } from "@/test/renderWithProviders";
import { TasksFiltersCard } from "./TasksFiltersCard";

describe("TasksFiltersCard", () => {
  it("calls callbacks for field change and actions", async () => {
    const user = userEvent.setup();
    const onFieldChange = vi.fn();
    const onSearch = vi.fn();
    const onClear = vi.fn();

    renderWithProviders(
      <TasksFiltersCard
        draft={{
          title: "",
          status: "",
          priority: "",
          isDescending: true,
          isActive: "true",
        }}
        hasActiveFilters
        onClear={onClear}
        onFieldChange={onFieldChange}
        onSearch={onSearch}
      />,
    );

    await user.type(screen.getByRole("textbox", { name: "Title" }), "Task");
    await user.click(screen.getByRole("button", { name: "Search" }));
    await user.click(screen.getByRole("button", { name: "Clear" }));

    expect(onFieldChange).toHaveBeenCalled();
    expect(onSearch).toHaveBeenCalledTimes(1);
    expect(onClear).toHaveBeenCalledTimes(1);
  });

  it("disables clear button when there are no active filters", () => {
    renderWithProviders(
      <TasksFiltersCard
        draft={{
          title: "",
          status: "",
          priority: "",
          isDescending: true,
          isActive: "true",
        }}
        hasActiveFilters={false}
        onClear={vi.fn()}
        onFieldChange={vi.fn()}
        onSearch={vi.fn()}
      />,
    );

    expect(screen.getByRole("button", { name: "Clear" })).toBeDisabled();
  });
});
