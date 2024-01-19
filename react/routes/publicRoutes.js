import { lazy } from "react";
const Landing = lazy(() => import("../components/pages/landing/Landing"));
const PageNotFound = lazy(() => import("../components/errors/Error404"));
const ServerError = lazy(() => import("../components/errors/Error500"));
const ContactUs = lazy(() => import("../components/Contact/ContactUs"));
const FAQs = lazy(() => import("../components/faqs/FaqAccordion"));
const About = lazy(() => import("../components/pages/about/About"));
const LogIn = lazy(() => import("../components/pages/user/splitLayout/Login"));
const Register = lazy(() =>
  import("../components/pages/user/splitLayout/Registration")
);
const ConfirmPage = lazy(() => import("../components/pages/user/ConfirmPage"));
const ForgotPass = lazy(() =>
  import("../components/pages/user/simpleLayout/ForgetPassword")
);
const ResetPass = lazy(() =>
  import("../components/pages/user/cardLayout/ChangePassword")
);

const routes = [
  {
    path: "/",
    name: "Landing",
    exact: true,
    element: Landing,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/login",
    name: "Log-In",
    exact: true,
    element: LogIn,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/register",
    name: "Register",
    exact: true,
    element: Register,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/confirm",
    name: "Account Confirmation",
    exact: true,
    element: ConfirmPage,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/faqs",
    name: "FAQs",
    exact: true,
    element: FAQs,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/forgot",
    name: "Forgot Password",
    exact: true,
    element: ForgotPass,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/changepassword",
    name: "Change Password",
    exact: true,
    element: ResetPass,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/contact",
    name: "Contact Us",
    exact: true,
    element: ContactUs,
    roles: [],
    isAnonymous: true,
  },
  {
    path: "/about",
    name: "About",
    exact: true,
    element: About,
    roles: [],
    isAnonymous: true,
  },
];

const errorRoutes = [
  {
    path: "/error-500",
    name: "Error - 500",
    element: ServerError,
    roles: [],
    exact: true,
    isAnonymous: true,
  },
  {
    path: "*",
    name: "Error - 404",
    element: PageNotFound,
    roles: [],
    exact: true,
    isAnonymous: true,
  },
];
var allRoutes = [...routes, ...errorRoutes];

export default allRoutes;
