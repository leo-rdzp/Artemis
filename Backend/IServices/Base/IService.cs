using Artemis.Backend.Core.Utilities;

namespace Artemis.Backend.IServices.Base
{
    public interface IService<T>
    {
        void PrepareMandatoryParameters();
        Task<ResultNotifier> ExecuteAsync(T dto);
    }
}
