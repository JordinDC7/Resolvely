import axios from "axios";
import * as helper from './serviceHelpers'

const endpoint = `${helper.API_HOST_PREFIX}/api/surveyanswers`

const getSurveyAnswersByInstanceId = (id) => {
    const config = {
        method: "GET",
        url: `${endpoint}/answers/${id}`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const surveyAnswersService = {
    getSurveyAnswersByInstanceId
    
}

export default surveyAnswersService; 
