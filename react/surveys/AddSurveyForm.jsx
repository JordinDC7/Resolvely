import { React, useEffect, useState } from "react";
import { Form, Field, ErrorMessage, useFormik, FormikProvider } from "formik";
import surveySchema from "../../schemas/surveySchema";
import lookUpService from "services/lookUpService";
import { mapLookUpItem } from "helpers/utils";
import debug from "sabio-debug";
import "./surveys.css";
import surveysService from "../../services/surveysService";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useLocation, useNavigate } from "react-router-dom";

function AddSurveyForm() {
  const _logger = debug.extend("AddSurveyForm");
  const location = useLocation();

  let taskId = null;

  if (location && location.state) {
    taskId = location.state.taskId;
  }

  const navigate = useNavigate();

  const [formData] = useState({
    name: "",
    description: "",
    statusId: "",
    surveyTypeId: "",
    taskId: null,
  });
  const onSubmitClicked = () => {
    let payload = {
      ...formik.values,
      statusId: parseInt(formik.values.statusId),
      surveyTypeId: parseInt(formik.values.surveyTypeId),
      taskId: taskId,
    };

    _logger("onSubmitClicked firing.");
    surveysService
      .surveyAdd(payload)
      .then(onSurveyAddSuccess)
      .catch(onSurveyAddError);
  };

  const [types, setTypes] = useState({
    statusId: [],
    surveyTypeId: [],
  });

  useEffect(() => {
    lookUpService
      .lookUp(["StatusTypes", "SurveyTypes"])
      .then(onGetLookupSuccess)
      .catch(onGetTypesError);
  }, []);

  const onSurveyAddSuccess = (response) => {
    _logger("this is the response from submitting: ", response);
    _logger("This should be surveyId: ", response.item);
    toast.success("Survey Added Successfully", {
      position: toast.POSITION.TOP_RIGHT,
    });
    navigate("/surveys/questions", {
      type: "SURVEY_CREATE_VIEW",
      state: { payload: response.item },
    });
  };
  const onSurveyAddError = (error) => {
    _logger("this is the error response from submitting: ", error);
    toast.error("Error Adding Survey", { position: toast.POSITION.TOP_RIGHT });
  };
  const onGetLookupSuccess = (response) => {
    _logger(response);
    const { statusTypes } = response.item;
    const { surveyTypes } = response.item;

    setTypes((prevState) => {
      let types = { ...prevState };
      types.statusId = statusTypes.map(mapLookUpItem);
      types.surveyTypeId = surveyTypes.map(mapLookUpItem);
      return types;
    });
  };
  const onGetTypesError = (error) => {
    _logger(error);
  };

  const formik = useFormik({
    enableReinitialize: true,
    initialValues: formData,
    onSubmit: onSubmitClicked,
    validationSchema: surveySchema,
  });
  return (
    <div className="container container-add-survey">
      <div className="row card-body">
        <div className="col-md-7 offset-md-3">
          <FormikProvider value={formik}>
            <Form id="userLogin mt-2 " className="card p-2 bg-light">
              <h4 className="text-center card-title card-title2">Add Survey</h4>
              <div className="form-group ">
                <label htmlFor="name" className="form-label">
                  Name
                </label>
                <Field
                  type="text"
                  className="form-control"
                  id="name"
                  name="name"
                />
                <ErrorMessage
                  name="name"
                  component="div"
                  className="error-message"
                />
              </div>
              <div className="form-group">
                <label htmlFor="description" className="form-label">
                  Description
                </label>
                <Field
                  type="text"
                  className="form-control"
                  id="description"
                  name="description"
                />
                <ErrorMessage
                  name="description"
                  component="div"
                  className="error-message"
                />
              </div>
              <div className="form-group">
                <label htmlFor="statusId" className="form-label">
                  Status Type
                </label>
                <Field
                  component="select"
                  name="statusId"
                  className="form-control"
                >
                  <option>Please Select Status Type</option>
                  {types.statusId}
                </Field>
                <ErrorMessage
                  name="statusId"
                  component="div"
                  className="error-message"
                />
              </div>
              <div className="form-group">
                <label htmlFor="surveyTypeId" className="form-label">
                  Survey Type
                </label>
                <Field
                  component="select"
                  name="surveyTypeId"
                  className="form-control"
                >
                  <option>Please Select Status Type</option>
                  {types.surveyTypeId}
                </Field>
                <ErrorMessage
                  name="surveyTypeId"
                  component="div"
                  className="error-message"
                />
              </div>
              <button
                type="submit"
                className="btn btn-success col-3 mt-2 survey-add-btn"
                // onClick={onSubmitClicked}
              >
                Add+
              </button>
            </Form>
          </FormikProvider>
        </div>
      </div>
    </div>
  );
}

export default AddSurveyForm;
