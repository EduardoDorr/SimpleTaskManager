import { Navigate, createBrowserRouter } from "react-router-dom";
import { MainLayout } from "@/shared/layout/MainLayout";
import { lazy } from "react";

const TasksPage = lazy(() =>
  import("@/features/tasks/TasksPage").then((m) => ({ default: m.TasksPage }))
);

export const appRouter = createBrowserRouter([
  {
    path: "/",
    element: <MainLayout />,
    children: [
      { index: true, element: <Navigate replace to="/tasks" /> },
      { path: "tasks", element: <TasksPage /> },
      { path: "*", element: <Navigate replace to="/tasks" /> },
    ],
  },
]);