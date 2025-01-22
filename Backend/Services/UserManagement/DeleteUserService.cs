using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.UserManagement
{
    public class DeleteUserService(
        ArtemisDbContext context,
        ILogger<DeleteUserService> logger,
        ITransactionScope transactionScope) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ILogger<DeleteUserService> _logger = logger;
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

        public override async Task<ResultNotifier> ExecuteAsync(int userId)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return ResultNotifier.Failure("User not found");
                }

                user.Status = CommonTags.Deleted;
                user.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }
                return ResultNotifier.Success("User deleted successfully");
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
                _logger.LogError(ex, "Error deleting user {userId}", userId);
                return ResultNotifier.Failure("Error deleting user");
            }
        }
    }
}
