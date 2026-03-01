import { describe, expect, it } from "vitest";
import { formatDate, formatDateTime } from "./dateFormat";

describe("dateFormat", () => {
  it("returns '-' when date is invalid", () => {
    expect(formatDate("invalid-date")).toBe("-");
    expect(formatDateTime("invalid-date")).toBe("-");
  });

  it("formats date values using en-US locale", () => {
    const value = new Date("2026-02-27T18:45:00.000Z");
    const expected = new Intl.DateTimeFormat("en-US", { dateStyle: "medium" }).format(value);

    expect(formatDate(value)).toBe(expected);
  });

  it("formats datetime values using en-US locale", () => {
    const value = new Date("2026-02-27T18:45:00.000Z");
    const expected = new Intl.DateTimeFormat("en-US", {
      dateStyle: "medium",
      timeStyle: "short",
    }).format(value);

    expect(formatDateTime(value)).toBe(expected);
  });
});
