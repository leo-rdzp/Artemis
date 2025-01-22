using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.RoleManagement
{
    public class DeleteRoleService : BaseService<int>
    {
        private readonly ArtemisDbContext _context;
        private readonly ILogger<DeleteRoleService> _logger;
        private readonly ITransactionScope _transactionScope;

        public DeleteRoleService(
            ArtemisDbContext context,
            ILogger<DeleteRoleService> logger,
            ITransactionScope transactionScope)
        {
            _context = context;
            _logger = logger;
            _transactionScope = transactionScope;
        }

        public override void PrepareMandatoryParameters()
        {
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(int roleId)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                // Check if role exists and has no active users
                var role = await _context.Roles
                    .Include(r => r.UserRoles!.Where(ur => ur.User!.Status != CommonTags.Deleted))
                    .FirstOrDefaultAsync(r => r.Id == roleId);

                if (role == null)
                {
                    return ResultNotifier.Failure("Role not found");
                }

                // Check if role has active users
                if (role.UserRoles != null && role.UserRoles.Any())
                {
                    return ResultNotifier.Failure("Cannot delete role with active users assigned");
                }

                // Soft delete the role
                role.Status = CommonTags.Inactive;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }
                return ResultNotifier.Success("Role deleted successfully");
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
                _logger.LogError(ex, "Error deleting role {roleId}", roleId);
                return ResultNotifier.Failure("Error deleting role");
            }
        }
    }
}
