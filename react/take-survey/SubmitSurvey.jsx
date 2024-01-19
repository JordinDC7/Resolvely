import React from "react";
import PropTypes from "prop-types";
import "./takesurvey.css";

function SubmitSurvey({ onPrevious, onNext, isLastQuestion, onSubmit }) {
  return (
    <div className="question-navigation">
      <button onClick={onPrevious}>Previous</button>
      {isLastQuestion ? (
        <button onClick={onSubmit}>Submit</button>
      ) : (
        <button onClick={onNext}>Next</button>
      )}
    </div>
  );
}

SubmitSurvey.propTypes = {
  onPrevious: PropTypes.func.isRequired,
  onNext: PropTypes.func.isRequired,
  isLastQuestion: PropTypes.bool.isRequired,
  onSubmit: PropTypes.func.isRequired,
};

export default SubmitSurvey;
