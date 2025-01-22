using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Services.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers.IdentityManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(
        GetUsersService getUsersService,
        GetUserService getUserService,
        CreateUserService createUserService,
        UpdateUserService updateUserService,
        DeleteUserService deleteUserService,
        ILogger<UserController> logger) : ControllerBase
    {
        private readonly GetUsersService _getUsersService = getUsersService;
        private readonly GetUserService _getUserService = getUserService;
        private readonly CreateUserService _createUserService = createUserService;
        private readonly UpdateUserService _updateUserService = updateUserService;
        private readonly DeleteUserService _deleteUserService = deleteUserService;
        private readonly ILogger<UserController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] FilterDTO filter)
        {
            try
            {
                var result = await _getUsersService.ExecuteAsync(filter);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, "An error occurred while retrieving users");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var result = await _getUserService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {userId}", id);
                return StatusCode(500, "An error occurred while retrieving the user");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
        {
            try
            {
                var result = await _createUserService.ExecuteAsync(user);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "An error occurred while creating the user");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO user)
        {
            try
            {
                var result = await _updateUserService.ExecuteAsync(user);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {userName}", user.UserName);
                return StatusCode(500, "An error occurred while updating the user");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _deleteUserService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultStatus.Message);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {userId}", id);
                return StatusCode(500, "An error occurred while deleting the user");
            }
        }
    }
}