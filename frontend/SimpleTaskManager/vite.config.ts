import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";
import { fileURLToPath } from "url";

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  build: {
    outDir: "dist",
    rollupOptions: {
      output: {
        manualChunks: {
          "react-vendor": ["react", "react-dom", "react-router-dom"],
          "mui-core": ["@mui/material", "@emotion/react", "@emotion/styled"],
          "mui-x": ["@mui/x-data-grid"],
          "query-vendor": ["@tanstack/react-query"],
        },
      },
    }
  },
});