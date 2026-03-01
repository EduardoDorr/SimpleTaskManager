import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { renderWithProviders } from "@/test/renderWithProviders";
import { ConfirmDialog } from "./ConfirmDialog";

describe("ConfirmDialog", () => {
  it("renders title/description and triggers actions", async () => {
    const user = userEvent.setup();
    const onConfirm = vi.fn();
    const onCancel = vi.fn();

    renderWithProviders(
      <ConfirmDialog
        description="Confirm action description"
        onCancel={onCancel}
        onConfirm={onConfirm}
        open
        title="Delete task?"
      />,
    );

    expect(screen.getByText("Delete task?")).toBeInTheDocument();
    expect(screen.getByText("Confirm action description")).toBeInTheDocument();

    await user.click(screen.getByRole("button", { name: "Confirm" }));
    await user.click(screen.getByRole("button", { name: "Cancel" }));

    expect(onConfirm).toHaveBeenCalledTimes(1);
    expect(onCancel).toHaveBeenCalledTimes(1);
  });

  it("disables action buttons when loading", () => {
    renderWithProviders(
      <ConfirmDialog
        loading
        onCancel={vi.fn()}
        onConfirm={vi.fn()}
        open
        title="Delete task?"
      />,
    );

    expect(screen.getByRole("button", { name: "Confirm" })).toBeDisabled();
    expect(screen.getByRole("button", { name: "Cancel" })).toBeDisabled();
  });
});