import React, { useState, useContext } from "react";
import { useHistory } from "react-router-dom";

import "./login.css";
import { Grid, Paper, TextField } from "@material-ui/core";
import { fetchApi } from "../../modules/api";
import { AuthContext } from "../../features/auth/provider";

function Company() {
  const [coName, setCoName] = useState("");
  const [coDescription, setCoDescription] = useState("");
  const [error, setError] = useState(undefined);
  const [redirect, setRedirect] = useState(false);
  const history = useHistory();
  const { tempVariable } = useContext(AuthContext);

  const submit = async (e) => {
    e.preventDefault();

    const json = await fetchApi({
      path: `Company/Create/${tempVariable}`,
      method: "POST",
      body: {
        coName,
        coDescription,
      },
    });

    if (json.error) setError(json.error);
    else {
      setRedirect(true);
    }
  };

  if (redirect) history.push("/project");

  return (
    <div style={{ margin: "20px auto", width: "100%" }}>
      <form onSubmit={submit}>
        <Grid>
          <Paper elevation={10} className="loginBox" style={{ width: "400px" }}>
            <div
              style={{
                color: "tomato",
                fontSize: "30px",
                alignSelf: "flex-start",
              }}
            >
              <h5>Company Information</h5>
            </div>

            <div className="textField">
              <TextField
                label="Company Name"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setCoName(e.target.value)}
              />
              <TextField
                id="outlined-textarea"
                label="Description"
                variant="outlined"
                margin="dense"
                multiline
                rows={4}
                fullWidth
                required
                onChange={(e) => setCoDescription(e.target.value)}
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

export default Company;
