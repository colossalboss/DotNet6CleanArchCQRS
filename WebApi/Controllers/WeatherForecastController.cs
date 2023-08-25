using Application.Pdfs.Queries;
using Application.Person.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Route("list")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var query = new GetPersonById { Id = 2 };
        var result = await _mediator.Send(query);
        var transformedResult = TypedResults.Ok(result);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet(Name = "GeneratePdf")]
    [Route("testpdf")]
    public async Task<IActionResult> GenerateSamplePdf()
    {
        var htmlPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sample.html");

        var query = new GenerateSmaplePdf { FilePath = htmlPath };
        var stream = await _mediator.Send(query);

        byte[] response = stream.ToArray();
        return File(response, "application/pdf", "samplepdf.pdf");
    }
}

