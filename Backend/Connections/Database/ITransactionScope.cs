using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Connections.Database;

public interface ITransactionScope
{
    Task<IDbContextTransaction> GetTransactionAsync();
    IDbContextTransaction GetTransaction();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
    void Dispose();
}
