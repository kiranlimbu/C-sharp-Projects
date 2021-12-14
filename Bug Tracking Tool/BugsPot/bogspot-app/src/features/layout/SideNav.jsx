import React, { useContext, useState } from "react";
import { Link } from "react-router-dom";

import "./layout.css";
import { SideNavData } from "./SideNavData";

function SideNav() {
  return (
    <div className="sidebar">
      <div className="sidebarWrapper">
        <div className="sidebarMenu">
          <ul className="sidebarList">
            {SideNavData.map((val, key) => {
              return (
                <li
                  key={key}
                  id={window.location.pathname == val.link ? "active" : ""}
                  className="sidebarListItem"
                  // onClick={() => {
                  //   window.location.pathname = val.link;
                  // }}
                >
                  <Link to={val.link} className="sidebarListItem">
                    <div>{val.icon}</div>
                    <div>{val.title}</div>
                  </Link>
                </li>
              );
            })}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default SideNav;
