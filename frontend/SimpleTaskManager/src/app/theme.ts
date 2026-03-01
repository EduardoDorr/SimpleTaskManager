import { createTheme } from "@mui/material/styles";

export const appTheme = createTheme({
  palette: {
    primary: { main: "#4F46E5" },
    secondary: { main: "#22D3EE" },
    success: { main: "#16A34A" },
    error: { main: "#DC2626" },
    background: {
      default: "#F8FAFC",
      paper: "#FFFFFF",
    },
    text: {
      primary: "#0F172A",
      secondary: "#475569",
    },
  },
});