using Microsoft.EntityFrameworkCore.Storage;

namespace Artemis.Backend.Connections.Database
{
    public class TransactionScope(ArtemisDbContext context) : ITransactionScope, IDisposable
    {
        private readonly ArtemisDbContext _context = context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        public IDbContextTransaction GetTransaction()
        {
            if (_transaction == null || _context.Database.CurrentTransaction == null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
            return _transaction;
        }

        public async Task<IDbContextTransaction> GetTransactionAsync()
        {
            if (_transaction == null || _context.Database.CurrentTransaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
            return _transaction;
        }

        public void Commit()
        {
            if (_transaction == null) return;

            try
            {
                if (_transaction?.GetDbTransaction().Connection != null)
                {
                    _transaction.Commit();
                }
            }
            catch
            {
                if (_transaction?.GetDbTransaction().Connection != null)
                {
                    _transaction.Rollback();
                }
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction == null) return;

            try
            {
                if (_transaction?.GetDbTransaction().Connection != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                if (_transaction?.GetDbTransaction().Connection != null)
                {
                    await _transaction.RollbackAsync();
                }
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Rollback()
        {
            if (_transaction == null) return;

            try
            {
                if (_transaction?.GetDbTransaction().Connection != null)
                {
                    _transaction.Rollback();
                }
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null) return;

            try
            {
                if (_transaction?.GetDbTransaction().Connection != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
