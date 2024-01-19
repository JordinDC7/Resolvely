import axios from "axios";
import * as helper from './serviceHelpers'

const endpoint = `${helper.API_HOST_PREFIX}/api/surveyinstances`

const getAllSurveyInstancesPaginated = (pageIndex, pageSize) => {
    const config = {
        method: "GET",
        url: `${endpoint}?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const getSelectBySurveyId = (pageIndex, pageSize, surveyId) => {
    const config = {
        method: "GET",
        url: `${endpoint}/surveyId?pageIndex=${pageIndex}&pageSize=${pageSize}&surveyId=${surveyId}`,
        withCredentials: true,
        crossdomain: true,
        headers: { "Content-Type": "application/json" }
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
}

const getAllSurveys = () => {
    const config = {
        method: "GET",
        url: `${helper.API_HOST_PREFIX}/api/surveyanswers/allsurveys`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const surveyInstancesService = {
    getAllSurveyInstancesPaginated, getAllSurveys, 
    getSelectBySurveyId
}

export default surveyInstancesService; 
