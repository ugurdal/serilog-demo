using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SerilogDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("WeatherForecast requested");
            var rng = new Random();
            var rngIndex = rng.Next(Summaries.Length);
            try
            {
                if (rngIndex % 2 == 0)
                    throw new Exception($"Demo exception, value{rngIndex}");

                return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rngIndex]
                    })
                    .ToArray());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Method:{Method}, value={Value}", nameof(Get), rngIndex);
                return BadRequest("");
            }
        }
    }
}