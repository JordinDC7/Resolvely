import React from "react";
import "./takesurvey.css";
import PropTypes from "prop-types";

function Questions({ question }) {
  return (
    <div className="question-container">
      <div className="questionText">{question}</div>
    </div>
  );
}
Questions.propTypes = {
  state: PropTypes.shape({
    question: PropTypes.string.isRequired,
    answerOptions: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number.isRequired,
        text: PropTypes.string,
        value: PropTypes.string.isRequired,
        additionalInfo: PropTypes.string,
        showTextBox: PropTypes.bool.isRequired,
      })
    ).isRequired,
  }).isRequired,
  currentQuestionIndex: PropTypes.number.isRequired,
}.isRequired;
export default Questions;
