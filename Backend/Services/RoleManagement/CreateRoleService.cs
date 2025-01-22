using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Models.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.RoleManagement
{
    public class CreateRoleService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<CreateRoleService> logger,
        ITransactionScope transactionScope) : BaseService<RoleDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateRoleService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["Name", "Description", "BusinessId"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(RoleDTO roleDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(roleDto);

                if (_context.Roles.Any(r => r.Name == roleDto.Name))
                {
                    return ResultNotifier.Failure("Role name already exists");
                }

                // Validate Business exists and is active
                var business = await _context.Business
                    .FirstOrDefaultAsync(b => b.Id == roleDto.BusinessId && b.Status == CommonTags.Active);

                if (business == null)
                {
                    return ResultNotifier.Failure($"Business with ID {roleDto.BusinessId} not found or is inactive");
                }

                if (!string.IsNullOrEmpty(roleDto.Status))
                {
                    if (!CommonTags.RoleStatuses.Contains(roleDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.RoleStatuses)}");
                    }
                }
                else
                {
                    roleDto.Status = CommonTags.Active;
                }

                //Create the new  instance of role!
                var role = _mapper.Map<Role>(roleDto);
                //assign the business queried
                role.Business = business;

                _context.Roles.Add(role);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var createdRoleDto = _mapper.Map<RoleDTO>(role);
                return ResultNotifier.Success(createdRoleDto);
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
                _logger.LogError(ex, "Error creating role");
                return ResultNotifier.Failure("Error creating role");
            }
        }
    }
}
