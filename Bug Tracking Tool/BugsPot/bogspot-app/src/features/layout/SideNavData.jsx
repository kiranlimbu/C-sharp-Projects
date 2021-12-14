import React from "react";

import {
  BugReport,
  Assessment,
  AccountTree,
  PersonAdd,
} from "@material-ui/icons";

export const SideNavData = [
  {
    title: "Add Bug",
    icon: <BugReport />,
    link: "/addBug",
  },
  {
    title: "Dashboard",
    icon: <Assessment />,
    link: "/",
  },
  {
    title: "Bugs",
    icon: (
      <img
        src="https://cdn-icons-png.flaticon.com/512/6306/6306397.png"
        height="20px"
        width="20px"
        style={{
          opacity: 0.7,
          padding: "0px",
          marginRight: "2px",
          marginLeft: "2px",
        }}
        className="sidebarIcon"
      />
    ),
    link: "/bugs",
  },
  {
    title: "Projects",
    icon: <AccountTree />,
    link: "/projects",
  },
  {
    title: "Users",
    icon: <PersonAdd />,
    link: "/users",
  },
];
