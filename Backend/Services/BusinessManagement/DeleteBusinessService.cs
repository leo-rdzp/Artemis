using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Services.BusinessManagement
{
    public class DeleteBusinessService(
        ArtemisDbContext context,
        ILogger<DeleteBusinessService> logger,
        ITransactionScope transactionScope) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ILogger<DeleteBusinessService> _logger = logger;
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

        public override async Task<ResultNotifier> ExecuteAsync(int businessId)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                // Check if business exists and has no active dependencies
                var business = await _context.Business
                    .Include(b => b.Products)
                    .Include(b => b.Applications)
                    .Include(b => b.Areas)
                    .FirstOrDefaultAsync(b => b.Id == businessId);

                if (business == null)
                {
                    return ResultNotifier.Failure("Business not found");
                }

                // Check for active dependencies
                if (business.Products != null && business.Products.Any())
                {
                    return ResultNotifier.Failure("Cannot delete business with active Products");
                }

                // Check active business locations
                if (business.Applications != null && business.Applications.Any(app => app.Status != CommonTags.Deleted))
                {
                    return ResultNotifier.Failure("Cannot delete business with active Applications");
                }

                // Check active areas
                if (business.Areas != null && business.Areas.Any(a => a.Status != CommonTags.Deleted))
                {
                    return ResultNotifier.Failure("Cannot delete business with active areas");
                }

                // Soft delete the business
                business.Status = CommonTags.Inactive;
                business.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }
                return ResultNotifier.Success("Business deleted successfully");
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
                _logger.LogError(ex, "Error deleting business {businessId}", businessId);
                return ResultNotifier.Failure("Error deleting business");
            }
        }
    }
}
