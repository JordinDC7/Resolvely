import * as Yup from "yup";

const passRegex = new RegExp(
  "(?=.*\\d{1})(?=.*[a-z]{1})(?=.*[A-Z]{1})(?=.*[!@#$%^&*{|}?~_=+.-]{1})(?=.*[^a-zA-Z0-9])(?!.*\\s).{8,100}$",
  "i"
);

const registerValidation = Yup.object().shape({
  email: Yup.string()
    .email("Must use a valid email address.")
    .required("Email is required."),
  firstName: Yup.string().max(100).required("First Name is required."),
  lastName: Yup.string().max(100).required("Last Name is required."),
  mi: Yup.string().max(2, "2 allowed"),
  avatarUrl: Yup.string().max(255).url(),
  password: Yup.string()
    .min(8, "Password must be at least 8 characters.")
    .matches(passRegex, "Invalid password")
    .required("Password is required."),
  passwordConfirm: Yup.string()
    .oneOf([Yup.ref("password"), null], "Passwords do not match.")
    .required("Password confirmation is required."),
  roleId: Yup.number()
    .min(2, "Selection is required.")
    .max(3)
    .required("Selection is required."),
});

export default registerValidation;
