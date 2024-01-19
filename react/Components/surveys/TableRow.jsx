import React, { useState } from "react";
import PropTypes from "prop-types";
import Avatar from "components/common/Avatar";
import ModalComponent from "./ModalComponent";
import surveysService from "services/surveysService";
import { toast } from "react-toastify";
import "./surveys.css";
import "./modalcomponent.css";
import debug from "sabio-debug";
import StatusDropdown from "./StatusDropdown";
import SurveyTypeDropdown from "./SurveyTypeDropdown";
import SurveyNameEdit from "./SurveyNameEdit";
import DeleteSurvey from "./Delete";
import { useEffect } from "react";
import { FaRegPlusSquare } from "react-icons/fa";
import { useNavigate } from "react-router-dom";

function TableRow({
  data,
  updateParentData,
  setData,
  formattedDateModified,
  formattedDateCreated,
}) {
  const _logger = debug.extend("TableRow");

  const [surveyData, setSurveyData] = useState(data);

  const [isConfirmationModalOpen, setIsConfirmationModalOpen] = useState(false);
  const [isSurveyTypeDropdown, setSurveyTypeDropdown] = useState(false);
  const [isStatusDropdown, setStatusDropdown] = useState(false);
  const [isEditMode, setIsEditMode] = useState(false);
  _logger(surveyData, isStatusDropdown, isSurveyTypeDropdown);
  const [isDescriptionModalOpen, setIsDescriptionModalOpen] = useState(false);
  const [modalContent, setModalContent] = useState("");
  const [modalTitle, setModalTitle] = useState("");

  useEffect(() => {
    const handleEscPressed = (event) => {
      if (event.key === "Escape") {
        setIsEditMode(false);
        setStatusDropdown(false);
        setSurveyTypeDropdown(false);
        setIsConfirmationModalOpen(false);
      }
    };

    window.addEventListener("keydown", handleEscPressed);

    return () => {
      window.removeEventListener("keydown", handleEscPressed);
    };
  }, []);

  const navigate = useNavigate();

  const openDescriptionModal = (content, title) => {
    setModalContent(content);
    setModalTitle(title);
    setIsDescriptionModalOpen(true);
  };

  const handleStatusUpdate = (newStatus) => {
    setSurveyData((prevState) => {
      const updatedData = { ...prevState };

      updatedData.status = newStatus;

      return updatedData;
    });
  };

  const handleSurveyTypeUpdate = (newSurveyType) => {
    setSurveyData((prevState) => {
      const updatedData = { ...prevState };

      updatedData.surveyType = newSurveyType;

      return updatedData;
    });
  };

  const updateSurveyText = (newSurveyText) => {
    setSurveyData((prevState) => {
      const updatedData = { ...prevState };

      updatedData.name = newSurveyText;

      return updatedData;
    });
  };

  const handleCancelClick = () => {
    setIsConfirmationModalOpen(false);
    setIsDescriptionModalOpen(false);
  };

  const doubleClickHandler = (e) => {
    e.stopPropagation();
    setIsEditMode(true);
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter") {
      setIsEditMode(false);
      handleUpdateMethod();
    } else {
      return;
    }
  };

  const handleUpdateMethod = () => {
    setIsConfirmationModalOpen(true);
    handleConfirmUpdate();
  };
  const handleModalConfirmation = () => {
    setIsConfirmationModalOpen(false);

    handleConfirmUpdate();
  };

  const handleConfirmUpdate = (e) => {
    setSurveyTypeDropdown(false);
    setStatusDropdown(false);
    setIsConfirmationModalOpen(false);
    setIsEditMode(false);
    _logger(e);

    const updateSurveyData = {
      Id: surveyData.id,
      Name: surveyData.name,
      Description: surveyData.description,
      StatusId: surveyData.status.id,
      SurveyTypeId: surveyData.surveyType.id,
      TaskId: surveyData.taskId,
    };

    surveysService
      .surveysUpdateById(updateSurveyData, surveyData.id)
      .then(onUpdateSuccess)
      .catch(onUpdateError);
  };

  const onUpdateSuccess = (response) => {
    _logger(response);

    if (surveyData.status.id === 2) {
      toast.success("Successfully Deleted Survey");
    } else {
      toast.success("Successfully Updated Survey");
    }
    updateParentData(surveyData);
  };

  const onUpdateError = (response) => {
    toast.error("Error");
    _logger(response);
  };

  const onAddQuestion = () => {
    navigate("/surveys/questions", {
      state: {
        type: "SURVEY_TABLE_VIEW",
        payload: data.id,
      },
    });
  };

  return (
    <tr className="align-middle">
      <td className="text-nowrap">
        <div className="d-flex align-items-center">
          <Avatar src={data.createdBy.avatarUrl} size="l" name={data.name} />
          <SurveyNameEdit
            data={surveyData}
            key={data.id}
            handleKeyDown={handleKeyDown}
            doubleClickHandler={doubleClickHandler}
            handleCancelClick={handleCancelClick}
            handleModalConfirmation={handleModalConfirmation}
            isEditMode={isEditMode}
            updateSurveyText={updateSurveyText}
          ></SurveyNameEdit>
        </div>
      </td>
      <td
        className="text-nowrap truncated-text"
        onClick={() => openDescriptionModal(data.description, data.name)}
      >
        {data.description}
      </td>
      <td>
        <StatusDropdown
          data={data}
          key={data.id}
          status={surveyData.status}
          updateStatus={handleStatusUpdate}
          handleConfirmUpdate={handleConfirmUpdate}
          isStatusDropdown={isStatusDropdown}
          setStatusDropdown={setStatusDropdown}
          setSurveyData={setSurveyData}
          handleUpdateMethod={handleUpdateMethod}
        ></StatusDropdown>
      </td>
      <td>
        <SurveyTypeDropdown
          data={data}
          key={data.id}
          surveyType={surveyData.surveyType}
          updateSurveyType={handleSurveyTypeUpdate}
          handleConfirmUpdate={handleConfirmUpdate}
          isSurveyTypeDropdown={isSurveyTypeDropdown}
          setSurveyTypeDropdown={setSurveyTypeDropdown}
          setSurveyData={setSurveyData}
          handleUpdateMethod={handleUpdateMethod}
        ></SurveyTypeDropdown>
      </td>
      <td>{formattedDateModified}</td>
      <td>{formattedDateCreated}</td>
      <td>
        <div className="col d-flex">
          <FaRegPlusSquare
            type="button"
            onClick={onAddQuestion}
            className="text-primary"
            size={20}
            data-toggle="tooltip"
            data-placement="top"
            title="Click to add survey questions"
          />
          <DeleteSurvey
            data={data}
            key={data.id}
            handleStatusUpdate={handleStatusUpdate}
            handleConfirmUpdate={handleConfirmUpdate}
            setData={setData}
            handleSurveyTypeUpdate={handleSurveyTypeUpdate}
          />
        </div>
      </td>
      <ModalComponent
        isOpen={isConfirmationModalOpen}
        onConfirm={handleModalConfirmation}
        onCancel={handleCancelClick}
        title="Confirm Update"
        message="Are you sure you want to update the Survey?"
      />
      <ModalComponent
        isDescriptionModalOpen={isDescriptionModalOpen}
        onClose={() => setIsDescriptionModalOpen(false)}
        onConfirm={handleModalConfirmation}
        onCancel={handleCancelClick}
        title={modalTitle}
        message={modalContent}
      />
    </tr>
  );
}

TableRow.propTypes = {
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
  }).isRequired.isRequired,
  updateParentData: PropTypes.func.isRequired,
  fetchData: PropTypes.func.isRequired,
  formattedDateModified: PropTypes.func.isRequired,
  formattedDateCreated: PropTypes.func.isRequired,
}.isRequired;

export default TableRow;
