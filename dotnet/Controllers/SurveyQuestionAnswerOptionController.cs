using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Models.Requests;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models.Domain.SurveyQuestions;
using System.Collections.Generic;
using Sabio.Services.Services;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveyAnswerOptions")]
    [ApiController]
    public class SurveyQuestionAnswerOptionController : BaseApiController
    {
        private ISurveyQuestionAnswerOptionsService _service = null;
        private IAuthenticationService<int> _authService = null;

        public SurveyQuestionAnswerOptionController(ISurveyQuestionAnswerOptionsService service, ILogger<SurveysAPIController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;

        }

        [HttpGet("{id:int}")]
        public ActionResult GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<SurveyQuestion> list = _service.GetQuestionById(id);
                
                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Page Not Found");
                }
                else
                {
                    response = new ItemsResponse<SurveyQuestion> { Items = list };
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
