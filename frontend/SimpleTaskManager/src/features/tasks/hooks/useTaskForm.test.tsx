import { act, renderHook } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { useTaskForm } from "./useTaskForm";

describe("useTaskForm", () => {
  it("starts invalid when required fields are empty", () => {
    const { result } = renderHook(() =>
      useTaskForm({
        title: "",
        description: "",
        priority: "",
        dueDate: "",
      }),
    );

    expect(result.current.isValid).toBe(false);
    expect(result.current.errors.title).toBe("Title is required.");
    expect(result.current.errors.priority).toBe("Priority is required.");
    expect(result.current.errors.dueDate).toBe("Due date is required.");
  });

  it("becomes valid when user fills required fields with valid values", () => {
    const { result } = renderHook(() =>
      useTaskForm({
        title: "",
        description: "",
        priority: "",
        dueDate: "",
      }),
    );

    act(() => {
      result.current.setFieldValue("title", "Build assessment");
      result.current.setFieldValue("description", "Task description");
      result.current.setFieldValue("priority", "High");
      result.current.setFieldValue("dueDate", "2099-12-31");
    });

    expect(result.current.isDirty).toBe(true);
    expect(result.current.isValid).toBe(true);
    expect(result.current.errors.title).toBe("");
    expect(result.current.errors.priority).toBe("");
    expect(result.current.errors.dueDate).toBe("");
  });

  it("only shows field error after field is marked as touched", () => {
    const { result } = renderHook(() =>
      useTaskForm({
        title: "",
        description: "",
        priority: "",
        dueDate: "2099-12-31",
      }),
    );

    expect(result.current.showError("title")).toBe(false);

    act(() => {
      result.current.setFieldTouched("title", true);
    });

    expect(result.current.showError("title")).toBe(true);
  });

  it("resets state back to initial values", () => {
    const initialValues = {
      title: "Initial title",
      description: "Initial description",
      priority: "Medium" as const,
      dueDate: "2099-12-31",
    };

    const { result } = renderHook(() => useTaskForm(initialValues));

    act(() => {
      result.current.setFieldValue("title", "Changed title");
      result.current.setFieldTouched("title", true);
    });

    expect(result.current.isDirty).toBe(true);

    act(() => {
      result.current.resetForm();
    });

    expect(result.current.values).toEqual(initialValues);
    expect(result.current.isDirty).toBe(false);
    expect(result.current.touched.title).toBe(false);
  });
});
