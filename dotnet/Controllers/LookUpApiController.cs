using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/lookups")]
    [ApiController]
    public class LookUpApiController : BaseApiController
    {

        private ILookUpService _lookUpService = null;
        private IAuthenticationService<int> _authenticationService = null;
        public LookUpApiController(ILookUpService service
            , ILogger<LookUpApiController> logger
            , IAuthenticationService<int> authenticationService) : base(logger)
        {
            _lookUpService = service;
            _authenticationService = authenticationService;
        }

        [HttpPost()]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Dictionary<string, List<LookUp>>>> GetType(string[] tableNames)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Dictionary<string, List<LookUp>> lookup = _lookUpService.GetMany(tableNames);

                if (lookup == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not Found");
                }
                else
                {
                    response = new ItemResponse<Dictionary<string, List<LookUp>>> { Item = lookup };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ItemsResponse<LookUp3Col>> Get3Col(string tableName)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<LookUp3Col> lookUps = _lookUpService.GetLookUp3Col(tableName);

                if (lookUps == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Comment Was Not Found");
                }
                else
                {
                    response = new ItemsResponse<LookUp3Col> { Items = lookUps };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }
    }
}
