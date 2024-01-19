import Axios from "axios";
import * as helper from "./serviceHelpers";

const endpoint = `${helper.API_HOST_PREFIX}/api/events`;

const addEvent = (payload) => {
  const config = {
    method: "POST",
    url: endpoint,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const updateEvent = (payload, id) => {
  const config = {
    method: "PUT",
    url: `${endpoint}/${id}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getEvent = (pageIndex, pageSize) => {
  const config = {
    method: "GET",
    url: `${endpoint}?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getQuery = (pageIndex, pageSize, query) => {
  const config = {
    method: "GET",
    url: `${endpoint}/search?pageIndex=${pageIndex}&pageSize=${pageSize}&query=${query}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const deleteEvent = (evtId) => {
  const config = {
    method: "DELETE",
    url: `${endpoint}/${evtId}`,
    withCredentials: true,
    data: evtId,
    crossdomain: true,
    headers: { "Content-Type": "application/json" },
  };
  return Axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const eventService = {
  addEvent,
  updateEvent,
  getEvent,
  getQuery,
  deleteEvent,
};

export default eventService;
