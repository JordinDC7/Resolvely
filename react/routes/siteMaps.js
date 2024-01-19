export const dashboardRoutes = {
  label: "Dashboard",
  labelDisable: true,
  children: [
    {
      name: "Dashboard",
      isActive: true,
      icon: "chart-pie",
      children: [
        {
          name: "Front Page",
          to: "/",
          exact: true,
          isActive: true,
        },
        {
          name: "Dashboard",
          to: "/students/dashboard",
          isActive: true,
        },
      ],
    },
  ],
};
export const appRoutes = {
  label: "app",
  children: [

    {
      name: "Front Page",
      to: "/",
      exact: true,
      roles: ["Student", "Admin"],
      isActive: true,
    },
    {
      name: "Dashboard",
      to: "/students/dashboard",
      roles: ["Student"],
      isActive: true,
    },
    {
      name: "GPA Calculator",
      icon: "calendar-alt",
      to: "/calc",
      roles: ["Student"],
      isActive: true,
    },
    {
      name: "Blogs",
      icon: "comments",
      roles: ["Student", "Admin"],
      children: [
        {
          name: "View Blogs",
          to: "/blogs/view",
          roles: ["Student", "Admin"],
          isActive: true,
        },
        {
          name: "Create Blogs",
          to: "/blogs/create",
          roles: ["Admin"],
          isActive: true,
        },
      ],
      isActive: true,
    },

    {
      name: "Organizations",
      roles: ["Admin"],
      children: [
        {
          name: "Create Organizations",
          to: "/organizations/create",
          isActive: true,
        },
        {
          name: "All Organizations",
          to: "/organizations/view",
          isActive: true,
        },
      ],
      isActive: true,
    },
    {
      name: "Appointments",
      icon: "calendar-alt",
      roles: ["Admin"],
      children: [
        {
          name: "Create Appointments",
          to: "/appointments/create",
          isActive: true,
        },
        {
          name: "My Appointments",
          to: "/appointments/myAppointments",
          isActive: true,
        },
      ],
      isActive: true,
    },
    {
      name: "Experiences",
      icon: "envelope-open",
      isActive: true,
      roles: ["Student"],
      children: [
        {
          name: "Create Experience",
          to: "/experience/create",
          isActive: true,
        },
        {
          name: " User Experiences",
          to: "/experience/list",
          isActive: true,
        },
      ],
    },
    {
      name: "Notes",
      icon: "envelope-open",
      to: "/notes",
      roles: ["Student"],
      isActive: true,
    },
    {
      name: "Surveys",
      icon: "envelope-open",
      to: "/surveys/dashboard",
      roles: ["Admin"],
      children: [
        {
          name: "Survey's Dashboard",
          to: "/surveys/dashboard",
        },
        {
          name: "Survey Instances",
          to: "/surveyinstances",
          isActive: true,
        },
      ],
      isActive: true,
    },
    {
      name: "Events",
      icon: "envelope-open",
      isActive: true,
      roles: ["Admin"],
      children: [
        {
          name: "Create Events",
          to: "/events/create",
          isActive: true,
        },
        {
          name: " User Experiences",
          to: "/experience/list",
          isActive: true,
        },
      ],
    },
    {
      name: "Create FAQs",
      to: "/FAQsAdd",
      roles: ["Admin"],
      isActive: true,
    },
  ],
};


const siteMap = [dashboardRoutes, appRoutes];
export default siteMap;
