import axios from "axios";
import {
  onGlobalSuccess,
  onGlobalError,
  API_HOST_PREFIX,
} from "./serviceHelpers.js";

const surveyQuestionService = {
  endpoint: API_HOST_PREFIX + "/api/surveyquestion",
};

surveyQuestionService.getSurveyById = (id) => {
  const config = {
    method: "GET",
    url: `${surveyQuestionService.endpoint}/${id}`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

surveyQuestionService.createQuestion = (payload) => {
  const config = {
    method: "POST",
    url: `${surveyQuestionService.endpoint}`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

surveyQuestionService.updateQuestion = (payload) => {
  const config = {
    method: "PUT",
    url: `${surveyQuestionService.endpoint}/${payload.id}`,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

surveyQuestionService.deleteQuestionById = (id) => {
  const config = {
    method: "DELETE",
    url: `${surveyQuestionService.endpoint}/${id}`,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export default surveyQuestionService;
