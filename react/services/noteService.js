import axios from "axios";
import * as helper from './serviceHelpers'

const endpoint = `${helper.API_HOST_PREFIX}/api/notes`

const addNote = (payload) => {
    const config = {
      method: "POST",
      url: endpoint,
      data: payload,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  };

  const updateNote = (payload, id) => {
    const config = {
      method: "PUT",
      url: `${endpoint}/${id}`,
      data: payload,
      crossdomain: true,
      headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
  };

const getAllNotes = (pageIndex, pageSize) => {
    const config = {
        method: "GET",
        url: `${endpoint}?pageIndex=${pageIndex}&pageSize=${pageSize}`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};


const getNotesByCreatedBy = (pageIndex, pageSize, createdBy) => {
    const config = {
        method: "GET",
        url: `${endpoint}?pageIndex=${pageIndex}&pageSize=${pageSize}&createdBy=${createdBy}`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const notesService = {
    getAllNotes, addNote, getNotesByCreatedBy, updateNote
}

export default notesService; 
