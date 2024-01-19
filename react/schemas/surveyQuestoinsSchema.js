import * as Yup from "yup";

const surveyQuestionsSchema = Yup.object().shape({
  question: Yup.string().required("Please provide a question"),
  helpText: Yup.string(),
  isRequired: Yup.boolean().required(),
  isMultipleAllowed: Yup.boolean().required(),
  questionTypeId: Yup.number().required("Please select an answer type"),
  options: Yup.array().of(
    Yup.object().shape({
      value: Yup.string(),
      text: Yup.string(),
      additionalValue: Yup.string(),
      showTextBox: Yup.boolean().required(),
    })
  ),
});

export default surveyQuestionsSchema;
