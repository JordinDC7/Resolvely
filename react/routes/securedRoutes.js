import DetailedViewEvent from "components/events/DetailedViewEvent";
import { lazy } from "react";
const AdminDashboard = lazy(() => import("components/dashboards/analytics/AdminDashboard"))
const AllUsersList = lazy(() => import("components/dashboards/analytics/AllUsersList"))
const LocationForm = lazy(() => import("components/locations/LocationForm"));
const SurveysDash = lazy(() => import("components/surveys/SurveysDash"));
const GpaCalc = lazy(() => import("../components/GpaCalc/MainGpa"));
const ExperienceForm = lazy(() =>
  import("components/experience/ExperienceForm")
);
const ExperienceList = lazy(() =>
  import("components/experience/ExperienceList")
);
const AnalyticsDashboards = lazy(() =>
  import("../components/dashboards/analytics/index")
);
const EventsForm = lazy(() => import("components/events/EventsForm"));
const PageNotFound = lazy(() => import("../components/errors/Error404"));
const AddOrganization = lazy(() =>
  import("../components/pages/organizations/AddOrganization")
);
const AppointmentAddForm = lazy(() =>
  import("../components/pages/appointments/AppointmentAddForm")
);
const AppointmentTables = lazy(() =>
  import("../components/pages/appointments/AppointmentTables")
);

const FAQsAdd = lazy(() => import("../components/faqs/FaqAdd"));
const FAQsReview = lazy(() => import("../components/faqs/FaqsReview"));

const BlogForm = lazy(() => import("../components/Blogs/BlogForm"));
// dashboard
const Organizations = lazy(() =>
  import("../components/pages/organizations/Organizations")
);
const DetailedViewOrganization = lazy(() =>
  import("../components/pages/organizations/DetailedViewOrganization")
);
const Blogs = lazy(() => import("../components/Blogs/Blogs"));
const DetailedViewBlog = lazy(() =>
  import("../components/Blogs/DetailedViewBlog")
);
const Notes = lazy(() => import("../components/pages/note/NotesTask"));
const SurveyInstances = lazy(() =>
  import("../../src/components/pages/surveyinstances/SurveyInstances")
);
const ProfileWizard = lazy(() =>
  import("../components/pages/user/profileWizard/ProfileWizard")
);
const QuestionFormContainer = lazy(() =>
  import("../components/surveys/questions/QuestionFormContainer")
);

const MainDash = lazy(() => import("../components/dashboards/MainDash"))
const TakeSurvey = lazy(() => import("../../src/components/take-survey/TakeSurvey"));
const AddSurveyForm = lazy(() => import("../../src/components/surveys/AddSurveyForm"));
const dashboardRoutes = [
  {
    path: "/dashboard",
    name: "Dashboards",
    icon: "uil-home-alt",
    header: "Navigation",
    children: [
      {
        path: "/dashboard/analytics",
        name: "Analytics",
        element: AnalyticsDashboards,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/organizations/create",
        name: "Organizations",
        element: AddOrganization,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/organizations/view",
        name: "Organizations",
        element: Organizations,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/organizations/view/:id",
        name: "Organization Details",
        element: DetailedViewOrganization,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/appointments/create",
        name: "Appointments Add",
        element: AppointmentAddForm,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/appointments/myAppointments",
        name: "Appointments Get",
        element: AppointmentTables,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/appointments/create/:id",
        name: "Appointments Update",
        element: AppointmentAddForm,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/surveyinstances",
        name: "Survey Instances",
        element: SurveyInstances,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/faqsadd",
        name: "FAQs",
        element: FAQsAdd,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/faqsreview",
        name: "FAQsReview",
        element: FAQsReview,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/blogs/create",
        name: "Blogs",
        exact: true,
        element: BlogForm,
        roles: ["Admin"],
        isAnonymous: true,
      },
      {
        path: "/blogs/view",
        name: "Blogs",
        element: Blogs,
        roles: [],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/blogs/view/:id",
        name: "Blog Details",
        element: DetailedViewBlog,
        roles: [],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/blogs/create/:id",
        name: "Blogs",
        exact: true,
        element: BlogForm,
        roles: ["Admin"],
        isAnonymous: true,
      },
      {
        path: "/experience/create",
        name: "Experience",
        element: ExperienceForm,
        roles: ["Student"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/experience/edit/:id",
        name: "Experience",
        element: ExperienceForm,
        roles: ["Student"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/experience/list",
        name: "ExperienceList",
        element: ExperienceList,
        roles: ["Student"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/events/create",
        name: "Event Form",
        roles: ["Admin"],
        exact: true,
        element: EventsForm,
        isAnonymous: false,
      },
      {
        path: "/admin/dashboard",
        name: "Admin Dashboard",
        element: AdminDashboard,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/admin/dashboard/users",
        name: "Admin Users",
        element: AllUsersList,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/events/edit/:id",
        name: "Event Form",
        roles: ["Admin"],
        exact: true,
        element: EventsForm,
        isAnonymous: false,
     },
     {
        path: "/surveys/questions",
        name: "Survey Questions",
        element: QuestionFormContainer,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/events/view/:id",
        name: "Event Details",
        element: DetailedViewEvent,
        roles: [],
        exact: true,
        isAnonymous: false,
      },
      {
        path: "/surveys/take-survey",
        name: "Take Survey",
        roles: ["Admin"],
        exact: true,
        element: TakeSurvey,
        isAnonymous: false,
      },
    ],
  },
  {
    path: "/surveys",
    name: "Surveys",
    icon: "uil-home-alt",
    header: "Navigation",
    children: [
      {
        path: "/surveys/dashboard",
        name: "SurveysDashboard",
        element: SurveysDash,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },{
      path: "/surveys/addsurvey",
        name: "addsurvey",
        element: AddSurveyForm,
        roles: ["Admin"],
        exact: true,
        isAnonymous: false,
      },
    ],
  },
  {
    path: "/calc",
    name: "GpaCalc",
    exact: true,
    element: GpaCalc,
    roles: ["Student"],
    isAnonymous: false,
  },
  {
    path: "/notes",
    name: "Notes",
    exact: true,
    element: Notes,
    roles: ["Student"],
    isAnonymous: false,
  },
  {
    path: "/user",
    name: "Profile Setup",
    exact: true,
    roles: ["Student", "Parent", "Admin"],
    isAnonymous: false,
    children: [
      {
        path: "/user/profilewizard",
        name: "Profile Setup",
        exact: true,
        element: ProfileWizard,
        roles: ["Student", "Parent", "Admin"],
        isAnonymous: false,
      },
    ],
  },
  {
    path: "/students/dashboard",
    name: "Dashboard",
    exact: true,
    element: MainDash,
    roles: ["Student"],
    isAnonymous: false,
  }
];

const test = [
  {
    path: "/locationtest",
    name: "LocationForm",
    exact: true,
    element: LocationForm,
    roles: [],
    isAnonymous: false,
  },
  {
    path: "/secured",
    name: "A Secured Route",
    exact: true,
    element: AnalyticsDashboards,
    roles: ["Fail"],
    isAnonymous: false,
  },
  {
    path: "/secured2",
    name: "A Secured Route",
    exact: true,
    element: AnalyticsDashboards,
    roles: ["Admin"],
    isAnonymous: false,
  },
];

const errorRoutes = [
  {
    path: "*",
    name: "Error - 404",
    element: PageNotFound,
    roles: [],
    exact: true,
    isAnonymous: false,
  },
];

const allRoutes = [...dashboardRoutes, ...test, ...errorRoutes];

export default allRoutes;
