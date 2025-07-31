using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Core.Models;

namespace WarehouseManagement.Framework;

[ApiController]
[Route("api/[controller]")]
public abstract class ApplicationController : ControllerBase
{
    public override OkObjectResult Ok(object? value)
    {
        var envelope = Envelope<object>.Ok(value);

        return new OkObjectResult(envelope);
    }
}