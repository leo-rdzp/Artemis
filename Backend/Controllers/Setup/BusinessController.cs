using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Core.DTO.Setup;
using Artemis.Backend.Core.Models.Setup;
using Artemis.Backend.Services.BusinessManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers.Setup
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BusinessController(
        GetBusinessesService getBusinessesService,
        GetBusinessService getBusinessService,
        CreateBusinessService createBusinessService,
        UpdateBusinessService updateBusinessService,
        DeleteBusinessService deleteBusinessService,
        ILogger<BusinessController> logger) : ControllerBase
    {
        private readonly GetBusinessesService _getBusinessesService = getBusinessesService;
        private readonly GetBusinessService _getBusinessService = getBusinessService;
        private readonly CreateBusinessService _createBusinessService = createBusinessService;
        private readonly UpdateBusinessService _updateBusinessService = updateBusinessService;
        private readonly DeleteBusinessService _deleteBusinessService = deleteBusinessService;
        private readonly ILogger<BusinessController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetBusinesses([FromQuery] FilterDTO filter)
        {
            try
            {
                var result = await _getBusinessesService.ExecuteAsync(filter);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting businesses");
                return StatusCode(500, "An error occurred while retrieving businesses");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusiness(int id)
        {
            try
            {
                var result = await _getBusinessService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting business {businessId}", id);
                return StatusCode(500, "An error occurred while retrieving the business");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessDTO business)
        {
            try
            {
                var result = await _createBusinessService.ExecuteAsync(business);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating business");
                return StatusCode(500, "An error occurred while creating the business");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBusiness([FromBody] BusinessDTO business)
        {
            try
            {
                var result = await _updateBusinessService.ExecuteAsync(business);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating business {businessName}", business.Name);
                return StatusCode(500, "An error occurred while updating the business");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(int id)
        {
            try
            {
                var result = await _deleteBusinessService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultStatus.Message);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting business {businessId}", id);
                return StatusCode(500, "An error occurred while deleting the business");
            }
        }
    }
}
