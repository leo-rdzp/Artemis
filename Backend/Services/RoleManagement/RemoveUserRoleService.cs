using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.RoleManagement
{
    public class RemoveUserRoleService(
    ArtemisDbContext context,
    ILogger<RemoveUserRoleService> logger,
    ITransactionScope transactionScope) : BaseService<UserRoleDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ILogger<RemoveUserRoleService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["UserId", "RoleId"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(UserRoleDTO assignmentDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(assignmentDto);

                var userRole = await _context.UserRoles
                    .Include(ur => ur.User)
                    .Include(ur => ur.Role)
                    .FirstOrDefaultAsync(ur =>
                        ur.User!.Id == assignmentDto.UserId &&
                        ur.Role!.Id == assignmentDto.RoleId);

                if (userRole == null)
                    return ResultNotifier.Failure("User role assignment not found");

                _context.UserRoles.Remove(userRole);

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                return ResultNotifier.Success("User role removed successfully");
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
                _logger.LogError(ex, "Error removing user role");
                return ResultNotifier.Failure("Error removing user role");
            }
        }
    }
}
