import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { Toaster } from "react-hot-toast";
import "@/index.css";
import App from "./App";
import { appTheme } from "./theme";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,
      refetchOnWindowFocus: false,
      staleTime: 30_000,
    },
  },
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={appTheme}>
        <CssBaseline />
        <App />
        <Toaster
          position="top-right"
          toastOptions={{
            duration: 3000,
          }}
        />
      </ThemeProvider>
    </QueryClientProvider>
  </StrictMode>,
);