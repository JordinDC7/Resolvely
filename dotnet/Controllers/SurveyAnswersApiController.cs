using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Requests;
using Sabio.Models;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using Sabio.Models.Domain.Surveys;
using Sabio.Services.Services;

namespace Sabio.Web.Api.Controllers
{

    [Route("api/surveyanswers")]
    [ApiController]
    public class SurveyAnswersApiController : BaseApiController
    {

        private ISurveyAnswersService _service = null;
        private IAuthenticationService<int> _authService = null;

        public SurveyAnswersApiController(ISurveyAnswersService service,
           ILogger<PingApiController> logger,
           IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }



        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(SurveyAnswersAddRequest model)
        {
            int code = 201;
            BaseResponse response = null;

            try
            {
             
                int id = _service.Add(model);

                response = new ItemResponse<int>() { Item = id };


            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        //[HttpGet]
        //public ActionResult<ItemResponse<Paged<SurveyAnswers>>> GetAll(int pageIndex, int pageSize)
        //{
        //    int code = 200;
        //    BaseResponse response = null;

        //    try
        //    {
        //        Paged<SurveyAnswers> page = _service.SelectAllPaginated(pageIndex, pageSize);

        //        if (page == null)
        //        {
        //            code = 404;
        //            response = new ErrorResponse("App resource not found");
        //        }
        //        else
        //        {
        //            response = new ItemResponse<Paged<SurveyAnswers>> { Item = page };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        code = 500;
        //        response = new ErrorResponse(ex.Message);
        //        base.Logger.LogError(ex.ToString());
        //    }
        //    return StatusCode(code, response);
        //}



        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(SurveyAnswersUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
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



        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<SurveyAnswers>> Get(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                SurveyAnswers surveyAnswers = _service.Get(id);

                if (surveyAnswers == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource Not Found.");
                }
                else
                {
                    response = new ItemResponse<SurveyAnswers> { Item = surveyAnswers };
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

        [HttpGet("answers/{id:int}")]
        public ActionResult<ItemsResponse<List<SurveyAnswers>>> GetSurveyAnswersInstanceId(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<SurveyResult> surveyResult = _service.GetSurveyAnswersInstanceId(id);

                if (surveyResult == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource Not Found.");
                }
                else
                {
                    response = new ItemsResponse<SurveyResult> { Items = surveyResult };
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

        [HttpGet("allsurveys")]
        public ActionResult<ItemsResponse<List<SurveyAnswers>>> GetAllSurveys()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Survey> survey = _service.GetAllSurveys();

                if (survey == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application Resource Not Found.");
                }
                else
                {
                    response = new ItemsResponse<Survey> { Items = survey };
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


    }

}

