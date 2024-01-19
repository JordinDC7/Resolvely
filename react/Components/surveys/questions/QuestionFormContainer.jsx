import React, { useState, useEffect, useRef } from "react";
import QuestionForm from "./QuestionForm";
import surveyQuestionService from "services/surveyQuestionService";
import QuestionList from "./QuestionList";
import Swal from "sweetalert2";
import { toast } from "react-toastify";
import "./questions.css";
import { useNavigate, useLocation } from "react-router-dom";

const QuestionFormContainer = () => {
  const isInitialRender = useRef(true);
  const navigate = useNavigate()
  const { state } = useLocation()
  const [question, setQuestion] = useState({
    questions: [],
    questionsMapped: [],
  });

  const [questionToEdit, setQuestionToEdit] = useState();
  const [change, setChange] = useState(false);

  useEffect(() => {
    if (isInitialRender.current) {
      isInitialRender.current = false;
      return;
    }
    if((state?.type === 'SURVEY_TABLE_VIEW' && state?.payload)|| (state?.type === 'SURVEY_CREATE_VIEW' && state?.payload))
    {
      setQuestion(prev => ({...prev, surveyId: state.payload,}))
    surveyQuestionService.getSurveyById(state.payload).then(onSuccess).catch(onError);
    }

  }, [change, state]);

  const onSuccess = (res) => {
   
    setQuestion((prev) => ({
      ...prev,
      questions: [...res.items],
      questionsMapped: res.items.map(mapQuestions),
    }));
  };

  const deleteCard = (id) => {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!",
      allowOutsideClick: false
    }).then((result) => {
      if (result.isConfirmed) {
        surveyQuestionService
          .deleteQuestionById(id)
          .then(deleteSuccess)
          .catch(deleteError);
      }
    });
  };

  const deleteSuccess = () => {
    Swal.fire({
      title: "Deleted!",
      text: "Your file has been deleted.",
      icon: "success",
    });
    setChange((prev) => !prev);
  };

  const deleteError = () => {
    toast.error("Question could not be deleted", {
      position: toast.POSITION.TOP_RIGHT,
    });
  };

  const editCard = (card) => {
    setQuestionToEdit(card);
  };

  const mapQuestions = (q) => {
    return (
      <QuestionList
        key={`key-${q.id}`}
        deleteCard={deleteCard}
        editCard={editCard}
        question={q}
      />
    );
  };

  const addQuestion = () => {
    setChange((prev) => !prev);

    toast.success("Question Successfully Created", {
      position: toast.POSITION.TOP_RIGHT,
    });

    Swal.fire({
      title: "Add another question?",
      text: "If no you will be redirected",
      icon: "question",
      showDenyButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, create another question",
      denyButtonText: "No, I am done",
      allowOutsideClick: false
    }).then((result) => {
      if (!result.isConfirmed) {
        navigate('/surveys/dashboard')
      }
    });
  };

  const onError = () => {
    toast.error("No questions exist with this survey", {
      position: toast.POSITION.TOP_RIGHT,
    });
  };

  return (
    <div className="container h-100">
      <div className="row">
        <div className="col">
          <h1 className="">Create Question</h1>
          <QuestionForm
            order={question.questions.length}
            questionToEdit={questionToEdit}
            addQuestion={addQuestion}
            surveyId={question.surveyId}
          />
        </div>
        <div className="d-flex col-lg-4 d-flex flex-column">
        <h2 className="pt-2">Current Questions</h2>
          <div className="row question-list-scrollable">
            
            {question.questionsMapped}
          </div>
        </div>
      </div>
    </div>
  );
};

export default QuestionFormContainer;
