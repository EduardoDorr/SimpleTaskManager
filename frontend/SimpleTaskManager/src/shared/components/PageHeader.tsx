import { Stack, Typography } from "@mui/material";
import type { ReactNode } from "react";

type PageHeaderProps = {
  icon?: ReactNode;
  title: string;
  subtitle?: string;
  actions?: ReactNode;
};

export function PageHeader({ icon, title, subtitle, actions }: PageHeaderProps) {
  return (
    <Stack
      alignItems={{ xs: "flex-start", md: "center" }}
      direction={{ xs: "column", md: "row" }}
      justifyContent="space-between"
      spacing={2}
    >
      <Stack alignItems="flex-start" direction="column" spacing={0.5}>
        <Stack alignItems="center" direction="row" spacing={1.5}>
          {icon}
          <Typography variant="h4">{title}</Typography>
        </Stack>
        {subtitle ? (
          <Typography color="text.secondary" variant="body2">
            {subtitle}
          </Typography>
        ) : null}
      </Stack>
      {actions}
    </Stack>
  );
}