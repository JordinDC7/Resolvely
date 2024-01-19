import axios from "axios";
import * as helper from "./serviceHelpers";
import debug from "sabio-debug";

const _logger = debug.extend("blogService");



const endpoint = `${helper.API_HOST_PREFIX}/api/blogs`

const addBlog = (payload) => {
    _logger("blogservice")
    const config = {
        method: "POST",
        url: endpoint,
        data: payload,
        crossdomain: true,
        headers: { "Content-Type": "application/json" }
    };
    return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
};

const getBlogs = (pageIndex, pageSize) => {
  const config = {
    method: "GET",
    url: `${endpoint}/paginate?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true,
    crossdomain: true,
    headers: {"Content-Type": "application/json"}
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError);
}

const deleteBlogs = (blogId) => {
  const config = {
  method: "DELETE",
  url: `${endpoint}/${blogId}`,
  crossdomain: true,
  headers: {"Content-Type": "application/json"},
  roles: ["Admin"]
  };
  return axios(config).then(() => {
    return blogId
  })
}

const updateBlogs = (payload) => {
  const config = {
    method: "PUT",
    url: `${endpoint}/${payload.id}`,
    data: payload,
    crossdomain: true,
    headers: {"Content-Type": "application/json"}
  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
}

const filterBlogs = (pageIndex, pageSize, categoryId) => {
  const config = {
    method: "GET",
    url: `${endpoint}/category/${Number(categoryId)}?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    crossdomain: true,
    headers: {"Content-Type": "application/json"}

  };
  return axios(config).then(helper.onGlobalSuccess).catch(helper.onGlobalError)
}
const blogService = {
    addBlog,
    getBlogs,
    deleteBlogs,
    updateBlogs,
    filterBlogs
 
};


export default blogService;