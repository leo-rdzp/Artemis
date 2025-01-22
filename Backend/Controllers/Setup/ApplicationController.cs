using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Services.ApplicationManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers.Setup
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationController(
        GetApplicationsService getApplicationsService,
        GetApplicationService getApplicationService,
        CreateApplicationService createApplicationService,
        UpdateApplicationService updateApplicationService,
        DeleteApplicationService deleteApplicationService,
        ILogger<ApplicationController> logger) : ControllerBase
    {
        private readonly GetApplicationsService _getApplicationsService = getApplicationsService;
        private readonly GetApplicationService _getApplicationService = getApplicationService;
        private readonly CreateApplicationService _createApplicationService = createApplicationService;
        private readonly UpdateApplicationService _updateApplicationService = updateApplicationService;
        private readonly DeleteApplicationService _deleteApplicationService = deleteApplicationService;
        private readonly ILogger<ApplicationController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetApplications([FromQuery] FilterDTO filter)
        {
            try
            {
                var result = await _getApplicationsService.ExecuteAsync(filter);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting applications");
                return StatusCode(500, "An error occurred while retrieving applications");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(int id)
        {
            try
            {
                var result = await _getApplicationService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application {applicationId}", id);
                return StatusCode(500, "An error occurred while retrieving the application");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] ApplicationDTO application)
        {
            try
            {
                var result = await _createApplicationService.ExecuteAsync(application);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating application");
                return StatusCode(500, "An error occurred while creating the application");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApplication([FromBody] ApplicationDTO application)
        {
            try
            {
                var result = await _updateApplicationService.ExecuteAsync(application);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application {applicationId}", application.Id);
                return StatusCode(500, "An error occurred while updating the application");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            try
            {
                var result = await _deleteApplicationService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultStatus.Message);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting application {applicationId}", id);
                return StatusCode(500, "An error occurred while deleting the application");
            }
        }
    }
}