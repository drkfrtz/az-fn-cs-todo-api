using Azure.Core.Serialization;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using TodoApi.Models.Dtos;

namespace TodoApi.Extensions
{
    public static class UriExtensions
    {
        private const string resourceNotFoundMessage = "Resource not found.";

        public static string GetBaseUrl(this Uri uri)
        {
            return $"{uri.Scheme}://{uri.Authority}{uri.AbsolutePath}";
        }

        public static async Task<HttpResponseData> CreateResourceNotFoundResponse(
            this HttpRequestData req,
            ObjectSerializer serializer
        )
        {
            HttpResponseData res = req.CreateResponse();
            await res.WriteAsJsonAsync(
                new ApiError(resourceNotFoundMessage),
                serializer,
                HttpStatusCode.NotFound
            );
            return res;
        }

        public static async Task<HttpResponseData> CreateMalformedBodyResponse(
            this HttpRequestData req,
            ObjectSerializer serializer
        )
        {
            var errorResponse = req.CreateResponse();
            await errorResponse.WriteAsJsonAsync(
                new ApiError("Malformed request body."),
                serializer,
                HttpStatusCode.UnprocessableEntity
            );
            return errorResponse;
        }

        public static async Task<HttpResponseData> CreateOkResponse<T>(
            this HttpRequestData req,
            T body,
            ObjectSerializer serializer
        )
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(body, serializer);
            return response;
        }
    }
}
