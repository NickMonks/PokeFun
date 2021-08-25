using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pokemon_API.Exceptions
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = new ExceptionPokemon(error.Message);

                switch (error)
                {
                    case BadHttpRequestException:

                        responseModel.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                        break;

                    case PokemonNotFoundException:
                        
                        responseModel.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    
                    case PokemonTranslateAPIExhaustion:
                        
                        responseModel.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        break;
                    
                    default:
                        //  if no exception is found, will deem it as a server error
                        responseModel.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}
