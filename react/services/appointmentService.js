import axios from "axios";
import * as helper from "./serviceHelpers"

const endpoint = `${helper.API_HOST_PREFIX}/api/appointments` 

const addAppointment = (payload) => {
    const config = {
        method: "POST",
        url: endpoint,
        data: payload,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const getAppointmentCreatedBy = () => {
    const config = {
        method: "GET",
        url: endpoint,
        crossdomain: true,
        headers: { "Content-Type": "application/json" }
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const deleteAppointment = (appointmentId) => {
    const config = {
        method: "DELETE",
        url: `${endpoint}/${appointmentId}`,
        crossdomain: true,
        headers: { "Content-Type": "application/json" }
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};

const updateAppointment = (payload, appointmentId) => {
    const config = {
        method: "PUT",
        url: `${endpoint}/${appointmentId}`,
        data: payload,
        crossdomain: true,
        headers: { "Content-Type": "application/json" },
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
};


const appointmentService = {
    addAppointment, getAppointmentCreatedBy, deleteAppointment, updateAppointment
}


export default appointmentService;

