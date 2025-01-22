using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Services.ApplicationManagement;
using Artemis.Backend.Services.RoleManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers.IdentityManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleAssignmentController(
    AssignUserRoleService assignUserRoleService,
    RemoveUserRoleService removeUserRoleService,
    AssignApplicationRoleService assignApplicationRoleService,
    RemoveApplicationRoleService removeApplicationRoleService,
    GetUserRolesService getUserRolesService,
    GetApplicationRolesService getApplicationRolesService,
    ILogger<RoleAssignmentController> logger) : ControllerBase
    {
        private readonly ILogger<RoleAssignmentController> _logger = logger;

        [HttpPost("user")]
        public async Task<IActionResult> AssignUserRole([FromBody] UserRoleDTO assignment)
        {
            try
            {
                var result = await assignUserRoleService.ExecuteAsync(assignment);
                return result.ResultStatus.IsPassed ? Ok(result.ResultData) : BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning user role");
                return StatusCode(500, "An error occurred while assigning the role");
            }
        }

        [HttpDelete("user/{userId}/{roleId}")]
        public async Task<IActionResult> RemoveUserRole(int userId, int roleId)
        {
            try
            {
                var assignment = new UserRoleDTO { UserId = userId, RoleId = roleId };
                var result = await removeUserRoleService.ExecuteAsync(assignment);
                return result.ResultStatus.IsPassed ? Ok(result.ResultStatus.Message) : BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing user role");
                return StatusCode(500, "An error occurred while removing the role");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            try
            {
                var result = await getUserRolesService.ExecuteAsync(userId);
                return result.ResultStatus.IsPassed ? Ok(result.ResultData) : NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user roles");
                return StatusCode(500, "An error occurred while retrieving roles");
            }
        }

        [HttpPost("application")]
        public async Task<IActionResult> AssignApplicationRole([FromBody] ApplicationRoleDTO assignment)
        {
            try
            {
                var result = await assignApplicationRoleService.ExecuteAsync(assignment);
                return result.ResultStatus.IsPassed ? Ok(result.ResultData) : BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning application role");
                return StatusCode(500, "An error occurred while assigning the role");
            }
        }

        [HttpDelete("application/{applicationId}/{roleId}")]
        public async Task<IActionResult> RemoveApplicationRole(int applicationId, int roleId)
        {
            try
            {
                var assignment = new ApplicationRoleDTO { ApplicationId = applicationId, RoleId = roleId };
                var result = await removeApplicationRoleService.ExecuteAsync(assignment);
                return result.ResultStatus.IsPassed ? Ok(result.ResultStatus.Message) : BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing application role");
                return StatusCode(500, "An error occurred while removing the role");
            }
        }

        [HttpGet("application/{applicationId}")]
        public async Task<IActionResult> GetApplicationRoles(int applicationId)
        {
            try
            {
                var result = await getApplicationRolesService.ExecuteAsync(applicationId);
                return result.ResultStatus.IsPassed ? Ok(result.ResultData) : NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting application roles");
                return StatusCode(500, "An error occurred while retrieving roles");
            }
        }
    }
}
