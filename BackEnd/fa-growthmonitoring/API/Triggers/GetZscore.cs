using API.Services.Models;
using API.Services.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace API.Triggers
{
    public class Zscore
    {
        private readonly ILogger<Zscore> _logger;
        private readonly ZScoreService _zscoreService;

        public Zscore(
            ILogger<Zscore> logger,
            ZScoreService zscoreService)
        {
            _logger = logger;
            _zscoreService = zscoreService;
        }

        [Function("GetZscore")]
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
                _logger.LogInformation(
    "Received request for child details. Height: {Height}, Weight: {Weight}, Age: {Age}, Gender: {Gender}", child.height, child.weight, child.age, child.Gender);
                var zscoreResult = await _zscoreService.ProvideZscore(child);

                var response = req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(zscoreResult);

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