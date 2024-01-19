import React from "react";
import PropTypes from "prop-types";
import "./surveys.css";
import "./modalcomponent.css";

function SurveyNameEdit({
  data,
  handleKeyDown,
  doubleClickHandler,
  isEditMode,
  updateSurveyText,
}) {
  const handleTextChange = (e) => {
    const newTextValue = e.target.value;

    updateSurveyText(newTextValue);
  };

  return (
    <div className="name-container">
      {isEditMode ? (
        <input
          type="text"
          className="name-input"
          value={data.name}
          onChange={handleTextChange}
          onKeyDown={(e) => handleKeyDown(e)}
        />
      ) : (
        <div
          className="ms-2 truncated-text2"
          onDoubleClick={doubleClickHandler}
        >
          {data.name}
        </div>
      )}
    </div>
  );
}

SurveyNameEdit.propTypes = {
  data: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    status: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }),
    surveyType: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }),
    taskId: PropTypes.number.isRequired,
    createdBy: PropTypes.shape({
      id: PropTypes.number.isRequired,
      firstName: PropTypes.string.isRequired,
      lastName: PropTypes.string.isRequired,
      mi: PropTypes.string.isRequired,
      avatarUrl: PropTypes.string.isRequired,
    }).isRequired,
    dateCreated: PropTypes.string.isRequired,
    dateModified: PropTypes.string.isRequired,
  }).isRequired,
  handleKeyDown: PropTypes.func.isRequired,
  doubleClickHandler: PropTypes.func.isRequired,
  isEditMode: PropTypes.bool.isRequired,
  updateSurveyText: PropTypes.func.isRequired,
}.isRequired;
export default SurveyNameEdit;
