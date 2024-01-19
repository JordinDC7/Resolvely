import axios from "axios";
import { onGlobalError, onGlobalSuccess, API_HOST_PREFIX} from "./serviceHelpers";



const takeSurveyService = {
    endpoint: API_HOST_PREFIX + "/api",
    instance: API_HOST_PREFIX + "/api/surveyinstances",
    answers: API_HOST_PREFIX + "/api/surveyanswers"
};





const getSurveyQuestionById = (id) => {
    const config = {
        method: "GET",
        url: `${takeSurveyService.endpoint}/surveyquestion/${id}`,
        withCredentials: true,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
      };
      return axios(config).then(onGlobalSuccess).catch(onGlobalError);
}

const addSurveyInstance = (Id) =>
{
    const config = {
        method: "POST",
        url: `${takeSurveyService.instance}/${Id}`,
        withCredentials: true,
        data: Id,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
      };
      return axios(config).then(onGlobalSuccess).catch(onGlobalError);
}

const addSurveyAnswers = (payload) =>
{
    const config = {
        method: "POST",
        url: `${takeSurveyService.answers}`,
        withCredentials: true,
        data: payload,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
      };
      return axios(config).then(onGlobalSuccess).catch(onGlobalError);
}



const takeSurveys = { getSurveyQuestionById,addSurveyInstance, addSurveyAnswers }
export default takeSurveys;
