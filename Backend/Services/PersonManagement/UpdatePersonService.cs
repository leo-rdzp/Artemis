using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;

namespace Artemis.Backend.Services.PersonManagement
{
    public class UpdatePersonService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<UpdatePersonService> logger,
        ITransactionScope transactionScope) : BaseService<PersonDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdatePersonService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["Id"]);
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

                if (personDto.Id < 1)
                {
                    return ResultNotifier.Failure("Valid Person Id is Required!");
                }

                var person = await _context.Persons
                    .FirstOrDefaultAsync(p => p.Id == personDto.Id);

                if (person == null)
                {
                    return ResultNotifier.Failure("Person not found");
                }

                // First check if any of the key fields are being changed
                bool isNameOrEmailChanging =
                    !string.Equals(person.FirstName, personDto.FirstName, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(person.LastName, personDto.LastName, StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(person.Email, personDto.Email, StringComparison.OrdinalIgnoreCase);

                // Only check for duplicates if the identifying information is changing
                if (isNameOrEmailChanging)
                {
                    bool duplicateExists = await _context.Persons
                        .AsNoTracking()
                        .Where(p => p.Id != person.Id && p.Status != CommonTags.Deleted)
                        .Where(p => p.FirstName.ToLower() == personDto.FirstName.ToLower() &&
                                    p.LastName.ToLower() == personDto.LastName.ToLower() &&
                                    p.Email.ToLower() == (personDto.Email != null ? personDto.Email.ToLower() : null))
                        .AnyAsync();

                    if (duplicateExists)
                    {
                        return ResultNotifier.Failure("A person with this name and email address already exists!");
                    }
                }

                // Update properties
                UpdatePersonProperties(person, personDto);

                if (!string.IsNullOrEmpty(personDto.Status) && person.Status != personDto.Status)
                {
                    if (!CommonTags.PersonStatuses.Contains(personDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.PersonStatuses)}");
                    }
                    else
                    {
                        person.Status = personDto.Status;
                    }
                }
                
                if (!string.IsNullOrEmpty(personDto.Phone) && person.Phone != personDto.Phone)
                {
                    person.Phone = personDto.Phone;
                }
                person.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var updatedPersonDto = _mapper.Map<PersonDTO>(person);
                return ResultNotifier.Success(updatedPersonDto);
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
                _logger.LogError(ex, "Error updating person");
                return ResultNotifier.Failure($"Error updating person. {ex.Message}");
            }
        }

        private static void UpdatePersonProperties(Person person, PersonDTO personDto)
        {
            // Trim whitespace and ensure proper casing for names
            if (!string.IsNullOrWhiteSpace(personDto.FirstName))
            {
                person.FirstName = CultureInfo.CurrentCulture.TextInfo
                    .ToTitleCase(personDto.FirstName.Trim());
            }

            if (!string.IsNullOrWhiteSpace(personDto.LastName))
            {
                person.LastName = CultureInfo.CurrentCulture.TextInfo
                    .ToTitleCase(personDto.LastName.Trim());
            }

            // Email should be lowercase for consistency
            if (!string.IsNullOrWhiteSpace(personDto.Email))
            {
                person.Email = personDto.Email.Trim().ToLowerInvariant();
            }
        }
    }
}
