import { screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { describe, expect, it, vi } from "vitest";
import { renderWithProviders } from "@/test/renderWithProviders";
import { EmptyState } from "./EmptyState";

describe("EmptyState", () => {
  it("renders content and triggers action callback", async () => {
    const user = userEvent.setup();
    const onAction = vi.fn();

    renderWithProviders(
      <EmptyState
        actionLabel="Try again"
        description="No records were found."
        onAction={onAction}
        title="No data"
      />,
    );

    expect(screen.getByText("No data")).toBeInTheDocument();
    expect(screen.getByText("No records were found.")).toBeInTheDocument();

    await user.click(screen.getByRole("button", { name: "Try again" }));

    expect(onAction).toHaveBeenCalledTimes(1);
  });

  it("does not render action button when action data is missing", () => {
    renderWithProviders(<EmptyState title="No data" />);

    expect(screen.queryByRole("button")).not.toBeInTheDocument();
  });
});
