using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Web.StartUp;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using Sabio.Models.Domain.Podcasts;
using Sabio.Services.Interfaces;
using Sabio.Models.Requests;
using SendGrid;
using System.Linq;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/podcasts")]
    [ApiController]
    public class PodcastApiController : BaseApiController
    {
        private IPodcastService _podcast = null;
        private IAuthenticationService<int> _authService = null;
        public PodcastApiController(IPodcastService service
        , ILogger<PodcastApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _podcast = service;
            _authService = authService;
        }

        [HttpGet("select/paginate")]
        public ActionResult<ItemResponse<Paged<Podcasts>>> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Podcasts> paged = _podcast.SelectByCreatedBy(pageIndex,pageSize, createdBy); 
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Something went wrong");
                }
                else
                {
                    response = new ItemResponse<Paged<Podcasts>>() { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(PodcastUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _podcast.Update(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> AddPodcast(PodcastAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _podcast.AddPodcast(model, userId);
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

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Podcasts>>> GetPodcastsBySearch(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Podcasts> paged = _podcast.SearchPagination(pageIndex, pageSize, query);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Podcasts>>{Item= paged};                   
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

        [HttpDelete("{id:int}")]

        public ActionResult<SuccessResponse> PodcastsDelete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _podcast.PodcastsDelete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Podcasts>>> GetPodcastsByPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<Podcasts> paged = _podcast.SelectAllPagination(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Podcasts>> { Item = paged };
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

