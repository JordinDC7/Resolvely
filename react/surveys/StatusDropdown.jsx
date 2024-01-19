import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCheckCircle,
  faExclamationCircle,
  faInfoCircle,
} from "@fortawesome/free-solid-svg-icons";
import SubtleBadge from "components/common/SubtleBadge";
import "./surveys.css";
import "./modalcomponent.css";

function StatusDropdown({
  data,
  updateStatus,
  handleUpdateMethod,
  isStatusDropdown,
  setStatusDropdown,
}) {
  const [selectedStatus, setSelectedStatus] = useState(data.status.id);

  const getStatusInfo = (statusId) => {
    const id = parseInt(statusId, 10);
    switch (id) {
      case 1:
        return { name: "Active", class: "badge-subtle-success rounded-pill" };
      case 2:
        return {
          name: "Inactive",
          class: "badge-subtle-secondary rounded-pill",
        };
      case 3:
        return { name: "Pending", class: "badge-subtle-warning rounded-pill" };
      case 4:
        return { name: "Cancelled", class: "badge-subtle-danger rounded-pill" };
      default:
        return { name: "", class: "" };
    }
  };

  const getStatusIcon = (statusName) => {
    switch (statusName) {
      case "Active":
        return faCheckCircle;
      case "Inactive":
        return faInfoCircle;
      case "Pending":
        return faExclamationCircle;
      default:
        return faCheckCircle;
    }
  };

  useEffect(() => {
    setSelectedStatus(data.status.id);
  }, [data.status.id]);

  const handleStatusChange = (e) => {
    const newStatusId = parseInt(e.target.value, 10);
    setSelectedStatus(newStatusId);

    const newStatus = {
      ...data,
      id: newStatusId,
    };

    updateStatus(newStatus);
  };

  const statusInfo = getStatusInfo(data.status.id);
  const statusName = statusInfo.name;
  const statusClass = statusInfo.class;
  const statusIcon = getStatusIcon(data.status.name);

  return (
    <div className="status-container pointer-text-control">
      {isStatusDropdown ? (
        <>
          <select
            className="select-dropdown pointer-text-control"
            value={selectedStatus}
            onChange={handleStatusChange}
          >
            <option value="">Status: {statusName}</option>
            <option value="1">Active</option>
            <option value="2">Inactive</option>
            <option value="3">Pending</option>
            <option value="4">Cancelled</option>
          </select>
          <button
            className="save-button"
            onClick={(e) => handleUpdateMethod(e)}
          >
            Save
          </button>
        </>
      ) : (
        <div onClick={() => setStatusDropdown(true)}>
          <SubtleBadge pill className={statusClass}>
            {statusName}
            <FontAwesomeIcon icon={statusIcon} className="ms-2" />
          </SubtleBadge>
        </div>
      )}
    </div>
  );
}

StatusDropdown.propTypes = {
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
  updateStatus: PropTypes.func.isRequired,
  handleUpdateMethod: PropTypes.func.isRequired,
  isStatusDropdown: PropTypes.bool.isRequired,
  setStatusDropdown: PropTypes.func.isRequired,
}.isRequired;

export default StatusDropdown;
