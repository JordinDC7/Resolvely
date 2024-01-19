import axios from "axios";
import "./serviceHelpers";
import logger from "sabio-debug";
const _logger = logger.extend("autoLogin");

let {
  REACT_APP_API_HOST_PREFIX: API,
  REACT_APP_API_NODE_HOST_PREFIX: NODE_API,
  REACT_APP_TEMP_USER_ID: userId,
  REACT_APP_TEMP_USER_NAME: userName,
  REACT_APP_TEMP_USER_ROLE: userRole,
  REACT_APP_VERBOSE: isVerbose,
  REACT_APP_DOT_NET_ENABLED: isDotNetEnabled,
  REACT_APP_NODE_ENABLED: isNodeEnabled,
  REACT_APP_IS_TEST_ENV: isTestEnv,
} = process.env;
isTestEnv = isTestEnv === "true" ? true : false;
isVerbose = isVerbose === "true" ? true : false;

isDotNetEnabled = isDotNetEnabled === "true" ? true : false;
isNodeEnabled = isNodeEnabled === "true" ? true : false;

_logger(" parameters used by auto login service", {
  API,
  NODE_API,
  userId,
  userName,
  userRole,
  isVerbose,
  isDotNetEnabled,
  isNodeEnabled,
  isTestEnv,
});

if (isVerbose) {
  _logger(`
  
  Use the .env.development variables to customize the credentials this script uses to log you in automatically. 
  You can also shut this message off by setting isVerbose=false;
  
  `);
}

export const autLogInUser = {
  userId,
  userName,
  userRole,
};

class SiteTester {
  constructor(host) {
    this.host = host;
  }

  run = (onDone) => {
    _logger(
      `Tests for ${this.host}. You can disable this test if you follow this message to the originating file.`
    );
    return this.logIn(autLogInUser)
      .then(this.getCurrent)
      .then(() => this.getCurrent("POST"))
      .then((res) => {
        if (onDone) {
          onDone(res);
        }

        return res;
      });
  };

  logIn = (user, httpMethod) => {
    _logger(user, httpMethod);
    const config = {
      method: "POST",
      url: this.host + `/api/users/login`,
      data: {
        email: "TestAdmin@gmail.com",
        password: "Password1!",
      },

      headers: { "Content-Type": "application/json" },
    };

    return axios(config).then(this.onLogInSuccess).catch(this.onErrorResponse);
  };

  logOut = () => {
    const config = {
      method: "GET",
      url: `${this.host}/api/temp/auth/logout`,
      headers: { "Content-Type": "application/json" },
    };

    return axios(config);
  };

  getCurrent = (httpMethod) => {
    const config = {
      method: (typeof httpMethod === "string" && httpMethod) || "GET",
      url: this.host + "/api/temp/auth/current",
      headers: { "Content-Type": "application/json" },
    };

    return axios(config).then(this.onLogInSuccess).catch(this.onErrorResponse);
  };

  onLogInSuccess = (response) => {
    if (isVerbose) {
      _logger("Success for", {
        url: response.config.url,
        method: response.config.method,
      });
    }
    return response;
  };

  onErrorResponse = (response) => {
    _logger("Error Response for", {
      url: response.config.url,
      method: response.config.method,
    });

    return Promise.reject(response);
  };
}

let nodeLogInService, netLogInService;

netLogInService = new SiteTester(API);
nodeLogInService = new SiteTester(NODE_API);

//do not run auto login when we are in isTestEnv
if (isDotNetEnabled && !isTestEnv) {
  netLogInService.run();
}

if (isNodeEnabled && !isTestEnv) {
  nodeLogInService.run();
}
const service = {
  nodeLogInService,
  netLogInService,
  isNodeEnabled,
  isDotNetEnabled,
};
export default service;
