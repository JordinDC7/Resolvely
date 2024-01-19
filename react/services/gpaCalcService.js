import axios from "axios";
import * as helper from "./serviceHelpers";

const gpaCalcService = {
  endpoint: `${helper.API_HOST_PREFIX}/api/gpacalc`
};

gpaCalcService.addRow = (payload) => {
  const config = {
    method: "POST",
    url: `${gpaCalcService.endpoint}/calc`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

gpaCalcService.getByLevel = (level) => {
  const config = {
    method: "GET",
    url: `${gpaCalcService.endpoint}/level/${level}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

gpaCalcService.deleteRow = (id) => {
  const config = {
    method: "DELETE",
    url: `${gpaCalcService.endpoint}/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
gpaCalcService.getAll = () => {
  const config = {
    method: "Get",
    url: `${gpaCalcService.endpoint}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
gpaCalcService.update = (id, payload) => {
  const config = {
    method: "PUT",
    url: `${gpaCalcService.endpoint}/${id}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

export default gpaCalcService;
