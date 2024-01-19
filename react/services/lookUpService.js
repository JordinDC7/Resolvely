import axios from "axios";
import {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX,
} from "./serviceHelpers";
const endpoint = { lookUpUrl: `${API_HOST_PREFIX}/api/lookups` };

const lookUp = (payload) => {
  const config = {
    method: "POST",
    url: `${endpoint.lookUpUrl}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const lookUp3Col = (tableName) => {
  const config = {
    method: "GET",
    url: `${endpoint.lookUpUrl}?tableName=${tableName}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

const lookUpService = { lookUp, lookUp3Col };

export default lookUpService;
