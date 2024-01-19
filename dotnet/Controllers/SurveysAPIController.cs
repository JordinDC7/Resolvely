using EllipticCurve;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveys")]
    [ApiController]
    public class SurveysAPIController : BaseApiController
    {
        private ISurveysService _service = null;
        private IAuthenticationService<int> _authService = null;

        public SurveysAPIController(ISurveysService service, ILogger<SurveysAPIController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;

        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(SurveyAddRequest model)
        {
            ObjectResult result = null;
           
            try
            {
              int  userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(SurveyUpdateRequest model, int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
              
                _service.Update(model, id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("paginate/createdBy")]
        public ActionResult<ItemResponse<Paged<Survey>>> GetCreatedByPaginated(int pageIndex, int pageSize)
        {


            int code = 200;
            BaseResponse response = null;

            try
            {
               
                int createdBy = _authService.GetCurrentUserId();

                Paged<Survey> page = _service.GetAllByCreatedByPaginated(pageIndex, pageSize, createdBy);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Survey>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }
        
        [HttpGet("paginate/allSurveys")]
        public ActionResult<ItemResponse<Paged<Survey>>> GetSurveysByPaginated(int pageIndex, int pageSize, int statusId, bool excluded)
        {
            


            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Survey> page = _service.GetAllPaginated(pageIndex, pageSize, statusId, excluded);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Survey>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);

        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Survey>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Survey survey = _service.GetById(id);

                if (survey == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Survey> { Item = survey };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            
            try
            {
                _service.DeleteById(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}
