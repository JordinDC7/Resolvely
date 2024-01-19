import axios from "axios";
import {
  onGlobalSuccess,
  onGlobalError,
  API_HOST_PREFIX,
} from "./serviceHelpers.js";

const fileService = {
  endpoint: API_HOST_PREFIX + "/api/file",
};

fileService.uploadFile = (files) => {
  const config = {
    method: "POST",
    url: fileService.endpoint,
    data: files,
    crossdomain: true,
    headers: { "Content-Type": "multipart/form-data" },
  };

  return axios(config).then(onGlobalSuccess).catch(onGlobalError);
};

export default fileService;
