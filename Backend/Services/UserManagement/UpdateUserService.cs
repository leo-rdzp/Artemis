using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Authentication;
using Artemis.Backend.IServices.Base;
using Artemis.Backend.Services.PersonManagement;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.UserManagement
{
    public class UpdateUserService(
    ArtemisDbContext context,
    IMapper mapper,
    ILogger<UpdateUserService> logger,
    IPasswordHasher passwordHasher,
    ITransactionScope transactionScope,
    ItemListTools itemListTools,
    UpdatePersonService updatePersonService) : BaseService<UserDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateUserService> _logger = logger;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITransactionScope _transactionScope = transactionScope;
        private readonly ItemListTools _itemListTools = itemListTools;
        private readonly UpdatePersonService _updatePersonService = updatePersonService;

        public override void PrepareMandatoryParameters()
        {
            // Only Id and UserName are mandatory
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

        public override async Task<ResultNotifier> ExecuteAsync(UserDTO userDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(userDto);

                if (userDto.Id < 1)
                {
                    return ResultNotifier.Failure("Valid User Id is Required!");
                }

                var user = await _context.Users
                    .Include(u => u.Person)
                    .Include(u => u.UserRoles!)
                    .Include(u => u.Properties!)
                    .FirstOrDefaultAsync(u => u.Id == userDto.Id);

                if (user == null)
                {
                    return ResultNotifier.Failure("User not found");
                }

                var paramList = _itemListTools.ObjectToItemList(userDto);
                
                if (user.UserName != userDto.UserName &&
                    _context.Users.Any(u => u.UserName == userDto.UserName))
                {
                    return ResultNotifier.Failure("Username already exists");
                }
                else if (!string.IsNullOrEmpty(userDto.UserName) && user.UserName != userDto.UserName)
                {
                    user.UserName = userDto.UserName;
                }

                // Optional updates - only apply if provided in the DTO
                if (!string.IsNullOrEmpty(userDto.Password))
                {
                    user.Password = _passwordHasher.HashPassword(userDto.Password);
                }

                if (!string.IsNullOrEmpty(userDto.Type) && user.Type != userDto.Type)
                {
                    if (!CommonTags.UserTypes.Contains(userDto.Type))
                    {
                        return ResultNotifier.Failure($"Invalid Type. Valid values are: {string.Join(", ", CommonTags.UserTypes)}");
                    }
                    else
                    {
                        user.Type = userDto.Type;
                    }
                }

                // Check if RequireMFA was included in the request                
                if (paramList.ContainsKey("RequireMFA"))
                {
                    user.RequireMFA = userDto.RequireMFA;
                }                

                if (!string.IsNullOrEmpty(userDto.Status) && user.Status != userDto.Status)
                {
                    if (!CommonTags.UserStatuses.Contains(userDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.UserStatuses)}");
                    }
                    else
                    {
                        user.Status = userDto.Status;
                    }
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

                    user.PasswordExpirationDate = userDto.PasswordExpirationDate.Value;
                }

                user.UpdateDate = DateTime.UtcNow;

                // Check if PersonId was included and is valid
                if (paramList.ContainsKey("PersonId") && userDto.PersonId > 0)
                {
                    _updatePersonService.SetAutoCommit(false);
                    var result = await _updatePersonService.ExecuteAsync(userDto.Person);

                    if (result.ResultStatus.IsFailed)
                    {
                        return result;
                    }

                    // Update properties instead of reassigning
                    if (result.ResultData != null)
                    {
                        var updatedPerson = (PersonDTO)result.ResultData;
                        user.Person.FirstName = updatedPerson.FirstName;
                        user.Person.LastName = updatedPerson.LastName;
                        user.Person.Email = updatedPerson.Email ?? "";
                        user.Person.Phone = updatedPerson.Phone ?? "";
                        user.Person.Status = updatedPerson.Status;
                        user.Person.UpdateDate = updatedPerson.UpdateDate;
                    }
                }

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var updatedUserDto = _mapper.Map<UserDTO>(user);
                return ResultNotifier.Success(updatedUserDto);
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
                _logger.LogError(ex, "Error updating user");
                return ResultNotifier.Failure("Error updating user");
            }
        }
    }
}
