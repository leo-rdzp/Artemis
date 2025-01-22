using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Services.RoleManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers.IdentityManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController(
        GetRolesService getRolesService,
        GetRoleService getRoleService,
        CreateRoleService createRoleService,
        UpdateRoleService updateRoleService,
        DeleteRoleService deleteRoleService,
        ILogger<RoleController> logger) : ControllerBase
    {
        private readonly GetRolesService _getRolesService = getRolesService;
        private readonly GetRoleService _getRoleService = getRoleService;
        private readonly CreateRoleService _createRoleService = createRoleService;
        private readonly UpdateRoleService _updateRoleService = updateRoleService;
        private readonly DeleteRoleService _deleteRoleService = deleteRoleService;
        private readonly ILogger<RoleController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] FilterDTO filter)
        {
            try
            {
                var result = await _getRolesService.ExecuteAsync(filter);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles");
                return StatusCode(500, "An error occurred while retrieving roles");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            try
            {
                var result = await _getRoleService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role {roleId}", id);
                return StatusCode(500, "An error occurred while retrieving the role");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO role)
        {
            try
            {
                var result = await _createRoleService.ExecuteAsync(role);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, "An error occurred while creating the role");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDTO role)
        {
            try
            {
                var result = await _updateRoleService.ExecuteAsync(role);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role {roleName}", role.Name);
                return StatusCode(500, "An error occurred while updating the role");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _deleteRoleService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultStatus.Message);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role {roleId}", id);
                return StatusCode(500, "An error occurred while deleting the role");
            }
        }
    }
}
