const dateFormatter = new Intl.DateTimeFormat("en-US", { dateStyle: "medium" });
const dateTimeFormatter = new Intl.DateTimeFormat("en-US", {
  dateStyle: "medium",
  timeStyle: "short",
});

function toDate(value: string | Date): Date {
  return value instanceof Date ? value : new Date(value);
}

function isValidDate(value: Date): boolean {
  return !Number.isNaN(value.getTime());
}

export function formatDate(value: string | Date): string {
  const dateValue = toDate(value);
  return isValidDate(dateValue) ? dateFormatter.format(dateValue) : "-";
}

export function formatDateTime(value: string | Date): string {
  const dateValue = toDate(value);
  return isValidDate(dateValue) ? dateTimeFormatter.format(dateValue) : "-";
}