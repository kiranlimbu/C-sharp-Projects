import React, { useState, useContext, useEffect } from "react";
import { Link, useHistory } from "react-router-dom";

import "./login.css";
import { Grid, Paper, Avatar, TextField, Typography } from "@material-ui/core";
import { AssignmentInd, Lock } from "@material-ui/icons";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";

import { fetchApi } from "../../modules/api";
import { AuthContext } from "../../features/auth/provider";

function Login() {
  const { getUser, fullName } = useContext(AuthContext);
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(undefined);
  const [redirect, setRedirect] = useState(false);
  const history = useHistory();

  const submit = async (e) => {
    e.preventDefault();

    const json = await fetchApi({
      path: "Auth/Login",
      method: "POST",
      body: { userName, password },
    });

    if (json.error || json.status === 401 || json.status === 400) {
      setError(json.error);
      return "";
    } else {
      await getUser();
      setRedirect(true);
    }
  };

  console.log("login: ", fullName);
  if (redirect || fullName) history.push("/");
  // inline css style
  const avatarStyle = { background: "black" };

  return (
    <div style={{ margin: "20px auto", width: "100%" }}>
      <form onSubmit={submit}>
        <Grid>
          <Paper elevation={10} className="loginBox">
            <div className="logo">
              <div>
                <Avatar style={avatarStyle}>
                  <AssignmentInd />
                </Avatar>
              </div>
              <div>
                <h5>Login here</h5>
              </div>
            </div>
            <div className="textField">
              <TextField
                label="Username"
                placeholder="Email Address"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setUserName(e.target.value)}
              />
              <TextField
                label="Password"
                placeholder="Password"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setPassword(e.target.value)}
              />
              <FormControlLabel
                control={
                  <Checkbox
                    // checked={checked}
                    // onChange={handleChange}
                    name="checkedB"
                    color="primary"
                  />
                }
                label="Remember me"
              />
            </div>
            <div className="button-signin">
              <button type="submit" className="button">
                Sign in
              </button>
            </div>
            <div className="link">
              {/* <BrowserRouter> */}
              <Typography className="icon-lock">
                <Link to="#" />
                <div>
                  <Lock fontSize="small" />
                </div>
                <div>Forgot password</div>
              </Typography>
              <div>
                Do you have an account?&nbsp;
                <Link
                  to="/register"
                  style={{ textDecoration: "none", color: "tomato" }}
                >
                  Register
                </Link>
              </div>
              {/* </BrowserRouter> */}
            </div>
          </Paper>
        </Grid>
      </form>
    </div>
  );
}

export default Login;
