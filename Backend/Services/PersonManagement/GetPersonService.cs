using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.PersonManagement
{
    public class GetPersonService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<GetPersonService> logger) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetPersonService> _logger = logger;

        public override void PrepareMandatoryParameters()
        {
        }

        public override async Task<ResultNotifier> ExecuteAsync(int personId)
        {
            try
            {
                var person = await _context.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == personId);

                if (person == null)
                {
                    return ResultNotifier.Failure("Person not found");
                }

                var personDto = _mapper.Map<PersonDTO>(person);
                return ResultNotifier.Success(personDto);
            }
            catch (ArtemisException ex)
            {
                _logger.LogError(
                    "Artemis Exception occurred. Message: {Message}, Details: {Details}",
                    ex.Message,
                    ex.DetailedMessage);
                return ResultNotifier.Failure($"{ex.Message} - {ex.DetailedMessage}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving person {personId}", personId);
                return ResultNotifier.Failure("Error retrieving person");
            }
        }
    }
}
