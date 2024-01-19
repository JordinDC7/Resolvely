using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Events;
using Sabio.Models.Requests.Events;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Data.SqlClient;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventApiController : BaseApiController
    {
        private IEventService _service = null;
        private IAuthenticationService<int> _authService = null;

        public EventApiController(IEventService service
            , ILogger<EventApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService; 
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id) 
        {
            int iCode = 200;
            BaseResponse response = null;
            try 
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex) 
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("createdBy")]
        public ActionResult<ItemResponse<Paged<Event>>> GetByCreatedBy( int pageIndex, int pageSize) 
        {
           
            int iCode = 200;
            BaseResponse response = null;
            Paged<Event> pagedEvent = null;

            try 
            {
                int userId = _authService.GetCurrentUserId();
                pagedEvent = _service.GetByCreatedBy( pageIndex, pageSize, userId);
                if (pagedEvent == null) 
                {
                    iCode = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else 
                {
                    response = new ItemResponse<Paged<Event>> { Item = pagedEvent };
                }
            }
            catch(Exception ex) 
            {
                iCode=500;
                response = new ErrorResponse($"Exception Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Event>> Get(int id) 
        {
            int iCode = 200;
            BaseResponse response = null;

            try 
            {
                Event aEvent = _service.GetById(id);
                if(aEvent == null) 
                {
                    iCode = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else 
                {
                    response = new ItemResponse<Event> { Item = aEvent };
                }
            }
            catch(Exception ex) 
            {
                iCode = 500;
                response = new ErrorResponse($"Exception Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<Event>>> GetAll(int pageIndex, int pageSize) 
        {
            int iCode = 200;
            BaseResponse response = null;
            Paged<Event> page = null;

            try 
            {
                 page = _service.GetAll(pageIndex, pageSize);
                
                if(page == null) 
                {
                    iCode = 404;
                    response = new ErrorResponse("App resource not found");
                }
                else 
                {
                    response = new ItemResponse<Paged<Event>> { Item = page };
                }
            }
            catch (Exception ex) 
            {
                iCode=500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());    
            }
            return StatusCode(iCode, response); 
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Event>>> GetByQuery(int pageIndex, int pageSize, string query)
        {
            int iCode = 200;
            BaseResponse response = null;
            Paged<Event> page = null;

            try
            {
                page = _service.GetByQuery(pageIndex, pageSize, query);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<Event>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(EventAddRequest request) 
        {                      
            ObjectResult result = null;

            try 
            {

                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(request, userId);

               ItemResponse<int> response = new ItemResponse<int> { Item = id };
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

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(EventUpdateRequest model) 
        {
            int iCode = 200;
            BaseResponse response = null;

            try 
            {
                int userId = _authService.GetCurrentUserId();
                
                _service.Update(model, userId);
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
