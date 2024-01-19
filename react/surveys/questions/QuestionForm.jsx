import React, { useState, useEffect } from "react";
import lookUpService from "services/lookUpService";
import surveyQuestionsSchema from "../../../schemas/surveyQuestoinsSchema";
import {
  Form,
  Field,
  useFormik,
  FormikProvider,
  FieldArray,
  ErrorMessage,
} from "formik";
import { FaRegTrashCan } from "react-icons/fa6";
import { mapLookUpItem } from "../../../helpers/utils";
import PropTypes from "prop-types";
import "./questions.css";
import surveyQuestionService from "services/surveyQuestionService";
import {toast} from "react-toastify"

const QuestionsForm = ({ addQuestion, questionToEdit, order, surveyId }) => {

  const [showMultiple, setShowMultiple] = useState(false);
  const [inputType, setInputType] = useState("");
  const [selectId, setSelectId] = useState(0);
  const [questionTypes, setQuestionTypes] = useState({
    qTypes: [],
    mappedQTypes: [],
  });

  const [formData] = useState({
    question: "",
    helpText: "",
    isRequired: false,
    isMultipleAllowed: false,
    questionTypeId: 0,
    statusId: 1,
    sortOrder: null,
    options: [
      {
        value: "",
        text: "",
        additionalInfo: "",
        showTextBox: false,
      },
    ],
  });

  useEffect(() => {
    if (questionToEdit) {
      formik.setValues({
        id: questionToEdit.id,
        question: questionToEdit.question,
        helpText: questionToEdit.helpText,
        isRequired: questionToEdit.isRequired,
        isMultipleAllowed: questionToEdit.isMultipleAllowed,
        questionTypeId: questionToEdit.questionType.id,
        surveyId: questionToEdit.surveyId,
        statusId: 1,
        sortOrder: questionToEdit.sortOrder,
        options: questionToEdit.answerOptions,
      });
      handleSwitch(null, questionToEdit.questionType.id);
    }

    lookUpService
      .lookUp(["QuestionTypes"])
      .then(onGetTypesSuccess)
      .catch(onGetTypesError);
  }, [questionToEdit]);

  const onGetTypesSuccess = (data) => {
    const { questionTypes } = data.item;

    setQuestionTypes((prevState) => {
      let newQuestion = { ...prevState };
      newQuestion.qTypes = questionTypes;
      newQuestion.mappedQTypes = questionTypes.map(mapLookUpItem);
      return newQuestion;
    });
  };

  const onGetTypesError = () => {
    toast.error("An error occured on the page", {
      position: toast.POSITION.TOP_RIGHT});
  };

  const onSubmitClick = () => {
    
    let payload = formik.values;
    if (!payload.sortOrder) {
      payload.sortOrder = order + 1;
    }

    payload.questionTypeId = selectId;

    payload.surveyId = surveyId

    if (payload.questionTypeId === 1) {
      payload.options[0].text = "";
    } else if (payload.questionTypeId === 2) {
      payload.options[0].text = "Short Text";
    } else if (payload.questionTypeId === 3) {
      payload.options[0].text = "Long Text";
    } else if (payload.questionTypeId === 4) {
      payload.options[0].text = "File Upload";
    }

    if (payload.id) {
      surveyQuestionService
        .updateQuestion(payload)
        .then(onSuccess)
        .catch(onError);
    } else {
      surveyQuestionService
        .createQuestion(payload)
        .then(onSuccess)
        .catch(onError);
    }
  };

  const onSuccess = () => {
    addQuestion(formik.values);
    formik.setTouched({}, false);
    formik.resetForm();
    setInputType("");
    setShowMultiple(false);
    setSelectId(0);
  };

  const onError = () => {
    toast.error("There was an error creating question", {
      position: toast.POSITION.TOP_RIGHT});
  };

  const resetForm = () => {
    formik.setTouched({}, false);
    formik.resetForm();
    setInputType("");
    setShowMultiple(false);
    setSelectId(0);
  };

  const handleSwitch = (e, id) => {

    let value = null;

    if (id !== null) {
      value = id;
    } else if (e !== null) {
      value = Number(e.currentTarget.value);
    }

    switch (value) {
      case 1:
        !showMultiple && setShowMultiple(true);

        setSelectId(value);

        break;

      case 2:
        showMultiple && setShowMultiple(false);

        setSelectId(value);

        setInputType(
          <Field
            className="form-control"
            type="text"
            name="shortText"
            readOnly
          />
        );

        break;

      case 3:
        showMultiple && setShowMultiple(false);

        setSelectId(value);

        setInputType(
          <Field
            className="form-control"
            as="textarea"
            name="longText"
            readOnly
          />
        );

        break;

      case 4:
        showMultiple && setShowMultiple(false);

        setSelectId(value);

        return setInputType(
          <Field
            className="form-control"
            type="file"
            id="formFile"
            name="fileUpload"
            disabled
          />
        );

      default:
        return inputType;
    }
  };

  const formik = useFormik({
    enableReintialize: true,
    initialValues: formData,
    onSubmit: onSubmitClick,
    validationSchema: surveyQuestionsSchema,
  });
  return (
    <div className="bg-white survey-question-form shadow rounded-1 pt-4 pb-4">
      <FormikProvider value={formik}>
        <Form>
          <div className="row w-100 m-auto">
            <div className="col mb-4">
              <Field
                name="question"
                className="form-control form-control-lg"
                placeholder="Question"
              />
              <ErrorMessage name="question" component="div" />
            </div>
            <div className="col-sm-4">
              <Field
                component="select"
                name="questionTypeId"
                className="form-select form-select-lg"
                onChange={(e) => handleSwitch(e, null)}
                value={selectId}
              >
                <option>Answer type</option>
                {questionTypes.mappedQTypes}
              </Field>
            </div>
          </div>
          <div className="row w-100 m-auto">
            <div className="col mb-4">
              <Field
                name="helpText"
                className="form-control form-control-lg"
                id="helpText"
                placeholder="Add help text"
              />
            </div>

            {showMultiple && (
              <div className="d-flex pt-2 col-sm-4">
                <div className="custom-control custom-checkbox w-100 d-flex gap-2 pt-2 align-items-start">
                  <div>
                    <Field
                      type="checkbox"
                      role="switch"
                      className="border border-primary m-0"
                      name="isMultipleAllowed"
                      id="customCheck1"
                    ></Field>
                  </div>
                  <div>
                    <label
                      className="custom-control-label h-100 m-0"
                      htmlFor="customCheck1"
                    >
                      Allow multiple answers?
                    </label>
                  </div>
                </div>
              </div>
            )}
          </div>
          <div className="row  m-auto">
            <div className="col col-sm-5 w-100">
              {!showMultiple ? (
                inputType
              ) : (
                <FieldArray name="options">
                  {({ push, remove }) => (
                    <>
                      {formik.values.options.map((option, index) => (
                        <div key={index} className="w-50 row mb-3">
                          <div className="input-group-prepend d-flex m-auto">
                            <div className="input-group-text">
                              <Field
                                type="checkbox"
                                value="false"
                                name={`options.${index}.showTextBox`}
                                disabled
                              />
                            </div>
                            <div className="col">
                              <Field
                                placeholder="Answer value"
                                type="text"
                                className="form-control sq-input-noBorder"
                                name={`options[${index}].value`}
                                aria-label="Text input with checkbox"
                              />
                            </div>

                            <button
                              className="btn-sm button-no-border bg-transparent"
                              type="button"
                              onClick={() => remove(index)}
                            >
                              <FaRegTrashCan
                                className="text-danger"
                                size={30}
                              />
                            </button>
                          </div>
                        </div>
                      ))}
                      {showMultiple && (
                        <button
                          type="button"
                          className="btn btn-primary add-another-value-btn mb-2"
                          onClick={() =>
                            push({
                              additionalInfo: "",
                              showTextBox: false,
                              text: "",
                              value: "",
                            })
                          }
                        >
                          Add value
                        </button>
                      )}
                    </>
                  )}
                </FieldArray>
              )}
            </div>
          </div>
          <div className="container d-flex">
            <div className="col d-flex">
              <div className="form-check form-switch h-100 mb-0 d-flex gap-2 align-items-center">
                <Field
                  className="form-check-input"
                  type="checkbox"
                  role="switch"
                  name="isRequired"
                />
                <label
                  className="form-check-label m-auto pt-1"
                  htmlFor="flexSwitchCheckChecked"
                >
                  Required
                </label>
              </div>
            </div>

            <div className="d-flex justify-content-end gap-2 col mt-3">
              <button className="btn btn-primary" type="submit">
                Save
              </button>
              <button className="btn btn-secondary" onClick={resetForm}>
                Reset
              </button>
            </div>
          </div>
        </Form>
      </FormikProvider>
    </div>
  );
};

QuestionsForm.propTypes = {
  order: PropTypes.number,
  addQuestion: PropTypes.func.isRequired,
  questionToEdit: PropTypes.shape({
    id: PropTypes.number,
    question: PropTypes.string,
    helpText: PropTypes.string,
    isRequired: PropTypes.bool,
    isMultipleAllowed: PropTypes.bool,
    questionType: PropTypes.shape({
      id: PropTypes.number,
    }),
    surveyId: PropTypes.number,
    statusId: PropTypes.number,
    sortOrder: PropTypes.number,
    answerOptions: PropTypes.arrayOf(
      PropTypes.shape([
        {
          id: PropTypes.id,
          value: PropTypes.string,
          text: PropTypes.string,
          additionalInfo: PropTypes.string,
          showTextBox: PropTypes.bool,
        },
      ])
    ),
  }),
  surveyId: PropTypes.number.isRequired
};

export default React.memo(QuestionsForm);
