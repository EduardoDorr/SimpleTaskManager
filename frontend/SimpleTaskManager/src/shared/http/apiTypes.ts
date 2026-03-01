export type ApiError = {
  title: string;
  detail: string;
  status: number;
  type: string;
};

export type ApiInfo = {
  code: string;
  message: string;
};

export type ApiResult<T> = {
  success: boolean;
  status: number;
  errors: ApiError[];
  infos: ApiInfo[];
  value: T | null;
};