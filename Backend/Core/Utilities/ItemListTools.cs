using System.Collections;
using System.Reflection;

namespace Artemis.Backend.Core.Utilities
{
    public class ItemListTools
    {
        private readonly HashSet<string> PropertiesNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "Properties"
        };

        // Helper method for safe string conversion
        private string SafeToString(object? value) => value?.ToString() ?? string.Empty;

        public ItemList ConvertPropertiesDTOToItemList(List<object> propertiesListModel)
        {
            var propItemLst = new ItemList();
            if (propertiesListModel.Count == 0) return propItemLst;

            foreach (var property in propertiesListModel)
            {
                var propertyType = property.GetType();
                var valueProperty = propertyType.GetProperty("Value");
                var tagProperty = propertyType.GetProperty("Tag");

                if (valueProperty == null || tagProperty == null) continue;

                var stringValue = valueProperty.GetValue(property) switch
                {
                    byte[] byteValue => System.Text.Encoding.UTF8.GetString(byteValue),
                    var val => SafeToString(val)
                };

                var stringTag = SafeToString(tagProperty.GetValue(property));
                if (string.IsNullOrEmpty(stringTag)) continue;

                var value = ParseValue(stringValue);
                AddToItemList(propItemLst, stringTag, value);
            }

            return propItemLst;
        }

        private object ParseValue(string stringValue)
        {
            if (int.TryParse(stringValue, out var intValue) && !stringValue.StartsWith("0"))
                return intValue;

            if (bool.TryParse(stringValue, out var boolValue))
                return boolValue;

            return stringValue;
        }

        private void AddToItemList(ItemList itemList, string key, object value)
        {
            if (!itemList.ContainsKey(key))
            {
                itemList.Add(key, value);
                return;
            }

            if (itemList[key] is List<object> list)
            {
                list.Add(value);
            }
            else
            {
                itemList[key] = new List<object> { itemList[key], value };
            }
        }

        public ItemList ObjectToItemList(object obj, List<string>? includeTags = null)
        {
            if (obj == null) return new ItemList();

            var result = new ItemList();
            var properties = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => includeTags?.Contains(p.Name) != false);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;

                ProcessPropertyValue(result, prop.Name, value, includeTags);
            }

            return result;
        }

        public ItemList ArrayToItemList(List<object> list, string propName, List<string>? includeTags)
        {
            if (list.Count == 0) return [];

            return IsPropertiesName(propName)
                ? HandlePropertiesList(list)
                : HandleRegularList(list, includeTags);
        }

        private ItemList HandlePropertiesList(List<object> list)
        {
            var result = new ItemList();
            var convertedList = ConvertPropertiesDTOToItemList(list);

            foreach (var (key, value) in convertedList)
            {
                result.Add(key, SafeToString(value));
            }

            return result;
        }

        private ItemList HandleRegularList(List<object> list, List<string>? includeTags)
        {
            var result = new ItemList();

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item == null) continue;

                ProcessListItem(result, item, i, includeTags);
            }

            return result;
        }

        private void ProcessPropertyValue(ItemList result, string propName, object value, List<string>? includeTags)
        {
            switch (value)
            {
                case DateTime or string or bool or int or Int32 or Int16 or Int64 or double:
                    result.Add(propName, SafeToString(value));
                    break;

                case ItemList itemList:
                    foreach (var item in itemList)
                    {
                        result.Add($"{propName}.{item.Key}", SafeToString(item.Value));
                    }
                    break;

                case IEnumerable enumerable when value.GetType().IsGenericType:
                    HandleEnumerableProperty(result, propName, enumerable, includeTags);
                    break;

                default:
                    HandleComplexProperty(result, propName, value, includeTags);
                    break;
            }
        }

        private void HandleEnumerableProperty(ItemList result, string propName, IEnumerable enumerable, List<string>? includeTags)
        {
            var auxItemList = ArrayToItemList(enumerable.Cast<object>().ToList(), propName, includeTags);
            var prefix = IsPropertiesName(propName) ? $"{propName}." : propName;

            foreach (var item in auxItemList)
            {
                result.Add($"{prefix}{item.Key}", SafeToString(item.Value));
            }
        }

        private void HandleComplexProperty(ItemList result, string propName, object value, List<string>? includeTags)
        {
            var nestedItemList = ObjectToItemList(value, includeTags);
            foreach (var item in nestedItemList)
            {
                result.Add($"{propName}.{item.Key}", SafeToString(item.Value));
            }
        }

        private void ProcessListItem(ItemList result, object item, int index, List<string>? includeTags)
        {
            var key = $"[{index}]";

            switch (item)
            {
                case DateTime or string or bool or int or Int32 or Int16 or Int64:
                    result.Add(key, SafeToString(item));
                    break;

                case IEnumerable enumerable when item.GetType().IsGenericType:
                    var nestedList = ArrayToItemList(enumerable.Cast<object>().ToList(), key, includeTags);
                    foreach (var (nestedKey, value) in nestedList)
                    {
                        result.Add($"{key}.{nestedKey}", SafeToString(value));
                    }
                    break;

                default:
                    var objectList = ObjectToItemList(item, includeTags);
                    foreach (var (propertyKey, value) in objectList)
                    {
                        result.Add($"{key}.{propertyKey}", SafeToString(value));
                    }
                    break;
            }
        }

        private bool IsPropertiesName(string name)
            => PropertiesNames.Contains(name);
    }
}