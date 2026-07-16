using API.Services.Models;
using API.Services.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace API.Triggers
{
    public class LMSValue
    {
        private readonly ILogger<LMSValue> _logger;
        private readonly LMSService _lmsService;

        public LMSValue(
            ILogger<LMSValue> logger,
            LMSService lmsService)
        {
            _logger = logger;
            _lmsService = lmsService;
        }

        [Function("GetLMSValue")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var child = await JsonSerializer.DeserializeAsync<Beneficiary>(
                    req.Body,
                    options);

                if (child == null)
                {
                    var badRequest = req.CreateResponse(HttpStatusCode.BadRequest);

                    await badRequest.WriteAsJsonAsync(new
                    {
                        Message = "Invalid request body."
                    });

                    return badRequest;
                }

                // Currently requesting WLZ.
                // Later this can come from the request body if required.
                var lms = await _lmsService.GetLMSValues(child, child.type);

                var response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(lms);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching LMS values.");

                var error = req.CreateResponse(HttpStatusCode.InternalServerError);

                await error.WriteAsJsonAsync(new
                {
                    Message = ex.Message
                });

                return error;
            }
        }
    }
}