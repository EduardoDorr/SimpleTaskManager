import axios from "axios";

const fallbackBaseUrl = "http://localhost:8080";

export const axiosClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? fallbackBaseUrl,
  timeout: 10_000,
  headers: {
    "Content-Type": "application/json",
  },
});

function generateCorrelationId() {
  return crypto.randomUUID().replace(/-/g, "");
}

axiosClient.interceptors.request.use((config) => {
  config.headers = config.headers ?? {};
  config.headers["X-Correlation-Id"] = generateCorrelationId();

  return config;
});