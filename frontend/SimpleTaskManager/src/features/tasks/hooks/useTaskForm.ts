import { useMemo, useState } from "react";
import type { TaskPriority } from "../types/taskTypes";

const titleMaxLength = 50;
const descriptionMaxLength = 250;

export type TaskFormValues = {
  title: string;
  description: string;
  priority: "" | TaskPriority;
  dueDate: string;
};

type TaskFormTouched = {
  title: boolean;
  description: boolean;
  priority: boolean;
  dueDate: boolean;
};

type TaskFormErrors = {
  title: string;
  description: string;
  priority: string;
  dueDate: string;
};

const initialTouched: TaskFormTouched = {
  title: false,
  description: false,
  priority: false,
  dueDate: false,
};

function isPastDate(dateValue: string): boolean {
  if (!dateValue) {
    return false;
  }

  const todayUtc = new Date().toISOString().split("T")[0];
  return dateValue < todayUtc;
}

function getErrors(values: TaskFormValues): TaskFormErrors {
  const titleValue = values.title.trim();
  const descriptionValue = values.description.trim();

  return {
    title: !titleValue
      ? "Title is required."
      : titleValue.length > titleMaxLength
        ? `Title must be at most ${titleMaxLength} characters.`
        : "",
    description:
      descriptionValue.length > descriptionMaxLength
        ? `Description must be at most ${descriptionMaxLength} characters.`
        : "",
    priority: !values.priority ? "Priority is required." : "",
    dueDate: !values.dueDate
      ? "Due date is required."
      : isPastDate(values.dueDate)
        ? "Due date cannot be in the past."
        : "",
  };
}

export function useTaskForm(initialValues: TaskFormValues) {
  const [values, setValues] = useState<TaskFormValues>(initialValues);
  const [touched, setTouched] = useState<TaskFormTouched>(initialTouched);

  const errors = useMemo(() => getErrors(values), [values]);

  const setFieldValue = <K extends keyof TaskFormValues>(field: K, value: TaskFormValues[K]) => {
    setValues((currentValues) => ({
      ...currentValues,
      [field]: value,
    }));
  };

  const setFieldTouched = <K extends keyof TaskFormTouched>(field: K, value = true) => {
    setTouched((currentTouched) => ({
      ...currentTouched,
      [field]: value,
    }));
  };

  const showError = <K extends keyof TaskFormTouched>(field: K) =>
    touched[field] && Boolean(errors[field]);

  const isDirty = useMemo(() => {
    return (Object.keys(values) as (keyof TaskFormValues)[]).some(
      (field) => values[field] !== initialValues[field],
    );
  }, [initialValues, values]);

  const isValid = useMemo(() => {
    return !Object.values(errors).some(Boolean);
  }, [errors]);

  const resetForm = () => {
    setValues(initialValues);
    setTouched(initialTouched);
  };

  return {
    values,
    touched,
    errors,
    isDirty,
    isValid,
    setFieldValue,
    setFieldTouched,
    showError,
    resetForm,
  };
}
