using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.DTO.Common;
using Artemis.Backend.Services.PersonManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artemis.Backend.Controllers.IdentityManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonController(
        GetPersonsService getPersonsService,
        GetPersonService getPersonService,
        CreatePersonService createPersonService,
        UpdatePersonService updatePersonService,
        DeletePersonService deletePersonService,
        ILogger<PersonController> logger) : ControllerBase
    {
        private readonly GetPersonsService _getPersonsService = getPersonsService;
        private readonly GetPersonService _getPersonService = getPersonService;
        private readonly CreatePersonService _createPersonService = createPersonService;
        private readonly UpdatePersonService _updatePersonService = updatePersonService;
        private readonly DeletePersonService _deletePersonService = deletePersonService;
        private readonly ILogger<PersonController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetPersons([FromQuery] FilterDTO filter)
        {
            try
            {
                var result = await _getPersonsService.ExecuteAsync(filter);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting persons");
                return StatusCode(500, "An error occurred while retrieving persons");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            try
            {
                var result = await _getPersonService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return NotFound(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting person {personId}", id);
                return StatusCode(500, "An error occurred while retrieving the person");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDTO person)
        {
            try
            {
                var result = await _createPersonService.ExecuteAsync(person);

                if (result.ResultStatus.IsPassed)
                {
                    return CreatedAtAction(nameof(GetPerson), new { id = (result.ResultData as PersonDTO)?.Id }, result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating person");
                return StatusCode(500, "An error occurred while creating the person");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson([FromBody] PersonDTO person)
        {
            try
            {
                var result = await _updatePersonService.ExecuteAsync(person);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultData);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating person {firstName} {lastName}", person.FirstName, person.LastName);
                return StatusCode(500, "An error occurred while updating the person");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            try
            {
                var result = await _deletePersonService.ExecuteAsync(id);

                if (result.ResultStatus.IsPassed)
                {
                    return Ok(result.ResultStatus.Message);
                }
                return BadRequest(result.ResultStatus.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting person {personId}", id);
                return StatusCode(500, "An error occurred while deleting the person");
            }
        }
    }
}
