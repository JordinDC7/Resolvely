import * as Yup from "yup";

const validationSchema = Yup.object({
  eventTypeId: Yup.number().required("Required"),
  name: Yup.string().max(255).required("Required"),
  summary: Yup.string().max(255).required("Required"),
  shortDescription: Yup.string().max(4000).required("Required"),
  venueId: Yup.number().required("Required"),
  eventStatusId: Yup.number().required("Required"),
  imageUrl: Yup.string().max(400).optional(),
  externalSiteUrl: Yup.string().max(400).optional(),
  isFree: Yup.bool().required("Required"),
  dateStart: Yup.date().required("Required"),
  dateEnd: Yup.date().required("Required"),
});

export default validationSchema;
