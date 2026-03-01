import { NavLink, useLocation } from "react-router-dom";
import ChecklistRoundedIcon from "@mui/icons-material/ChecklistRounded";
import {
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Typography,
} from "@mui/material";

type AppSidebarProps = {
  onNavigate?: () => void;
};

const navigationItems = [
  {
    label: "Tasks",
    to: "/tasks",
    icon: <ChecklistRoundedIcon />,
  },
];

export function Sidebar({ onNavigate }: AppSidebarProps) {
  const location = useLocation();

  return (
    <Box sx={{ width: 240 }}>
      <Toolbar>
        <Typography fontWeight={700} variant="h6">
          SimpleTaskManager
        </Typography>
      </Toolbar>
      <List sx={{ px: 1 }}>
        {navigationItems.map((item) => (
          <ListItem disablePadding key={item.to}>
            <ListItemButton
              component={NavLink}
              onClick={onNavigate}
              selected={location.pathname.startsWith(item.to)}
              sx={{ borderRadius: 1.5 }}
              to={item.to}
            >
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.label} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Box>
  );
}