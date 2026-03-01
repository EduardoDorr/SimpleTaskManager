import MenuRoundedIcon from "@mui/icons-material/MenuRounded";
import { useState } from "react";
import { AppBar, Box, Drawer, IconButton, Toolbar, Typography } from "@mui/material";
import { Outlet } from "react-router-dom";
import { Sidebar } from "./Sidebar";

const drawerWidth = 260;

export function MainLayout() {
  const [mobileOpen, setMobileOpen] = useState(false);

  const toggleMobileSidebar = () => {
    setMobileOpen((currentValue) => !currentValue);
  };

  const closeMobileSidebar = () => {
    setMobileOpen(false);
  };

  const sidebarContent = <Sidebar onNavigate={closeMobileSidebar} />;

  return (
    <Box
      sx={{
        bgcolor: "background.default",
        display: "flex",
        height: "100vh",
        overflowX: "hidden",
      }}
    >
      <AppBar
        color="inherit"
        elevation={0}
        position="fixed"
        sx={{
          borderBottom: "1px solid",
          borderColor: "divider",
          width: { md: `calc(100% - ${drawerWidth}px)` },
          ml: { md: `${drawerWidth}px` },
        }}
      >
        <Toolbar>
          <IconButton
            aria-label="Open navigation menu"
            color="inherit"
            edge="start"
            onClick={toggleMobileSidebar}
            sx={{ display: { md: "none" }, mr: 1 }}
          >
            <MenuRoundedIcon />
          </IconButton>
          <Typography fontWeight={600} variant="subtitle1">
            <Box
              component="img"
              src="/logo.svg"
              alt="Task Management Logo"
              width={24}
              height={24}
              sx={{ mr: 1, verticalAlign: "middle" }}
            />
            Task Management
          </Typography>
        </Toolbar>
      </AppBar>

      <Box component="nav" sx={{ width: { md: drawerWidth }, flexShrink: { md: 0 } }}>
        <Drawer
          ModalProps={{ keepMounted: true }}
          onClose={closeMobileSidebar}
          open={mobileOpen}
          sx={{
            display: { xs: "block", md: "none" },
            "& .MuiDrawer-paper": { boxSizing: "border-box", width: drawerWidth },
          }}
          variant="temporary"
        >
          {sidebarContent}
        </Drawer>
        <Drawer
          open
          sx={{
            display: { xs: "none", md: "block" },
            "& .MuiDrawer-paper": { boxSizing: "border-box", width: drawerWidth },
          }}
          variant="permanent"
        >
          {sidebarContent}
        </Drawer>
      </Box>

      <Box
        component="main"
        sx={{
          display: "flex",
          flexDirection: "column",
          flexGrow: 1,
          minHeight: 0,
          minWidth: 0,
          overflow: "hidden",
          p: { xs: 2, md: 3 },
          width: { md: `calc(100% - ${drawerWidth}px)` },
        }}
      >
        <Toolbar />
        <Box sx={{ flex: 1, minHeight: 0, minWidth: 0 }}>
          <Outlet />
        </Box>
      </Box>
    </Box>
  );
}