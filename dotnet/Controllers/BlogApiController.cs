using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Sabio.Models;
using Sabio.Web.StartUp;
using System.Drawing.Printing;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain;
using Sabio.Models.Requests;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogApiController : BaseApiController
    {
        private IBlogService _service = null;
        private IAuthenticationService<int> _authService = null;
        public BlogApiController(IBlogService service,
                                    ILogger<BlogApiController> logger,
                                    IAuthenticationService<int> authService) : base(logger)
        {

            _service = service;
            _authService = authService;
        }

        [HttpGet("{id:int}")]
        public ActionResult GetBlog(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Blog blog = _service.GetBlog(id);

                if (blog == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemResponse<Blog> { Item = blog };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                return base.StatusCode(500, new ErrorResponse($"SqlException Errors: ${ex.Message}"));
            }
            return StatusCode(code, response);

        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Blog>>> GetAllBlogsPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> page = _service.GetAllBlogsPaginated(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not found");

                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("category/{id:int}")]
        public ActionResult<ItemResponse<Paged<Blog>>>GetBlogByCategoryPaginated(int id, int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> blog = _service.GetBlogCategory(pageIndex, pageSize, id);

                if (blog == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");

                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = blog };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                return base.StatusCode(500, new ErrorResponse($"SqlException Errors: ${ex.Message}"));
            }
            return StatusCode(code, response);

        }

        [HttpGet("author")]
        public ActionResult<ItemResponse<Paged<Blog>>> GetAuthorPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                Paged<Blog> page = _service.GetBlogAuthorsPaginated(pageIndex, pageSize, userId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not found");

                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult AddBlogs(BlogAddRequest model)
        {
            ObjectResult result = null;
            try
            {

                int userId = _authService.GetCurrentUserId();

                int id = _service.AddBlog(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                return Created201(response);
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
        public ActionResult<SuccessResponse> BlogUpdate(BlogUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
                response = new SuccessResponse();
                return Ok(response);
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteBlogs(int id)
        {
            int code = 200;
            BaseResponse response = null; //do not declare an instance

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
                return Ok(response);
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
