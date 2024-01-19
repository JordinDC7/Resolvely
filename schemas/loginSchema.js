import * as Yup from "yup";

const loginValidation = Yup.object().shape({
  email: Yup.string()
    .email("Must use a valid email address.")
    .required("Field required"),
  password: Yup.string()
    .min(8, "Password must be at least 8 characters.")
    .required("Field required"),
});

export default loginValidation;
