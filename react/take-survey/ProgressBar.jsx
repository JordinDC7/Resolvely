import React from "react";
import "./takesurvey.css";
import PropTypes from "prop-types";

function ProgressBar({ progress }) {
  const progressBarStyle = {
    "--progress-width": `${progress}%`,
  };
  return (
    <div className="progress-bar">
      <div className="progress" style={progressBarStyle}></div>
    </div>
  );
}
ProgressBar.propTypes = {
  progress: PropTypes.number.isRequired,
};
export default ProgressBar;
