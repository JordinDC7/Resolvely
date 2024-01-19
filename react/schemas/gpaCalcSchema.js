import * as Yup from "yup"


const calcSchema = Yup.object().shape({
  components:Yup.array().of(
    Yup.object().shape(
      {
        levelTypeId: Yup.number().required(),
        courseId:Yup.number().required("Select course"),
        gradeTypeId:Yup.number().required("Select grade"),
        courseWeightTypeId:Yup.number().required("Select weight"),
        credits:Yup.number().required("Please add credits"),
        semester:Yup.number().required("Please add a semester")
      }
    )
  )
    })
export default calcSchema;
