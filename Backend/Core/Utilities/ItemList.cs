namespace Artemis.Backend.Core.Utilities
{
    public class ItemList : Dictionary<string, object>
    {
        /// <summary>
        /// Creates a deep copy of the current ItemList
        /// </summary>
        public ItemList Clone()
        {
            var newItemList = new ItemList();
            foreach (var entry in this)
            {
                newItemList.Add(entry.Key, entry.Value);
            }
            return newItemList;
        }

        /// <summary>
        /// Gets a string value for the specified key
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when key is not found</exception>
        /// <exception cref="InvalidCastException">Thrown when value cannot be converted to string</exception>
        public string GetString(string key)
        {
            if (!ContainsKey(key))
                throw new KeyNotFoundException($"Key '{key}' not found in ItemList");

            return this[key]?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets a string value for the specified key, or returns defaultValue if key not found or value is null/empty
        /// </summary>
        public string GetString(string key, string defaultValue)
        {
            if (!ContainsKey(key))
                return defaultValue;

            var value = this[key]?.ToString();
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
        /// Gets an integer value for the specified key
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when key is not found</exception>
        /// <exception cref="FormatException">Thrown when value cannot be converted to int</exception>
        public int GetInt(string key)
        {
            if (!ContainsKey(key))
                throw new KeyNotFoundException($"Key '{key}' not found in ItemList");

            var value = this[key];
            return value switch
            {
                int intValue => intValue,
                long longValue => (int)longValue,
                string strValue => int.Parse(strValue),
                _ => int.Parse(value?.ToString() ?? "0")
            };
        }

        /// <summary>
        /// Gets an integer value for the specified key, or returns defaultValue if key not found or conversion fails
        /// </summary>
        public int GetInt(string key, int defaultValue)
        {
            if (!ContainsKey(key))
                return defaultValue;

            var value = this[key];
            try
            {
                return value switch
                {
                    int intValue => intValue,
                    long longValue => (int)longValue,
                    string strValue => int.Parse(strValue),
                    _ => int.Parse(value?.ToString() ?? defaultValue.ToString())
                };
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets an object for the specified key, or returns defaultValue if key not found
        /// </summary>
        public object Get(string key, object defaultValue)
        {
            return ContainsKey(key) ? this[key] : defaultValue;
        }

        /// <summary>
        /// Gets an object for the specified key
        /// </summary>
        /// <exception cref="KeyNotFoundException">Thrown when key is not found</exception>
        public object Get(string key)
        {
            if (!ContainsKey(key))
                throw new KeyNotFoundException($"Key '{key}' not found in ItemList");

            return this[key];
        }

        /// <summary>
        /// Gets a boolean value for the specified key
        /// </summary>
        public bool GetBoolean(string key)
        {
            if (!ContainsKey(key))
                return false;

            var value = this[key];

            if (value == null)
                return false;

            if (value is bool boolValue)
                return boolValue;

            var stringValue = value.ToString()?.ToLower() ?? "";

            return stringValue switch
            {
                "true" => true,
                "false" => false,
                "1" => true,
                "0" => false,
                _ => !string.IsNullOrEmpty(stringValue)
            };
        }
    }
}
