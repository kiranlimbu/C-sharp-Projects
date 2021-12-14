import React, { useContext, useState } from "react";

import "./newbug.css";
import { Grid, Paper, TextField, MenuItem } from "@material-ui/core";
import { AuthContext } from "../../features/auth/provider";

const statuses = [
  {
    value: "Open",
    label: "Open",
  },
  {
    value: "Assigned",
    label: "Assigned",
  },
  {
    value: "Fixed",
    label: "Fixed",
  },
  {
    value: "Retest",
    label: "Retest",
  },
  {
    value: "Reopen",
    label: "Reopen",
  },
  {
    value: "Close",
    label: "Close",
  },
];

const priorities = [
  {
    value: "Immediate",
    label: "Immediate",
  },
  {
    value: "High",
    label: "High",
  },
  {
    value: "Medium",
    label: "Medium",
  },
  {
    value: "Low",
    label: "Low",
  },
];

const severities = [
  {
    value: "Critical",
    label: "Critical",
  },
  {
    value: "High",
    label: "High",
  },
  {
    value: "Medium",
    label: "Medium",
  },
  {
    value: "Low",
    label: "Low",
  },
];

const types = [
  {
    value: "Functional",
    label: "Functional",
  },
  {
    value: "Graphical",
    label: "Graphical",
  },
  {
    value: "Wording",
    label: "Wording",
  },
  {
    value: "Ergonomics",
    label: "Ergonomics",
  },
  {
    value: "Performance",
    label: "Performance",
  },
];

function BugNew() {
  const [status, setStatus] = useState("Open");
  const [priority, setPriority] = useState("Low");
  const [severity, setSeverity] = useState("Low");
  const [type, setType] = useState("Functional");
  const { userInfo } = useContext(AuthContext);

  return (
    <form>
      <Grid>
        <Paper>
          <div className="container-bugnew">
            <div
              style={{
                width: "100%",
                padding: "10px 0px",
                fontSize: "30px",
                alignSelf: "flex-start",
                borderBottom: "1px solid #ddd",
              }}
            >
              <h5>New Bug Information</h5>
            </div>
            <div style={{ marginTop: "30px" }}>
              <TextField
                label="Tile"
                variant="outlined"
                margin="dense"
                fullWidth
                required
                // onChange={(e) => setCoName(e.target.value)}
              />
            </div>
            <div className="bugnew-Detail">
              <div className="bugnew-Detail-column">
                <div>
                  <TextField
                    select
                    label="Type"
                    value={type}
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    onChange={(e) => setType(e.target.value)}
                  >
                    {types.map((option) => (
                      <MenuItem key={option.value} value={option.value}>
                        {option.label}
                      </MenuItem>
                    ))}
                  </TextField>
                </div>
                <div>
                  <TextField
                    label="Assign To"
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    // required
                    // onChange={(e) => setCoName(e.target.value)}
                  />
                </div>
                <div>
                  <TextField
                    disabled
                    label="Reporter"
                    value={userInfo.fName}
                    // value="David Young"
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    // onChange={(e) => setCoName(e.target.value)}
                  />
                </div>
              </div>
              <div className="bugnew-Detail-column">
                <div>
                  <TextField
                    select
                    label="Status"
                    value={status}
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    onChange={(e) => setStatus(e.target.value)}
                  >
                    {statuses.map((option) => (
                      <MenuItem key={option.value} value={option.value}>
                        {option.label}
                      </MenuItem>
                    ))}
                  </TextField>
                </div>
                <div>
                  <TextField
                    select
                    label="Priority"
                    value={priority}
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    onChange={(e) => setPriority(e.target.value)}
                  >
                    {priorities.map((option) => (
                      <MenuItem key={option.value} value={option.value}>
                        {option.label}
                      </MenuItem>
                    ))}
                  </TextField>
                </div>
                <div>
                  <TextField
                    select
                    label="Severity"
                    value={severity}
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    onChange={(e) => setSeverity(e.target.value)}
                  >
                    {severities.map((option) => (
                      <MenuItem key={option.value} value={option.value}>
                        {option.label}
                      </MenuItem>
                    ))}
                  </TextField>
                </div>
              </div>
            </div>
            <div>
              <TextField
                id="outlined-textarea"
                label="Description"
                variant="outlined"
                margin="dense"
                multiline
                rows={4}
                fullWidth
                required
                // onChange={(e) => setCoDescription(e.target.value)}
              />
            </div>
            <div>
              <TextField
                id="outlined-textarea"
                label="Steps To Reproduce"
                variant="outlined"
                margin="dense"
                multiline
                rows={4}
                fullWidth
                required
                // onChange={(e) => setCoDescription(e.target.value)}
              />
            </div>
            <div>
              <TextField
                id="outlined-textarea"
                label="Actual result"
                variant="outlined"
                margin="dense"
                multiline
                rows={4}
                fullWidth
                required
                // onChange={(e) => setCoDescription(e.target.value)}
              />
            </div>
            <div>
              <TextField
                id="outlined-textarea"
                label="Expected Result"
                variant="outlined"
                margin="dense"
                multiline
                rows={4}
                fullWidth
                required
                // onChange={(e) => setCoDescription(e.target.value)}
              />
            </div>
            <div
              style={{
                marginTop: 20,
                display: "flex",
                justifyContent: "center",
              }}
            >
              <button type="submit" className="bugnew-button">
                Submit
              </button>
            </div>
          </div>
        </Paper>
      </Grid>
    </form>
  );
}

export default BugNew;
