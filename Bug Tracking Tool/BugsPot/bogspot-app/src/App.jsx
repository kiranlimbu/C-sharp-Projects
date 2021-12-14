import React, { useEffect, useState, useContext } from "react";
import { Switch, Route, BrowserRouter, useHistory } from "react-router-dom";
import "./App.css";

import Login from "./pages/login/Login";
import Project from "./pages/project/Project";
import Company from "./pages/company/Company";
import Register from "./pages/register/Register";
import Dashboard from "./pages/dashboard";

import { AuthProvider, AuthContext } from "./features/auth/provider";
import { fetchApi } from "./modules/api";
import AddBug from "./pages/bugs";
import Layout from "./features/layout";

function ProtectedRoute({ children, path }) {
  const authCtx = useContext(AuthContext);
  const history = useHistory();

  if (!authCtx.userInfo) {
    history.push("/login");
    return "";
  }

  console.log("inside protectedRoute");
  return (
    <Route exact path={path}>
      {children}
    </Route>
  );
}

function App() {
  return (
    <div className="App">
      <AuthProvider>
        <BrowserRouter>
          <Switch>
            <Route path="/login" component={Login} />
            <Route path="/register" component={Register} />
            <Route path="/company" component={Company} />
            <Route path="/project" component={Project} />
            <Layout>
              <ProtectedRoute exact path="/addBug">
                <AddBug />
              </ProtectedRoute>
              <ProtectedRoute exact path="/">
                <Dashboard />
              </ProtectedRoute>
            </Layout>
          </Switch>
        </BrowserRouter>
      </AuthProvider>
    </div>
  );
}

export default App;

/* 
<Route exact path="/addBug" component={AddBug} />
              <Route exact path="/" component={Dashboard} />


const [userInfo, setUserInfo] = useState(undefined);
  const [fullName, setFullName] = useState(undefined);

  useEffect(() => {
    (async () => {
      const response = await fetch("http://localhost:5000/Auth/User", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Accept: "application/json",
        },
        credentials: "include",
      });

      setFullName(response.fullName);
      console.log("useEffect:", JSON.stringify(response));
    })();
  });

  console.log("App component:", fullName);
*/
