import React from "react";

import "./dashboard.css";
import { ArrowDownward, ArrowUpward } from "@material-ui/icons";

function Dashboard() {
  return (
    <div className="page-container">
      <div className="featured">
        <div className="featuredItem">
          <div className="featuredItem-ExtraDiv">
            <span className="featuredIitle">Total Projects</span>
            <div className="featuredMoneyContainer">
              <span className="featuredMoney">$2,415</span>
              <span className="featuredMoneyRate">
                -1.4
                <ArrowDownward className="featuredIcon negative" />
              </span>
            </div>

            <span className="featuredSub">Compare to last month</span>
          </div>
        </div>

        <div className="featuredItem">
          <div className="featuredItem-ExtraDiv">
            <span className="featuredIitle">Total Bugs</span>
            <div className="featuredMoneyContainer">
              <span className="featuredMoney">$4,415</span>
              <span className="featuredMoneyRate">
                -11.4
                <ArrowDownward className="featuredIcon negative" />
              </span>
            </div>
            <span className="featuredSub">Compare to last month</span>
          </div>
        </div>

        <div className="featuredItem">
          <div className="featuredItem-ExtraDiv">
            <span className="featuredIitle">Total Members</span>
            <div className="featuredMoneyContainer">
              <span className="featuredMoney">$2,225</span>
              <span className="featuredMoneyRate">
                +2.4
                <ArrowUpward className="featuredIcon" />
              </span>
            </div>
            <span className="featuredSub">Compare to last month</span>
          </div>
        </div>
      </div>

      <div className="bughistory">
        <div className="bughistoryTitle">Bug History</div>
        <div className="bugistoryChart">Chart Goes here</div>
      </div>
    </div>
  );
}

export default Dashboard;
