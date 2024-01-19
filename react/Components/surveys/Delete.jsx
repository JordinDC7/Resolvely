import React, { useState, useEffect } from "react";
import { FaTrashAlt } from "react-icons/fa";
import "./surveys.css";
import PropTypes from "prop-types";
import ModalComponent from "./ModalComponent";

function DeleteSurvey({
  data,
  handleStatusUpdate,
  handleConfirmUpdate,
  handleSurveyTypeUpdate,
}) {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [updateCompleted, setUpdateCompleted] = useState(false);

  const handleDelete = () => {
    const newStatusId = 2;
    const newSurveyTypeId = 2;

    const newStatus = {
      ...data,
      id: newStatusId,
    };

    const newSurveyType = {
      ...data,
      id: newSurveyTypeId,
    };

    handleSurveyTypeUpdate(newSurveyType);
    handleStatusUpdate(newStatus);

    setIsModalOpen(false);
    setUpdateCompleted(true);
  };

  useEffect(() => {
    if (updateCompleted) {
      handleConfirmUpdate();
      setUpdateCompleted(false);
    }
  }, [updateCompleted]);

  return (
    <div className="d-flex align-items-center">
      <div className="d-flex align-items-center trash-icon2">
        <FaTrashAlt onClick={() => setIsModalOpen(true)} />
      </div>

      <ModalComponent
        isOpen={isModalOpen}
        onCancel={() => setIsModalOpen(false)}
        onConfirm={() => handleDelete()}
        title="Confirm Deletion"
        message="Are you sure you want to delete this Survey?"
      />
    </div>
  );
}

DeleteSurvey.propTypes = {
  data: PropTypes.shape({
    id: PropTypes.number,
    name: PropTypes.string,
    description: PropTypes.string,
    status: PropTypes.shape({
      id: PropTypes.number,
      name: PropTypes.string,
    }),
    surveyType: PropTypes.shape({
      id: PropTypes.number,
      name: PropTypes.string,
    }),
    taskId: PropTypes.number,
    createdBy: PropTypes.shape({
      id: PropTypes.number,
      firstName: PropTypes.string,
      lastName: PropTypes.string,
      mi: PropTypes.string,
      avatarUrl: PropTypes.string,
    }),
    dateCreated: PropTypes.string,
    dateModified: PropTypes.string,
  }).isRequired,
  handleConfirmUpdate: PropTypes.func.isRequired,
  handleStatusUpdate: PropTypes.func.isRequired,
  handleSurveyTypeUpdate: PropTypes.func.isRequired,
}.isRequired;

export default DeleteSurvey;
