import React, { useState } from "react";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars } from "@fortawesome/free-solid-svg-icons";
import "./surveysicontool.css";

function SurveysIconTool() {
  const [isExpanded, setIsExpanded] = useState(false);

  return (
    <div
      className={`widget-container ${isExpanded ? "expanded" : "collapsed"}`}
      onMouseEnter={() => setIsExpanded(true)}
      onMouseLeave={() => setIsExpanded(false)}
    >
      <div className="widget-header">
        {isExpanded ? <h3>Surveys</h3> : <FontAwesomeIcon icon={faBars} />}
      </div>
      {isExpanded && (
        <nav className="widget-nav">
          <NavLink to="/builder" activeClassName="active-link" target="_blank">
            Builder
          </NavLink>
          <NavLink
            to="/responses"
            activeClassName="active-link"
            target="_blank"
          >
            Responses
          </NavLink>
          <NavLink
            to="/analytics"
            activeClassName="active-link"
            target="_blank"
          >
            Analytics
          </NavLink>
        </nav>
      )}
    </div>
  );
}

export default SurveysIconTool;
