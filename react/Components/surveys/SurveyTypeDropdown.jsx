import React, { useState } from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCheckCircle, faEdit } from "@fortawesome/free-solid-svg-icons";
import SubtleBadge from "components/common/SubtleBadge";
import "./surveys.css";
import "./modalcomponent.css";

function SurveyTypeDropdown({
  data,
  updateSurveyType,
  handleUpdateMethod,
  isSurveyTypeDropdown,
  setSurveyTypeDropdown,
}) {
  const [selectedSurveyType, setSelectedSurveyType] = useState(
    data.surveyType.id
  );

  const getSurveyTypeIcon = (surveyTypeName) => {
    switch (surveyTypeName) {
      case "Draft":
        return faEdit;
      case "Default":
        return faCheckCircle;
      default:
        return faCheckCircle;
    }
  };

  const getTypeInfo = (surveyTypeId) => {
    const id = parseInt(surveyTypeId, 10);
    switch (id) {
      case 1:
        return { name: "Default", class: "badge-subtle-success rounded-pill" };
      case 2:
        return { name: "Draft", class: "badge-subtle-primary rounded-pill" };
      default:
        return { name: "", class: "" };
    }
  };
  const handleSurveyTypeChange = (e) => {
    const newSurveyTypeId = parseInt(e.target.value, 10);
    setSelectedSurveyType(newSurveyTypeId);

    const newSurveyType = {
      ...data,
      id: newSurveyTypeId,
    };

    updateSurveyType(newSurveyType);
  };

  const typeInfo = getTypeInfo(data.surveyType.id);
  const surveyTypeIcon = getSurveyTypeIcon(data.surveyType.name);

  const surveyTypeClass = typeInfo.class;
  const surveyTypeName = typeInfo.name;

  return (
    <div className="status-container pointer-text-control">
      {isSurveyTypeDropdown ? (
        <>
          <select
            className="select-dropdown pointer-text-control"
            value={selectedSurveyType}
            onChange={handleSurveyTypeChange}
          >
            <option value="">Survey Type: {surveyTypeName}</option>
            <option value="1">Default</option>
            <option value="2">Draft</option>
          </select>
          <button
            className="save-button"
            onClick={(e) => handleUpdateMethod(e)}
          >
            Save
          </button>
        </>
      ) : (
        <div onClick={() => setSurveyTypeDropdown(true)}>
          <SubtleBadge pill className={surveyTypeClass}>
            {surveyTypeName}
            <FontAwesomeIcon icon={surveyTypeIcon} className="ms-2" />
          </SubtleBadge>
        </div>
      )}
    </div>
  );
}

SurveyTypeDropdown.propTypes = {
  data: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    status: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
    surveyType: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
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
  updateSurveyType: PropTypes.func.isRequired,
  handleUpdateMethod: PropTypes.func.isRequired,
  isSurveyTypeDropdown: PropTypes.bool.isRequired,
  setSurveyTypeDropdown: PropTypes.func.isRequired,
}.isRequired;

export default SurveyTypeDropdown;
