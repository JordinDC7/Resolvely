using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.FAQs;
using Sabio.Models.Requests.FAQs;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/FAQs")]
    [ApiController]
    public class FAQsApiController : BaseApiController
    {
        private IFAQsService _FAQsService = null;
        private IAuthenticationService<int> _authenticationService = null;
        public FAQsApiController(IFAQsService service
            , ILogger<FAQsApiController> logger
            , IAuthenticationService<int> authenticationService) : base(logger)
        {
            _FAQsService = service;
            _authenticationService = authenticationService;
        }


        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                _FAQsService.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }


        [HttpPost]
        public ActionResult<ItemResponse<int>> Insert(FAQsAddRequest model)
        {
            ObjectResult result = null;
            try

            {
                int userId = _authenticationService.GetCurrentUserId();
                int id = _FAQsService.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ItemsResponse<FAQ>> GetAllFAQs()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                List<FAQ> list = _FAQsService.GetAllFAQs();
                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<FAQ> { Items = list };
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


        [HttpGet("{categoryId:int}")]
        public ActionResult<ItemsResponse<FAQ>> GetByCategoryId(int categoryId)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                List<FAQ> faqList = _FAQsService.GetByCategoryId(categoryId);
                if (faqList == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<FAQ> { Items = faqList };
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

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> UpdateFAQ(FAQUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                int currentUserId = _authenticationService.GetCurrentUserId();
                _FAQsService.UpdateFAQ(model, currentUserId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }
    }
}
