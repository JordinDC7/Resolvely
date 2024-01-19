import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import debug from "sabio-debug";
import { FaRegEdit } from "react-icons/fa";
import { FaRegTrashCan } from "react-icons/fa6";

const QuestionList = ({ question, editCard, deleteCard }) => {

  const _logger = debug.extend("SurveyQuestionsForm");
  _logger("ID CHECK", question);

  const [pageData, setPageData] = useState();

  useEffect(() => {
    pageData && setPageData(...question);
  });

  const deleteLocalCard = () => {
    deleteCard(question.id);
  };

  const editLocalCard = () => {
    editCard(question);
  };

  return (
    <div className="card w-100 mb-2" style={{ width: "18rem" }}>
      <div className="card-body">
        <h3 className="card-title mb-3">{question.question}</h3>
        <h6>{question.questionType.name}</h6>
        <div className="d-flex justify-content-end">
          <button
            className="card-link button-no-border bg-transparent"
            data-page={`/surveys/questions/${question.id}`}
            onClick={editLocalCard}
          >
            <FaRegEdit className="text-primary" size={25} />
          </button>
          <button
            onClick={deleteLocalCard}
            className="card-link button-no-border bg-transparent"
          >
            <FaRegTrashCan className="text-danger" size={25} />
          </button>
        </div>
      </div>
    </div>
  );
};

QuestionList.propTypes = {
  id: PropTypes.number.isRequired,
  question: PropTypes.string.isRequired,
  questionType: PropTypes.shape({
    name: PropTypes.string,
  }),
  answerOptions: PropTypes.arrayOf(
    PropTypes.shape({
      value: PropTypes.string.isRequired,
      text: PropTypes.string.isRequired,
      additionalInfo: PropTypes.string.isRequired,
      showTextBox: PropTypes.bool.isRequired,
    })
  ),
  editCard: PropTypes.func.isRequired,
  deleteCard: PropTypes.func.isRequired,
};

export default QuestionList;
