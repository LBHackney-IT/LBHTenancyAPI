using System;
using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Newtonsoft.Json;
using WebResponse = AgreementService.WebResponse;

namespace LBHTenancyAPI.Infrastructure.API
{
    public class APIResponse
    {
        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("errors")]
        public IList<APIError> Errors { get; set; }

        public APIResponse(object data)
        {
            StatusCode = (int) HttpStatusCode.OK;
            Data = data;
        }
        public APIResponse(Exception ex)
        {
            StatusCode = (int) HttpStatusCode.InternalServerError;
            Errors =  new List<APIError> { new APIError(ex) };
        }

        public APIResponse(IResponse<object> response)
        {
            if (response == null)
                StatusCode = (int) HttpStatusCode.BadRequest;
            if(response.Errors != null)

        }

        //public APIResponse(WebResponse response)
        //{
        //    if (response == null)
        //    {
        //        StatusCode = 
        //        return;
        //    }

        //    IsSuccess = response.Success;
        //    Errors = response.Success
        //        ? null
        //        : new List<APIError>{new APIError(response)};
        //}
    }

    public class APIError
    {
        public string Message { get; set; }
        public string Code { get; set; }

        public APIError()
        {

        }

        public APIError(ValidationFailure validationFailure)
        {
            Message = validationFailure?.ErrorMessage;
            
        }

        public APIError(Exception ex)
        {
            Message = ex?.Message; 
        }

        public APIError(WebResponse response)
        {
            Code = $"UH_{response?.ErrorCode}";
            Message = response?.ErrorMessage;
        }
    }

    public class ValidationError
    {
        public string Message { get; set; }
        public string FieldName { get; set; }
    }

    public class APIErrorCode
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public abstract class ApiException:Exception
    {
        public HttpStatusCode StatusCode { get; protected set; }
    }

    public class BadRequestException : ApiException
    {
        public IList<APIError> Errors { get; set; }

        public BadRequestException()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequestException(IList<ValidationFailure> errors)
        {
            StatusCode = HttpStatusCode.BadRequest;
            Errors = new List<APIError>();
            foreach (var validationFailure in errors)
            {
                var error = new APIError(validationFailure);
                Errors.Add(error);
            }
        }
    }
}
