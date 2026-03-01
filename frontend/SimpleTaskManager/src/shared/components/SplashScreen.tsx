import { Box, CircularProgress, Stack, Typography } from "@mui/material";

type SplashScreenProps = {
  message?: string;
};

export function SplashScreen({ message = "Loading..." }: SplashScreenProps) {
  return (
    <Box
      sx={{
        alignItems: "center",
        display: "flex",
        justifyContent: "center",
        minHeight: "50vh",
        width: "100%",
      }}
    >
      <Stack alignItems="center" spacing={2}>
        <CircularProgress />
        <Typography color="text.secondary" variant="body2">
          {message}
        </Typography>
      </Stack>
    </Box>
  );
}