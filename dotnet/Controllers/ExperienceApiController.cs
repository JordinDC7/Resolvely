using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models.Domain.Experience;
using Sabio.Models;
using Sabio.Models.Requests.Locations;
using Sabio.Models.Requests.Experience;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/experiences")]
    [ApiController]
    public class ExperienceApiController : BaseApiController
    {
        private IExperienceService _service = null;
        private IAuthenticationService<int> _authService = null;
        public ExperienceApiController
            (IExperienceService service
            , ILogger<ExperienceApiController> logger
            , IAuthenticationService<int> authService)
            : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<SuccessResponse> Create(List<ExperienceAddRequest> model)
        {
            int code = 201;
            BaseResponse response = null;
            

            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                _service.Add(model, currentUserId);
                response = new SuccessResponse();

                
            }
            catch (Exception ex)
            {
                
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                code = 500;
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(ExperienceUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                _service.Update(model, currentUserId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
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
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Experience>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Experience experience = _service.GetById(id);

                if (experience == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Experience>() { Item = experience };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }


        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Experience>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<Experience> pagedList = _service.GetAll(pageIndex, pageSize);
                if (pagedList == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Experience>> response = new ItemResponse<Paged<Experience>>();
                    response.Item = pagedList;
                    result = Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }


    }
}
