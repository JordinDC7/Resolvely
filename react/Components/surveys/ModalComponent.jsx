import React from "react";
import Modal from "react-modal";
import PropTypes from "prop-types";
import "./modalcomponent.css";
import { CloseButton } from "react-bootstrap";
const ModalComponent = ({
  onCancel,
  title,
  message,
  isDescriptionModalOpen,
}) => {
  return (
    <Modal
      isOpen={isDescriptionModalOpen}
      onRequestClose={onCancel}
      contentLabel="Confirmation Modal"
      className="modal-content"
    >
      <CloseButton onClick={onCancel} />
      <h2>{title}</h2>
      <p>{message}</p>
    </Modal>
  );
};

ModalComponent.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onCancel: PropTypes.func.isRequired,
  onConfirm: PropTypes.func.isRequired,
  title: PropTypes.string.isRequired,
  message: PropTypes.string.isRequired,
}.isRequired;

export default ModalComponent;
