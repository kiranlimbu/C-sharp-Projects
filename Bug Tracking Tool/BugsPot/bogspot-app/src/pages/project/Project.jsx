import React, { useContext, useState } from "react";
import { useHistory } from "react-router";

import "./project.css";
import { Grid, Paper, TextField } from "@material-ui/core";
import { fetchApi } from "../../modules/api";
import { AuthContext } from "../../features/auth/provider";

function Project() {
  const [projName, setProjName] = useState("");
  const [projDescription, setProjDescription] = useState("");
  const [projEndDate, setProjEndDate] = useState(Date);
  const [error, setError] = useState(undefined);
  const [redirect, setRedirect] = useState(false);
  const history = useHistory();
  const { tempVariable } = useContext(AuthContext);

  const submit = async (e) => {
    e.preventDefault();
    console.log(tempVariable);

    const json = await fetchApi({
      path: `Project/Create/${tempVariable}`,
      method: "POST",
      body: {
        projName,
        projDescription,
        projEndDate,
      },
    });

    if (json.error) setError(json.error);
    else {
      setRedirect(true);
    }
  };

  if (redirect) history.push("/login");

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
              <h5>Project Information</h5>
            </div>

            <div className="textField">
              <TextField
                label="Project Name"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                onChange={(e) => setProjName(e.target.value)}
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
                onChange={(e) => setProjDescription(e.target.value)}
              />
              <TextField
                label="Project finish date"
                placeholder="mm/dd/yyyy"
                variant="outlined"
                margin="dense"
                fullWidth
                onChange={(e) => setProjEndDate(e.target.value)}
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

export default Project;
