using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models.AppSettings;
using Sabio.Models.Requests.Email;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailApiController : BaseApiController
    {
        IEmailService _service = null;
        private AppKeys _appKeys;

        public EmailApiController(IEmailService service, IOptions<AppKeys>appKeys, ILogger<BaseApiController> logger) : base(logger)
        {
            _service = service;
            _appKeys = appKeys.Value;
        }


        [AllowAnonymous]
        [HttpPost("contact")]
        public ActionResult<SuccessResponse> SendContactEmail(ContactUsRequest model)
        {
            int code = 201;
            BaseResponse response = null;
            try
            {
                _service.ContactUs(model);
                response= new SuccessResponse();
    
            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
                code = 500;
            }


            return StatusCode(code, response);
        }



    }
}
