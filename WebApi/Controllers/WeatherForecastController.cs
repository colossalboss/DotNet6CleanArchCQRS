using Application.Jobs.TestJob;
using Application.Pdfs.Queries;
using Application.Person.Queries;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;

namespace WebApi.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;
    private readonly IBackgroundJobClient _backgrondJob;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator, IBackgroundJobClient backgroundJobClient)
    {
        _logger = logger;
        _mediator = mediator;
        _backgrondJob = backgroundJobClient;
    }

    /// <summary>
    /// Get list of weather forecasts
    /// </summary>
    /// <param name="page">The requested page</param>
    /// <param name="pageSize">The size of the page</param>
    /// <returns>An IActionResult</returns>
    /// <response code="200">Returns the list of forecasts</response>
    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(int? page, int? pageSize)
    {
        try
        {
            var query = new GetPersonById { Id = 2 };
            var result = await _mediator.Send(query);
            var transformedResult = TypedResults.Ok(result);

            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray());
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Exception thrown while getting weather forecasts", ex.Message);
            return StatusCode(500, "An error occurred");
        }
    }

    //[HttpGet(Name = "GeneratePdf")]
    [HttpGet("testpdf")]
    public async Task<IActionResult> GenerateSamplePdf()
    {
        var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sample.html");

        var query = new GenerateSmaplePdf { FilePath = htmlPath };
        var stream = await _mediator.Send(query);

        byte[] response = stream.ToArray();
        return File(response, "application/pdf", "samplepdf.pdf");
    }

    //[HttpGet(Name = "StartJob")]
    [HttpGet("runjob")]
    public ActionResult StartBackgroundJob()
    {
        _backgrondJob.Enqueue<GenerateNothingJob>(i => i.DoTheWork(1000));
        return Ok();
    }
}

