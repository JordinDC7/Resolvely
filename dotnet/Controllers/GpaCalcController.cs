using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/gpacalc")]
    [ApiController]
    public class GpaCalcController : BaseApiController
    {
        IGpaCalcService _service;
        IAuthenticationService<int> _auth;

        public GpaCalcController(IGpaCalcService service, ILogger<BaseApiController> logger, IAuthenticationService<int> auth) : base(logger)
        {
            _service = service;
            _auth = auth;
        }



        [HttpPost("calc")]
        public ActionResult<SuccessResponse> AddGpa(GpaCalcsAddRequest model)
        {
            int code = 201;
            BaseResponse response = null;
            try
            {
                int userId = _auth.GetCurrentUserId();
                _service.AddGpaCalc(model, userId);

                response = new SuccessResponse();

            }

            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> UpdateGpa(GpaCalcUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;



            try
            {
                int userId = _auth.GetCurrentUserId();
                _service.UpdateGpaCalc(model, userId);
                response = new SuccessResponse();

            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }


        [HttpGet("level/{id:int}")]
        public ActionResult<ItemsResponse<GpaCalc>> GetByLevelTypeId(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<GpaCalc> list = _service.GetByLvlTypeId(id);
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Calc not found");
                }
                else
                {
                    response = new ItemsResponse<GpaCalc> { Items = list };

                }


            }
            catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response); 
        }

        [HttpGet]
        public ActionResult<ItemsResponse<GpaCalc>> GetAll()
        {
            int code= 200;
            BaseResponse response = null;

            try
            {
                int id = _auth.GetCurrentUserId();
                List<GpaCalc> list = _service.GetAll(id);
                response = new ItemsResponse<GpaCalc> { Items = list };

            }catch(Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);

        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse>DeleteCalc(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteCalc(id);
                response = new SuccessResponse();

            }
            catch(Exception ex) 
            { 
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

    }
}
