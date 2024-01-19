import Axios from "axios";
import * as helper from "./serviceHelpers";
import debug from "sabio-debug";

const _logger = debug.extend("Login");

const locationService = { endpoint: `${helper.API_HOST_PREFIX}/api/locations` };

locationService.addLocation = (payload) => {
  _logger("Request to ADD a Location is firing");
  const config = {
    method: "POST",
    url: `${locationService.endpoint}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

locationService.updateLocation = (payload, id) => {
  _logger("Request to UPDATE a Location is firing");
  const config = {
    method: "PUT",
    url: `${locationService.endpoint}/${id}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

locationService.getById = (id) => {
  _logger("Request to GET a Location By Id is firing");
  const config = {
    method: "GET",
    url: `${locationService.endpoint}/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

export default locationService;
