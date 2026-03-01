import axios from "axios";
import { toast } from "react-hot-toast";
import { axiosClient } from "./axiosClient";
import type { ApiResult } from "./apiTypes";

class HandledApiError extends Error {}

export async function request<T>(
  config: Parameters<typeof axiosClient.request>[0],
): Promise<T> {
  try {
    const response = await axiosClient.request<ApiResult<T>>(config);

    if (response.status === 204 || response.data === undefined || response.data === null) {
      return undefined as T;
    }

    const payload = response.data;

    if (!payload.success) {
      payload.errors.forEach((error) => {
        toast.error(error.detail || "Request failed.");
      });

      throw new HandledApiError(payload.errors[0]?.detail ?? "Request failed.");
    }

    return payload.value as T;
  } catch (error) {
    if (error instanceof HandledApiError) {
      throw error;
    }

    if (axios.isAxiosError(error)) {
      const payload = error.response?.data as ApiResult<unknown> | undefined;

      if (payload?.errors?.length) {
        payload.errors.forEach((apiError) => {
          toast.error(apiError.detail || "Request failed.");
        });
        throw new HandledApiError(payload.errors[0]?.detail ?? "Request failed.");
      }
    }

    toast.error("Unexpected error while calling API.");
    throw error;
  }
}