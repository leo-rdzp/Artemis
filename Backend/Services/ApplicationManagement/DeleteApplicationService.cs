using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.ApplicationManagement
{
    public class DeleteApplicationService(
        ArtemisDbContext context,
        ILogger<DeleteApplicationService> logger,
        ITransactionScope transactionScope) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ILogger<DeleteApplicationService> _logger = logger;
        private readonly ITransactionScope _transactionScope = transactionScope;

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

        public override async Task<ResultNotifier> ExecuteAsync(int applicationId)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                var application = await _context.Applications.FindAsync(applicationId);
                if (application == null)
                {
                    return ResultNotifier.Failure("Application not found");
                }

                application.Status = CommonTags.Inactive;
                application.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }
                return ResultNotifier.Success("Application deleted successfully");
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
                _logger.LogError(ex, "Error deleting application {applicationId}", applicationId);
                return ResultNotifier.Failure("Error deleting application");
            }
        }
    }
}
