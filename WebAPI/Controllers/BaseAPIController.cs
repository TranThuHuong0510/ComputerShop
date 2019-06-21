using Common.Object;
using Common.Utility;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace WebAPI.Controllers
{
    public class BaseAPIController : ApiController
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public FormattedContentResult<ObjectResult> InternalServerErrorResult()
        {
            var resultObject = new ObjectResult()
            {
                Message = Constants.Message.InternalServerError,
                Result = null,
                StatusCode = 500
            };
            return Content(HttpStatusCode.BadRequest, resultObject, ApiUtils.MediaTypeFormatterJson);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public FormattedContentResult<ObjectResult> DataValidationError(string message)
        {
            var resultObject = new ObjectResult()
            {
                Message = message,
                Result = null,
                StatusCode = 400
            };
            return Content(HttpStatusCode.BadRequest, resultObject, ApiUtils.MediaTypeFormatterJson);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public FormattedContentResult<ObjectResult> NotFoundErrorResult()
        {
            var resultObject = new ObjectResult()
            {
                Message = Constants.Message.NotFound,
                Result = null,
                StatusCode = 404
            };
            return Content(HttpStatusCode.NotFound, resultObject, ApiUtils.MediaTypeFormatterJson);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public FormattedContentResult<ObjectResult> ForbiddenErrorResult()
        {
            var resultObject = new ObjectResult()
            {
                Message = Constants.Message.Forbidden,
                Result = null,
                StatusCode = 403
            };
            return Content(HttpStatusCode.Forbidden, resultObject, ApiUtils.MediaTypeFormatterJson);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        public FormattedContentResult<ObjectResult> AuthorizationError()
        {
            var resultObject = new ObjectResult()
            {
                Message = "", // TODO
                Result = null,
                StatusCode = 401
            };
            return Content(HttpStatusCode.BadRequest, resultObject, ApiUtils.MediaTypeFormatterJson);
        }
    }
}