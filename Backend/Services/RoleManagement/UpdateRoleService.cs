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
    public class UpdateRoleService(
        ArtemisDbContext context,
        IMapper mapper,
        ILogger<UpdateRoleService> logger,
        ITransactionScope transactionScope,
        ItemListTools itemListTools) : BaseService<RoleDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateRoleService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;
        private readonly ItemListTools _itemListTools = itemListTools;

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

        public override async Task<ResultNotifier> ExecuteAsync(RoleDTO roleDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(roleDto);

                if (roleDto.Id < 1)
                {
                    return ResultNotifier.Failure("Valid Role Id is Required!");
                }

                var role = await _context.Roles
                    .Include(r => r.Business)
                    .FirstOrDefaultAsync(r => r.Id == roleDto.Id);

                if (role == null)
                {
                    return ResultNotifier.Failure("Role not found");
                }

                if (role.Name != roleDto.Name &&
                    _context.Roles.Any(r => r.Name == roleDto.Name))
                {
                    return ResultNotifier.Failure("Role name already exists");
                }
                else if (!string.IsNullOrEmpty(roleDto.Name))
                {
                    role.Name = roleDto.Name;
                }

                if (!string.IsNullOrEmpty(roleDto.Description))
                {
                    role.Description = roleDto.Description;
                }

                if (!string.IsNullOrEmpty(roleDto.Status))
                {
                    if (!CommonTags.RoleStatuses.Contains(roleDto.Status))
                    {
                        return ResultNotifier.Failure($"Invalid status. Valid values are: {string.Join(", ", CommonTags.RoleStatuses)}");
                    }
                    role.Status = roleDto.Status;
                }

                // Check if BusinessId was included and is valid
                var paramList = _itemListTools.ObjectToItemList(roleDto);
                if (paramList.ContainsKey("BusinessId") && roleDto.BusinessId > 0)
                {
                    var business = await _context.Business
                        .FirstOrDefaultAsync(p => p.Id == roleDto.BusinessId && p.Status == CommonTags.Active);

                    if (business == null)
                    {
                        return ResultNotifier.Failure($"Business with ID {roleDto.BusinessId} not found or is inactive");
                    }

                    role.Business = business;
                }

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                var updatedRoleDto = _mapper.Map<RoleDTO>(role);
                return ResultNotifier.Success(updatedRoleDto);
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
                _logger.LogError(ex, "Error updating role");
                return ResultNotifier.Failure("Error updating role");
            }
        }
    }
}
