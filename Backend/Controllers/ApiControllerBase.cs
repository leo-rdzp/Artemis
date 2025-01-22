using Artemis.Backend.Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult HandleResult<T>(ResultNotifier result)
    {
        if (result.ResultStatus.IsPassed)
        {
            return result.ResultData == null ? Ok() : Ok(result.ResultData);
        }

        if (string.IsNullOrEmpty(result.ResultStatus.Message))
        {
            return BadRequest();
        }

        return BadRequest(result.ResultStatus.Message);
    }
}
