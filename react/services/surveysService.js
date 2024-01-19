import axios from "axios";
import { onGlobalError, onGlobalSuccess, API_HOST_PREFIX } from "./serviceHelpers";


const endpoint = {
  surveyUrl: API_HOST_PREFIX + "/api/surveys",
};

const surveyAdd = (payload) => {
  const config = {
    method: "POST",
    url: `${endpoint.surveyUrl}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const surveysGetAll = (pageIndex, pageSize, statusId, excluded) => {
  const config = {
    method: "GET",
    url: `${endpoint.surveyUrl}/paginate/allSurveys?pageIndex=${pageIndex}&pageSize=${pageSize}&statusId=${statusId}&excluded=${excluded}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const surveysUpdateById = (payload, id) => {
  const config = {
    method: "PUT",
    url: `${endpoint.surveyUrl}/${id}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const surveysDeleteById = (id) => {
  const config = {
    method: "DELETE",
    url: `${endpoint.surveyUrl}/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const surveysService = {
  surveyAdd,
  surveysGetAll,
  surveysUpdateById,
  surveysDeleteById,
};
export default surveysService;
