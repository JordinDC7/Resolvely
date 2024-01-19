import React, { useEffect } from "react";
import PropTypes from "prop-types";
import "./takesurvey.css";

function OptionItem({
  option,
  isSelected,
  questionId,
  onOptionSelect,
  onAnswerOptionTextInputChange,
  quizState,
}) {
  useEffect(() => {
    if (
      option.value === null ||
      option.value === "Null" ||
      option.value === undefined
    ) {
      onOptionSelect(questionId, option.value, option);
    }
  }, [option.value]);

  const placeholder =
    option.value === "Null" ||
    option.value === undefined ||
    option.value === null
      ? "Please type your answer.."
      : option.value;

  return (
    <div key={option.id} className="option-container">
      {(option.showTextBox && isSelected) ||
      option.value === "Null" ||
      option.value === undefined ||
      (option.value === null && option.text) ? (
        <input
          autoFocus
          type="text"
          placeholder={placeholder}
          className={`button-as-input`}
          value={quizState.textInputAnswers[option.id] || ""}
          onChange={(event) => onAnswerOptionTextInputChange(option.id, event)}
          onClick={() => onOptionSelect(questionId, option.value, option)}
        />
      ) : (
        <button
          className={isSelected ? "option selected" : "option"}
          onClick={() => onOptionSelect(questionId, option.value, option)}
        >
          {option.value}
        </button>
      )}
    </div>
  );
}
OptionItem.propTypes = {
  option: PropTypes.shape({
    id: PropTypes.number.isRequired,
    text: PropTypes.string,
    value: PropTypes.string.isRequired,
    showTextBox: PropTypes.bool.isRequired,
  }).isRequired,
  isSelected: PropTypes.bool.isRequired,
  questionId: PropTypes.number.isRequired,
  onOptionSelect: PropTypes.func.isRequired,
  onAnswerOptionTextInputChange: PropTypes.func.isRequired,
  quizState: PropTypes.shape({
    selectedAnswers: PropTypes.objectOf(PropTypes.string),
    isMultipleAllowed: PropTypes.objectOf(PropTypes.bool),
    textInputAnswers: PropTypes.objectOf(PropTypes.string).isRequired,
  }).isRequired,
};
export default OptionItem;
