using Notification.API.Domain.Dto.Common;
using Notification.API.Helper.Resources;

namespace Notification.API.Helper
{
    public static class Utilities
    {

        //201
        public static ResponseModel SuccessResponseForAdd(object? data = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status201Created,
                Message = CommonMessage.SavedSuccessfully,
                Data = data
            };
        }

        //200
        public static ResponseModel SuccessResponseForUpdate(object? data = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = CommonMessage.UpdatedSuccessfully,
                Data = data
            };
        }

        //200
        public static ResponseModel SuccessResponseForDelete()
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = CommonMessage.DeletedSuccessfully,
                Data = null
            };
        }

        //200
        public static ResponseModel SuccessResponseForGet(object? data = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = null,
                Data = data
            };
        }

        //200
        public static ResponseModel SuccessResponseForMailSend()
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = CommonMessage.MailSendSuccessfully,
                Data = null
            };
        }

        //204
        public static ResponseModel NoContentResponse(object? data = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status204NoContent,
                Message = CommonMessage.NoContent,
                Data = data
            };
        }

        //400
        public static ResponseModel ValidationErrorResponse(string? message = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = message ?? CommonMessage.ValidationError,
                Data = null
            };
        }

        //400
        public static ResponseModel ValidationErrorResponse(List<string>? messageList)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = CommonMessage.ValidationError,
                Data = messageList
            };
        }

        //401
        public static ResponseModel UnAuthorizedResponse()
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = CommonMessage.UnAuthorized,
                Data = null
            };
        }

        //403
        public static ResponseModel ForbiddenResponse()
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status403Forbidden,
                Message = CommonMessage.ForbiddenResponse,
                Data = null
            };
        }

        //404
        public static ResponseModel NotFoundResponse()
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = CommonMessage.NotFound,
                Data = null
            };
        }

        //404
        public static ResponseModel NotFoundResponse(string message)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = message ?? CommonMessage.PNRNotFound,
                Data = null
            };
        }

        //500
        public static ResponseModel InternalServerErrorResponse(string? message = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = message ?? CommonMessage.InternalServerError,
                Data = null
            };
        }


        // 200
        public static ResponseModel SuccessResponse(string? message = null, object? data = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = message ?? CommonMessage.SuccessResponseMessage,
                Data = data
            };
        }

        //422 Unprocessable Entity
        public static ResponseModel FailedRequest(string? message = null, object? data = null)
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                Message = message ?? "Retry request failed",
                Data = data
            };
        }
    }
}
