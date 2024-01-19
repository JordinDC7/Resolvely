import * as Yup from "yup";




const blogSchema = Yup.object().shape({
  title: Yup.string().required("Title is required").min(2).max(100),
  subject: Yup.string().required("Subject is required").min(2).max(50),
  imageUrl: Yup.string().required("ImageUrl is required").min(2),
  content: Yup.string().required("Content is required").min(2).max(4000)
});


export default blogSchema;
