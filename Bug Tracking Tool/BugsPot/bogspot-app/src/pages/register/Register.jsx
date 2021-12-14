import React, { useState, useContext } from "react";
import { useHistory } from "react-router-dom";

import "./login.css";
import { Grid, Paper, TextField } from "@material-ui/core";
import { fetchApi } from "../../modules/api";
import { AuthContext } from "../../features/auth/provider";

function Register() {
  const [fName, setFName] = useState("");
  const [lName, setLName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState(undefined);
  const [redirect, setRedirect] = useState(false);
  const history = useHistory();
  const { getTempVariable } = useContext(AuthContext);

  const submit = async (e) => {
    e.preventDefault();

    const json = await fetchApi({
      path: "Auth/Register",
      method: "POST",
      body: {
        fName,
        lName,
        email,
        password,
        confirmPassword,
      },
    });

    if (json.error || json.status === 401) {
      setError(json.error);
      return "";
    } else {
      setRedirect(true);
      getTempVariable(email);
    }
  };

  if (redirect) history.push("/company");

  return (
    <div style={{ margin: "20px auto", width: "100%" }}>
      <form onSubmit={submit}>
        <Grid>
          <Paper elevation={10} className="loginBox">
            <div
              style={{
                color: "tomato",
                fontSize: "30px",
                alignSelf: "flex-start",
              }}
            >
              <h5>Register here</h5>
            </div>
            <div className="textField">
              <TextField
                label="First Name"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setFName(e.target.value)}
              />
              <TextField
                label="Last Name"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setLName(e.target.value)}
              />
              <TextField
                label="Email Address"
                placeholder="Username"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setEmail(e.target.value)}
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
              <TextField
                label="Confirm Password"
                placeholder="Password"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setConfirmPassword(e.target.value)}
              />
            </div>
            <div className="button-signin" style={{ marginTop: 20 }}>
              <button type="submit" className="button">
                Submit
              </button>
            </div>
          </Paper>
        </Grid>
      </form>
    </div>
  );
}

export default Register;
