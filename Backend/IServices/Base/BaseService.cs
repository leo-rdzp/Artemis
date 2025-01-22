using Artemis.Backend.Core.Utilities;

namespace Artemis.Backend.IServices.Base
{
    public abstract class BaseService<T> : IService<T>
    {
        private bool _autoCommit = true;
        private List<string>? _mandatoryParameters;
        private readonly ItemListTools _listTools = new();

        protected virtual void ValidateParameters(T parameters)
        {
            PrepareMandatoryParameters(); //call here to complete the validation with the service implementation!

            if (parameters == null)
            {
                throw new ArtemisException("No parameters provided",
                    "Mandatory parameters: " + string.Join(" | ", _mandatoryParameters ?? new List<string>()));
            }

            var paramList = (_listTools?.ObjectToItemList(parameters)) ?? throw new ArtemisException($"Cannot convert object {parameters.GetType()} to ItemList");

            if (_mandatoryParameters?.Count > 0)
            {
                var missingParams = new List<string>();
                foreach (var param in _mandatoryParameters)
                {
                    if (!(paramList != null && paramList.ContainsKey(param)))
                    {
                        missingParams.Add(param);
                    }
                }

                if (missingParams.Any())
                {
                    throw new ArtemisException("Required parameters missing",
                        $"Missing parameters: [{string.Join(", ", missingParams)}]");
                }
            }
        }

        protected virtual void OnServiceInitialize() { }
        public bool IsAutoCommit() => _autoCommit;
        public void SetAutoCommit(bool autoCommit)
        {
            _autoCommit = autoCommit;
            OnServiceInitialize();
        }

        protected void AddMandatoryParameter(string key)
        {
            _mandatoryParameters ??= new List<string>();
            _mandatoryParameters.Add(key);
        }

        protected void SetMandatoryParameters(List<string> parameters)
        {
            _mandatoryParameters = parameters;
        }

        protected List<string> GetMandatoryParameters() => _mandatoryParameters ?? new List<string>();

        protected void AppendMandatoryParameters(List<string> parameters)
        {
            _mandatoryParameters ??= new List<string>();
            _mandatoryParameters.AddRange(parameters);
        }

        public abstract void PrepareMandatoryParameters();
        public abstract Task<ResultNotifier> ExecuteAsync(T dto);
    }
}
