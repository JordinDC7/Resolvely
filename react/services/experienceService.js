import Axios from "axios";
import * as helper from "./serviceHelpers"
import debug from "sabio-debug";

const _logger = debug.extend("Login");

const experienceService = {endpoint: `${helper.API_HOST_PREFIX}/api/experiences`}

experienceService.addExperience = (payload) =>{
    _logger("Request to ADD an Experience is firing", payload)
      const config = {
        method: "POST",
        url: `${experienceService.endpoint}`,
        data: payload,
        withCredentials: true,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  };


  experienceService.getAllExp = (pageIndex, pageSize) =>{
    _logger("Request to Get All an Experiences is firing")
    const config = {
      method: "GET",
      url: `${experienceService.endpoint}/paginate/?PageIndex=${pageIndex}&PageSize=${pageSize}`,
      withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);

  }

  experienceService.updateExp = (id, payload) =>{
    _logger("Request to Update an Experience is firing")
    const config = {
      method: "PUT",
      url: `${experienceService.endpoint}/${id}`,
      data: payload,
      withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  }


  experienceService.deleteExp = (id) =>{
    _logger("Request to Delete an Experience is firing")
    const config = {
      method: "DELETE",
      url: `${experienceService.endpoint}/${id}`,
      withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  }

  export default experienceService