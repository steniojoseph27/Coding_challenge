using Microsoft.AspNetCore.Mvc;
using ClimateMonitor.Services;
using ClimateMonitor.Services.Models;
using System.Text.RegularExpressions;

namespace ClimateMonitor.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly DeviceSecretValidatorService _secretValidator;
    private readonly AlertService _alertService;

    public ReadingsController(
        DeviceSecretValidatorService secretValidator, 
        AlertService alertService)
    {
        _secretValidator = secretValidator;
        _alertService = alertService;
    }

    /// <summary>
    /// Evaluate a sensor readings from a device and return possible alerts.
    /// </summary>
    /// <remarks>
    /// The endpoint receives sensor readings (temperature, humidity) values
    /// as well as some extra metadata (firmwareVersion), evaluates the values
    /// and generate the possible alerts the values can raise.
    /// 
    /// There are old device out there, and if they get a firmwareVersion 
    /// format error they will request a firmware update to another service.
    /// </remarks>
    /// <param name="deviceSecret">A unique identifier on the device included in the header(x-device-shared-secret).</param>
    /// <param name="deviceReadingRequest">Sensor information and extra metadata from device.</param>
    [HttpPost("evaluate")]
    public IActionResult Evaluate([FromBody] DeviceReadingRequest deviceReadingRequest)
        {
            // Issue 1: validate secret header
            if (!Request.Headers.TryGetValue("x-device-shared-secret", out var deviceSecret) ||
                !_secretValidator.ValidateDeviceSecret(deviceSecret!))
            {
                return Problem(
                detail: "Device secret is not within the valid range.",
                statusCode: StatusCodes.Status401Unauthorized);
            }

            
    }
}
