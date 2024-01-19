using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models;
using Sabio.Models.AppSettings;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Files;
using Sabio.Models.Requests.Files;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Web.StartUp;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Sabio.Web.Api.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileApiController : BaseApiController
    {
        IFileService _file = null;
        private IAuthenticationService<int> _authService = null;
        
       
        public FileApiController(IFileService service, ILogger<FileApiController> logger, IAuthenticationService<int> authService) : base(logger)
        {
            _authService = authService;
            _file = service;
        }

        [HttpPost]
        public async Task<ActionResult> FileAdd(List<IFormFile> files)
        {
            ObjectResult result = null;

            try
            {

                int userId = _authService.GetCurrentUserId();

                Task<List<BaseFile>> listTask = _file.AddFile(files, userId);

                List<BaseFile> list = await listTask;

                ItemsResponse<BaseFile> response = new ItemsResponse<BaseFile>() { Items = list };

                result = StatusCode(201, response);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Files>>> FilePaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Files> paged = _file.FilePagination(pageIndex, pageSize);
                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Something went wrong");
                }
                else
                {
                    response = new ItemResponse<Paged<Files>>() { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Files>>> FileSearch(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                int userId = _authService.GetCurrentUserId();

                Paged<Files> paged = _file.FileSearchPagination(pageIndex, pageSize, userId);

                if (paged == null)
                {
                    code = 404;
                    response = new ErrorResponse("Something went wrong");
                }
                else
                {
                    response = new ItemResponse<Paged<Files>>() { Item = paged };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> FileDelete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                _file.FileDelete(id);
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
