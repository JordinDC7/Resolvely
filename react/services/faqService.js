import axios from "axios";
import * as helper from "./serviceHelpers"

var faqService = 
{
    endpoint: helper.API_HOST_PREFIX + "/api/faqs",
}

faqService.add = (payload) => {
  const config = {
    method: "POST",
    url: `${faqService.endpoint}`,
    headers: { "Content-Type": "application/json" },
    data: payload,
    withCredentials: true,
    crossdomain: true,
      };
      return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
   };

   faqService.getAll = () => {
    const config = {
      method: "GET",
      url: `${faqService.endpoint}`,
      headers: { "Content-Type": "application/json" },
      crossdomain: true,
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  };

  faqService.getById = (id) => {
    const config = {
      method: "GET",
      url: `${faqService.endpoint}/${id}`,
      withCredentials: true,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
     return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  };

  faqService.deleteById = (id) => {
    const config = {
      method: "DELETE",
      url: `${faqService.endpoint}/${id}`,
      headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
    };

    faqService.update = (payload, id) => {
    const config = {
      method: "PUT",
      url: `${faqService.endpoint}/${id}`,
      headers: { "Content-Type": "application/json" },
      data: payload,
      };
      return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
    };
export default faqService;