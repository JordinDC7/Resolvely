import React, { useState, useEffect } from "react";
import "./takesurvey.css";
import Options from "./Options";
import Questions from "./Questions";
import ProgressBar from "./ProgressBar";
import SubmitSurvey from "./SubmitSurvey";
import takeSurveysService from "../../services/takeSurveyService";
import debug from "sabio-debug";
import { toast } from "react-toastify";

function TakeSurvey() {
  const _logger = debug.extend("TakeSurvey");
  const [quizState, setQuizState] = useState({
    questions: [],
    currentQuestionIndex: 0,
    selectedAnswers: {},
    isMultipleAllowed: {},
    textInputAnswers: {},
  });

  const handleNextQuestion = () => {
    setQuizState((prevState) => ({
      ...prevState,
      currentQuestionIndex: prevState.currentQuestionIndex + 1,
    }));
  };

  const handlePreviousQuestion = () => {
    if (quizState.currentQuestionIndex === 0) {
      return;
    }
    setQuizState((prevState) => ({
      ...prevState,
      currentQuestionIndex: prevState.currentQuestionIndex - 1,
    }));
  };

  useEffect(() => {
    const surveyId = 3;
    takeSurveysService
      .getSurveyQuestionById(surveyId)
      .then(onSurveyGetSuccess)
      .catch(onSurveyGetError);
  }, []);

  const onSurveyGetSuccess = (response) => {
    let isMultipleAllowedMap = {};

    response.items.forEach((item) => {
      isMultipleAllowedMap[item.id] = item.isMultipleAllowed || false;
    });

    setQuizState((prevState) => ({
      ...prevState,
      questions: response.items,
      isMultipleAllowed: isMultipleAllowedMap,
    }));
  };
  const onSurveyGetError = (error) => {
    _logger(error);
  };

  const updateMultipleChoiceAnswers = (currentAnswers, option, optionValue) => {
    const isOptionSelected = currentAnswers.some(
      (answer) => answer.option === option
    );

    return isOptionSelected
      ? currentAnswers.filter((answer) => answer.option !== option)
      : [...currentAnswers, { value: optionValue, option }];
  };

  const updateSingleChoiceAnswer = (optionValue, option) => {
    return { value: optionValue, option };
  };

  const onOptionSelect = (questionId, optionValue, option) => {
    setQuizState((prevState) => {
      const isMultiple = prevState.isMultipleAllowed[questionId];

      let currentAnswers =
        prevState.selectedAnswers[questionId] || (isMultiple ? [] : "");

      if (isMultiple) {
        currentAnswers = updateMultipleChoiceAnswers(
          currentAnswers,
          option,
          optionValue
        );
      } else {
        currentAnswers = updateSingleChoiceAnswer(optionValue, option);
      }

      return {
        ...prevState,
        selectedAnswers: {
          ...prevState.selectedAnswers,
          [questionId]: currentAnswers,
        },
      };
    });
  };

  const onAnswerOptionTextInputChange = (optionId, event) => {
    const textInput = event.target.value;

    setQuizState((prevState) => {
      const updatedTextInputAnswers = {
        ...prevState.textInputAnswers,
        [optionId]: textInput,
      };

      return {
        ...prevState,
        textInputAnswers: updatedTextInputAnswers,
      };
    });
  };

  const onSubmit = () => {
    //
    const { questions, textInputAnswers, selectedAnswers } = quizState;

    const allQuestionsAnswered = questions.every(
      (question) =>
        textInputAnswers[question.id] || selectedAnswers[question.id]
    );

    if (!allQuestionsAnswered) {
      toast.error("Please Answer All Questions.", {
        position: toast.POSITION.TOP_RIGHT,
      });
      return;
    }

    //
    takeSurveysService
      .addSurveyInstance(3)
      .then((response) => onSurveyInstanceAddSuccess(response))
      .then(onSubmitSuccessToast)
      .catch(onSurveyInstanceAddError);
  };

  const onSurveyInstanceAddSuccess = (response) => {
    const surveyInstanceId = response.item;

    insertAnswers(surveyInstanceId);
  };
  const onSubmitSuccessToast = () => {
    //
    toast.success("Quiz Answers Submitted Successfully", {
      position: toast.POSITION.TOP_RIGHT,
    });
    //
  };
  const onSurveyInstanceAddError = (error) => {
    _logger(error);
    //
    toast.error("Error Submitting Quiz Answers", {
      position: toast.POSITION.TOP_RIGHT,
    });
    //
  };
  const insertAnswers = (surveyInstanceId) => {
    const { questions, selectedAnswers, textInputAnswers } = quizState;

    questions.forEach((question) => {
      let answer = selectedAnswers[question.id] || textInputAnswers;
      if (!answer) return;

      const answerArray = Array.isArray(answer) ? answer : [answer];

      answerArray.forEach((answerOption) => {
        const payload = processAnswerOption(
          answerOption,
          question.id,
          surveyInstanceId
        );
        _logger("payload:", payload);

        takeSurveysService
          .addSurveyAnswers(payload)
          .then(onSurveyAnswersAddSuccess)
          .catch(onSurveyAnswersAddError);
      });
    });
  };

  const processAnswerOption = (answerOption, questionId, surveyInstanceId) => {
    let answerOptionId = answerOption.option.id;

    let answer = quizState.textInputAnswers[answerOptionId];

    if (answer === null || answer === undefined || answer === "Null") {
      answer = "N/A";
    }

    return {
      instanceId: surveyInstanceId,
      questionId,
      answerOptionId,
      answer: answer,
    };
  };

  const onSurveyAnswersAddSuccess = (response) => {
    _logger(response);
  };
  const onSurveyAnswersAddError = (error) => {
    _logger(error);
  };

  const currentQuestion = quizState.questions[quizState.currentQuestionIndex];
  const currentQuestionIndex = quizState.currentQuestionIndex;
  const totalQuestions = quizState.questions.length;
  const isLastQuestion = quizState.currentQuestionIndex === totalQuestions - 1;
  const progress = ((currentQuestionIndex + 1) / totalQuestions) * 100;

  return (
    <div className="quiz-container">
      <ProgressBar progress={progress} />
      {currentQuestion && (
        <>
          <Questions question={currentQuestion.question} />
          <div className="helpText">
            <strong>Help: </strong> {currentQuestion.helpText}
          </div>
          <Options
            answerOptions={currentQuestion.answerOptions}
            questionId={currentQuestion.id}
            onOptionSelect={onOptionSelect}
            quizState={quizState}
            onAnswerOptionTextInputChange={onAnswerOptionTextInputChange}
          />
        </>
      )}
      <SubmitSurvey
        onPrevious={handlePreviousQuestion}
        onNext={handleNextQuestion}
        isLastQuestion={isLastQuestion}
        onSubmit={onSubmit}
      />
    </div>
  );
}

export default TakeSurvey;
