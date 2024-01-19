import * as Yup from "yup";

const passRegex = new RegExp(
  "(?=.*\\d{1})(?=.*[a-z]{1})(?=.*[A-Z]{1})(?=.*[!@#$%^&*{|}?~_=+.-]{1})(?=.*[^a-zA-Z0-9])(?!.*\\s).{8,100}$",
  "i"
);

const resetPassSchema = Yup.object().shape({
  password: Yup.string()
    .matches(passRegex, "Invalid password")
    .required("Please enter a new password"),
  confirmPassword: Yup.string()
    .min(8)
    .oneOf([Yup.ref("password"), null], "Passwords do not match.")
    .required("Password confirmation is required."),
  email: Yup.string(),
  token: Yup.string(),
});

export default resetPassSchema;
