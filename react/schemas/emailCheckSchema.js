import * as Yup from "yup";

const emailSchema = Yup.object().shape({
  email: Yup.string()
    .email("Must use a valid email address.")
    .required("Email is required"),
});

export default emailSchema;
