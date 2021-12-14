import { useContext } from "react";
import { Link } from "react-router-dom";

import { NotificationsNone } from "@material-ui/icons";
import "./layout.css";
import SideNav from "./SideNav";
import { AuthContext } from "../auth/provider";
import { fetchApi } from "../../modules/api";

export default function Layout({ children, hideHeader = false }) {
  const { setUserInfo, setFullName } = useContext(AuthContext);

  const logout = async () => {
    await fetchApi({
      path: "Auth/Logout",
      method: "POST",
    });
    // npm install some client side cookie mgmt library
    // use that library to delete the client side cookie
    // update the state in the context to remove userInfo
    setUserInfo(undefined);
    setFullName(undefined);
  };

  return (
    <>
      {!hideHeader && (
        <div className="pageWrapper">
          {/* TOP BAR */}
          <div className="topbar">
            <div className="topbarWrapper">
              <div className="topLeft">
                <span className="topbarlogo">BugsPot</span>
              </div>
              <div className="topRight">
                <div className="topbar-Icon-Container">
                  <NotificationsNone style={{ color: "#f5f5f5" }} />
                  <span className="topIconBadge"></span> {/* Red Dot */}
                </div>
                <div>
                  <Link
                    to="/login"
                    style={{ textDecoration: "none" }}
                    onClick={logout}
                  >
                    <h4>Logout</h4>
                  </Link>
                </div>
              </div>
            </div>
          </div>
          {/* END TOPBAR */}

          <div className="container">
            <SideNav />

            <div className="pages">
              <>{children}</>
            </div>
          </div>
        </div>
      )}
    </>
  );
}
