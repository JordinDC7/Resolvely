import axios from "axios";
import * as helper from "./serviceHelpers";

const emailService = {
  endpoint: `${helper.API_HOST_PREFIX}/api/emails`
};

emailService.sendContactEmail = (payload) => {
  const config = {
    method: "POST",
    url: `${emailService.endpoint}/contact`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};
export default emailService
