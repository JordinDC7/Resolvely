import * as Yup from "yup";

const surveySchema = Yup.object({
    name: Yup.string().max(100).required("Survey name is required"),
    description: Yup.string().min(5).max(2000).required("Survey description is required"),
    statusId: Yup.number().transform((value, originalValue) => {
        return isNaN(originalValue) ? undefined : value;
      }).required("Status is required"),
      surveyTypeId: Yup.number().transform((value, originalValue) => {
        return isNaN(originalValue) ? undefined : value;
      }).required("Survey type is required"),
    taskId: Yup.number().nullable(),    
})
export default surveySchema;