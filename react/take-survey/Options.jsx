import React from "react";
import "./takesurvey.css";
import PropTypes from "prop-types";
import OptionItem from "./OptionItem";

function Options({
  answerOptions,
  onOptionSelect,
  questionId,
  quizState,
  onAnswerOptionTextInputChange,
}) {
  const isMultiple = quizState.isMultipleAllowed[questionId];

  const mapOption = (option) => {
    const isSelected = isMultiple
      ? quizState.selectedAnswers[questionId]?.some(
          (answer) => answer.value === option.value
        )
      : quizState.selectedAnswers[questionId]?.value === option.value;

    return (
      <OptionItem
        key={option.id}
        option={option}
        isSelected={isSelected}
        questionId={questionId}
        onOptionSelect={onOptionSelect}
        onAnswerOptionTextInputChange={onAnswerOptionTextInputChange}
        quizState={quizState}
      />
    );
  };

  return <div className="options">{answerOptions.map(mapOption)}</div>;
}

Options.propTypes = {
  answerOptions: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      text: PropTypes.string,
      value: PropTypes.string.isRequired,
      additionalInfo: PropTypes.string,
      showTextBox: PropTypes.bool.isRequired,
    })
  ).isRequired,
  onOptionSelect: PropTypes.func.isRequired,
  questionId: PropTypes.number.isRequired,
  quizState: PropTypes.shape({
    selectedAnswers: PropTypes.objectOf(
      PropTypes.oneOfType([PropTypes.string, PropTypes.array])
    ),
    isMultipleAllowed: PropTypes.objectOf(PropTypes.bool),
    textBoxInput: PropTypes.objectOf(PropTypes.string),
    textInputAnswers: PropTypes.objectOf(PropTypes.string),
  }),
  onAnswerOptionTextInputChange: PropTypes.func.isRequired,
};

export default Options;
