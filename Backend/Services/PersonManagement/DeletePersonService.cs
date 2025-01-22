using Artemis.Backend.Connections.Database;
using Artemis.Backend.Core.Utilities;
using Artemis.Backend.IServices.Base;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Backend.Services.PersonManagement
{
    public class DeletePersonService(
        ArtemisDbContext context,
        ILogger<DeletePersonService> logger,
        ITransactionScope transactionScope) : BaseService<int>
    {
        private readonly ArtemisDbContext _context = context;
        private readonly ILogger<DeletePersonService> _logger = logger;
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

        public override async Task<ResultNotifier> ExecuteAsync(int personId)
        {
            await _transactionScope.GetTransactionAsync();
            try
            {
                var person = await _context.Persons.FindAsync(personId);
                if (person == null)
                {
                    return ResultNotifier.Failure("Person not found");
                }

                // Check if person is associated with any active users
                var hasActiveUsers = _context.Users
                    .Any(u => u.Person.Id == personId && u.Status == CommonTags.Active);

                if (hasActiveUsers)
                {
                    return ResultNotifier.Failure("Cannot delete person with active user accounts");
                }

                // Soft delete
                person.Status = CommonTags.Deleted;
                person.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                if (IsAutoCommit())
                {
                    await _transactionScope.CommitAsync();
                }
                return ResultNotifier.Success("Person deleted successfully");
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
                _logger.LogError(ex, "Error deleting person {personId}", personId);
                return ResultNotifier.Failure("Error deleting person");
            }
        }
    }
}
