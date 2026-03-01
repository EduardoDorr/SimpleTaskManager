import { Box, Button, Stack, Typography } from "@mui/material";
import type { ReactNode } from "react";

type EmptyStateProps = {
  title: string;
  description?: string;
  actionLabel?: string;
  onAction?: () => void;
  icon?: ReactNode;
};

export function EmptyState({
  title,
  description,
  actionLabel,
  onAction,
  icon,
}: EmptyStateProps) {
  return (
    <Box
      sx={{
        border: "1px dashed",
        borderColor: "divider",
        borderRadius: 2,
        p: 4,
      }}
    >
      <Stack alignItems="center" spacing={1.5}>
        {icon}
        <Typography variant="h6">{title}</Typography>
        {description ? (
          <Typography color="text.secondary" sx={{ maxWidth: 480, textAlign: "center" }} variant="body2">
            {description}
          </Typography>
        ) : null}
        {actionLabel && onAction ? (
          <Button onClick={onAction} variant="outlined">
            {actionLabel}
          </Button>
        ) : null}
      </Stack>
    </Box>
  );
}