using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Authentication;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.UserManagement
{
    public class CreateUserService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<CreateUserService> logger,
        IPasswordHasher passwordHasher,
        ITransactionScope transactionScope) : BaseService<UserDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateUserService> _logger = logger;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            // Username, Password and PersonId are mandatory for creation
            SetMandatoryParameters(["UserName", "Password", "PersonId"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(UserDTO userDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(userDto);

                if (_context.Users.Any(u => u.UserName == userDto.UserName))
                {
                    return ResultNotifier.Failure("Username already exists");
                }

                // Validate Person exists and is active
                var person = await _context.Persons
                    .FirstOrDefaultAsync(p => p.Id == userDto.PersonId);

                if (person != null && person.Status != CommonTags.Active)
                {
                    return ResultNotifier.Failure($"Person with ID {userDto.PersonId} is not Active");
                }

                if (person == null && userDto.Person == null)
                {                    
                    return ResultNotifier.Failure($"Can not create Person, Person Object is null");
                }

                //Create Person
                person ??= new Person
                {
                    Id = 0, // Will be set by DB on insert
                    FirstName = userDto.Person.FirstName,
                    LastName = userDto.Person.LastName,
                    Email = userDto.Person.Email ?? "",
                    Phone = userDto.Person.Phone ?? "",
                    Status = CommonTags.Active,
                    InsertDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow
                };

                // Hash the password
                userDto.Password = _passwordHasher.HashPassword(userDto.Password);

                // Set Type if provided
                if (!string.IsNullOrEmpty(userDto.Type))
                {
                    if (!CommonTags.UserTypes.Contains(userDto.Type))
                    {
                        return ResultNotifier.Failure($"Invalid Type. Valid values are: {string.Join(", ", CommonTags.UserTypes)}");
                    }
                }
                else
                {
                    userDto.Type = CommonTags.Internal;
                }

                //Status validation
                if (!string.IsNullOrEmpty(userDto.Status))
                {
                    if (!CommonTags.UserStatuses.Contains(userDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.UserStatuses)}");
                    }
                }
                else
                {
                    userDto.Status = CommonTags.Active;
                }

                if (userDto.PasswordExpirationDate.HasValue)
                {
                    // Validate it's not in the past
                    if (userDto.PasswordExpirationDate.Value < DateTime.UtcNow)
                    {
                        return ResultNotifier.Failure("Password expiration date cannot be in the past");
                    }

                    // Optional: Validate it's not too far in the future (e.g., max 1 year)
                    if (userDto.PasswordExpirationDate.Value > DateTime.UtcNow.AddYears(1))
                    {
                        return ResultNotifier.Failure("Password expiration date cannot be more than one year in the future");
                    }
                }

                var user = _mapper.Map<User>(userDto);

                // Set RequireMFA
                user.RequireMFA = userDto.RequireMFA;

                //Save person !!
                await _context.Persons.AddAsync(person);
                await _context.SaveChangesAsync();

                //Set Person
                user.Person = person;

                // Set default and required values
                user.InsertDate = DateTime.UtcNow;
                user.UpdateDate = DateTime.UtcNow;                

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var createdUserDto = _mapper.Map<UserDTO>(user);
                return ResultNotifier.Success(createdUserDto);
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
                _logger.LogError(ex, "Error creating user");
                return ResultNotifier.Failure("Error creating user");
            }
        }
    }
}
