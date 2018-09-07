using System;
using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;
using LBHTenancyAPI.UseCases.ArrearsAgreements;
using Newtonsoft.Json;
using AgreementService;

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

        public APIResponse(IExecuteWrapper<object> executeWrapper)
        {
            if (executeWrapper == null)
            {
                StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }
                
            if (executeWrapper.Errors != null)
            {
                StatusCode = (int) HttpStatusCode.InternalServerError;
                Errors = executeWrapper.Errors;
            }

        }
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
