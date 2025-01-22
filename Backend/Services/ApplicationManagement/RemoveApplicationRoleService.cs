using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.DTO.Authentication;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class RemoveApplicationRoleService(
    ArtemisDbContext context,
    ILogger<RemoveApplicationRoleService> logger,
    ITransactionScope transactionScope) : BaseService<ApplicationRoleDTO>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ILogger<RemoveApplicationRoleService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

        public override void PrepareMandatoryParameters()
        {
            SetMandatoryParameters(["ApplicationId", "RoleId"]);
        }

        protected override void OnServiceInitialize()
        {
            base.OnServiceInitialize();
            if (!IsAutoCommit())
            {
                _context.Database.UseTransaction(_context.Database.CurrentTransaction?.GetDbTransaction());
            }
        }

        public override async Task<ResultNotifier> ExecuteAsync(ApplicationRoleDTO assignmentDto)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                ValidateParameters(assignmentDto);

                var applicationRole = await _context.ApplicationRoles
                    .Include(ar => ar.Application)
                    .Include(ar => ar.Role)
                    .FirstOrDefaultAsync(ar =>
                        ar.Application!.Id == assignmentDto.ApplicationId &&
                        ar.Role!.Id == assignmentDto.RoleId);

                if (applicationRole == null)
                    return ResultNotifier.Failure("Application role assignment not found");

                _context.ApplicationRoles.Remove(applicationRole);
                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }

                return ResultNotifier.Success("Application role removed successfully");
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
                _logger.LogError(ex, "Error removing application role");
                return ResultNotifier.Failure("Error removing application role");
            }
        }
    }
}
