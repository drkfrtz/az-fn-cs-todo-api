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
            this HttpRequestData req
        )
        {
            HttpResponseData res = req.CreateResponse();
            await res.WriteAsJsonAsync(
                new ApiError(resourceNotFoundMessage),
                HttpStatusCode.NotFound
            );
            return res;
        }

        public static async Task<HttpResponseData> CreateMalformedBodyResponse(
            this HttpRequestData req
        )
        {
            var errorResponse = req.CreateResponse();
            await errorResponse.WriteAsJsonAsync(
                new ApiError("Malformed request body."),
                HttpStatusCode.UnprocessableEntity
            );
            return errorResponse;
        }

        public static async Task<HttpResponseData> CreateOkResponse<T>(
            this HttpRequestData req,
            T body
        )
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(body);
            return response;
        }
    }
}
