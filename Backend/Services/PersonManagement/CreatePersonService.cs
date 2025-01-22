using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.PersonManagement
{
    public class CreatePersonService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<CreatePersonService> logger,
        ITransactionScope transactionScope) : BaseService<PersonDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreatePersonService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["FirstName", "LastName", "Email"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(PersonDTO personDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(personDto);

                var exitsPerson = await _context.Persons
                    .FirstOrDefaultAsync(p => 
                        p.FirstName == personDto.FirstName &&
                        p.LastName == personDto.LastName &&
                        p.Email == personDto.Email 
                     );

                if (exitsPerson != null)
                {
                    return ResultNotifier.Failure("Person already Exist!");
                }

                if (!string.IsNullOrEmpty(personDto.Status))
                {
                    if (!CommonTags.PersonStatuses.Contains(personDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.PersonStatuses)}");
                    }
                }
                else
                {
                    personDto.Status = CommonTags.Active;
                }

                var person = _mapper.Map<Person>(personDto);                
                person.InsertDate = DateTime.UtcNow;
                person.UpdateDate = DateTime.UtcNow;

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var createdPersonDto = _mapper.Map<PersonDTO>(person);
                return ResultNotifier.Success(createdPersonDto);
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
                await _transactionScope.RollbackAsync();
                _logger.LogError(ex, "Error creating person");
                return ResultNotifier.Failure("Error creating person");
            }
        }
    }
}
