using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Services.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/surveyquestion")]
    [ApiController]
    public class SurveyQuestionController : BaseApiController
    {

        ISurveyQuestionsService _questionService = null;
        private IAuthenticationService<int> _authService = null;

        public SurveyQuestionController(ISurveyQuestionsService questionService, ILogger<FileApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _questionService = questionService;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult AddQuestion(QuestionAddRequest model)
        {
            BaseResponse result = null;
            int code = 201;
            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _questionService.AddQuestion(model, userId);

                result = new ItemResponse<int> { Item = id };

            }
            catch (Exception ex)
            {
                code = 500;
                result = new ErrorResponse(ex.Message);

            }
            return StatusCode(code, result);
        }

        [HttpGet("{id:int}")]
        public ActionResult GetSurveyById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<SurveyQuestion> list = _questionService.GetSurveyByIdWithQuestions(id);

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

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> UpdateQuestion(QuestionUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _questionService.UpdateQuestion(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Exception Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteQuestion(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _questionService.DeleteQuestion(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Exception Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }
    }
}
