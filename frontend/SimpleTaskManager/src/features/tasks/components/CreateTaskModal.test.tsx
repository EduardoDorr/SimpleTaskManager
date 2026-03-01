import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { renderWithProviders } from "@/test/renderWithProviders";
import { CreateTaskModal } from "./CreateTaskModal";

describe("CreateTaskModal", () => {
  it("keeps create button disabled while form is invalid", async () => {
    const user = userEvent.setup();

    renderWithProviders(
      <CreateTaskModal
        onClose={vi.fn()}
        onCreate={vi.fn().mockResolvedValue(undefined)}
        open
      />,
    );

    const createButton = screen.getByRole("button", { name: "Create Task" });
    const titleInput = screen.getByRole("textbox", { name: "Title" });

    expect(createButton).toBeDisabled();

    await user.type(titleInput, "Task title");

    expect(createButton).toBeDisabled();
  });

  it("submits a valid payload when form is filled", async () => {
    const user = userEvent.setup();
    const onCreate = vi.fn().mockResolvedValue(undefined);

    renderWithProviders(
      <CreateTaskModal
        onClose={vi.fn()}
        onCreate={onCreate}
        open
      />,
    );

    await user.type(screen.getByRole("textbox", { name: "Title" }), "  Build tests  ");
    await user.type(screen.getByRole("textbox", { name: "Description" }), "  Modal description  ");
    await user.click(screen.getByRole("combobox", { name: "Priority" }));
    await user.click(screen.getByRole("option", { name: "High" }));

    await user.click(screen.getByRole("button", { name: "Create Task" }));

    await waitFor(() => {
      expect(onCreate).toHaveBeenCalledTimes(1);
    });

    const payload = onCreate.mock.calls[0][0] as {
      title: string;
      description?: string;
      dueDate: string;
      priority: string;
    };

    expect(payload.title).toBe("Build tests");
    expect(payload.description).toBe("Modal description");
    expect(payload.priority).toBe("High");
    expect(payload.dueDate).toMatch(/^\d{4}-\d{2}-\d{2}$/);
  });
});
