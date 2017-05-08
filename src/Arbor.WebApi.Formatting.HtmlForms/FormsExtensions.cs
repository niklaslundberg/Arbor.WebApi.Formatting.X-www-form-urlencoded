using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using Newtonsoft.Json;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    public static class FormsExtensions
    {
        public static object ParseFromCollection<T>(this FormDataCollection formDataCollection)
        {
            if (formDataCollection == null)
            {
                throw new ArgumentNullException(nameof(formDataCollection));
            }

            return ParseFromCollection(formDataCollection, typeof(T));
        }

        public static object ParseFromCollection(
            this FormDataCollection formDataCollection,
            Type targetType)
        {
            if (formDataCollection == null)
            {
                throw new ArgumentNullException(nameof(formDataCollection));
            }

            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            return ParseFromPairs(formDataCollection, targetType);
        }

        public static object ParseFromPairs(
            IEnumerable<KeyValuePair<string, string>> collection,
            Type targetType)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (targetType == null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            var dynamicObject = new ExpandoObject();

            var pairGroups = collection
                .GroupBy(pair => pair.Key)
                .Select(grouping => new
                {
                    grouping.Key,
                    Values = grouping
                        .Select(item => item.Value)
                        .ToArray()
                })
                .ToArray();

            var nested = pairGroups.Where(pair => pair.Key.Contains("[")).ToArray();

            IDictionary<string, object> dynamicObjectDictionary = dynamicObject;

            var singleValuePairs = pairGroups.Where(pairGroup => pairGroup.Values.Length == 1).Except(nested);

            foreach (var keyValuePair in singleValuePairs)
            {
                dynamicObjectDictionary[keyValuePair.Key] = keyValuePair.Values.Single();
            }

            var multipleValuesPairs = pairGroups.Where(pairGroup => pairGroup.Values.Length >= 2).Except(nested);

            foreach (var keyValuePair in multipleValuesPairs)
            {
                string[] values = keyValuePair.Values;

                dynamicObjectDictionary[keyValuePair.Key] = values;
            }

            string json = JsonConvert.SerializeObject(dynamicObject);

            try
            {
                Console.WriteLine("parsing " + json + " into type " + targetType);
                object instance = JsonConvert.DeserializeObject(json, targetType);

                dynamic asDynamic = instance;

                if (instance != null)
                {
                    foreach (PropertyInfo propertyInfo in targetType.GetProperties()
                        .Where(property => property.CanWrite &&
                                           typeof(IEnumerable).IsAssignableFrom(
                                               property.PropertyType) && property.PropertyType.IsGenericType))
                    {
                        Type subTargetType = propertyInfo.PropertyType.GenericTypeArguments.FirstOrDefault();

                        string expectedName = propertyInfo.Name;

                        var matchingProperty = nested.Select(nestedGroup =>
                            {
                                int indexIndex = nestedGroup.Key.IndexOf("[");
                                int indexStopIndex = nestedGroup.Key.IndexOf("]");
                                int indexLength = indexStopIndex - indexIndex;

                                int dotIndex = nestedGroup.Key.IndexOf(".");

                                string name = nestedGroup.Key.Substring(0, indexIndex);

                                string index = nestedGroup.Key.Substring(indexIndex + 1, indexLength - 1);

                                string propertyName = nestedGroup.Key.Substring(dotIndex + 1);

                                return new { GroupName = name, nestedGroup.Values, Index = index, propertyName };
                            })
                            .Where(s => s.GroupName.Equals(expectedName, StringComparison.OrdinalIgnoreCase))
                            .ToArray();

                        var subPairs = new List<KeyValuePair<string, string>>();

                        var indexedGroups = matchingProperty.GroupBy(_ => _.Index);

                        foreach (var item in indexedGroups)
                        {
                            var pairs = new List<KeyValuePair<string, string>>();
                            foreach (var value in item)
                            {
                                foreach (string valueProperty in value.Values)
                                {
                                    pairs.Add(new KeyValuePair<string, string>(value.propertyName, valueProperty));
                                }
                            }

                            object currentCollection = propertyInfo.GetValue(instance);

                            if (currentCollection == null)
                            {
                                object newCollection = null;

                                if (!propertyInfo.PropertyType.IsAbstract)
                                {
                                    try
                                    {
                                        newCollection = Activator.CreateInstance(propertyInfo.PropertyType);
                                    }
                                    catch (Exception)
                                    {
                                        // Ignore exception
                                    }
                                }
                                else
                                {
                                    var listType = typeof(List<>);
                                    var constructedListType = listType.MakeGenericType(subTargetType);

                                    try
                                    {
                                        newCollection = Activator.CreateInstance(constructedListType);
                                    }
                                    catch (Exception)
                                    {
                                        // Ignore exception
                                    }
                                }

                                if (newCollection == null)
                                {
                                    throw new InvalidOperationException(
                                        $"Could not create new {propertyInfo.PropertyType.FullName}");
                                }

                                if (propertyInfo.PropertyType.IsAssignableFrom(newCollection.GetType()))
                                {
                                    propertyInfo.SetValue(instance, newCollection);

                                    currentCollection = newCollection;
                                }
                            }

                            object subTargetInstance = ParseFromPairs(pairs, subTargetType);

                            var genericCollectionType = typeof(ICollection<>);

                            var constructedCollectionType = genericCollectionType.MakeGenericType(subTargetType);

                            if (constructedCollectionType.IsAssignableFrom(currentCollection.GetType()))
                            {
                                var addMethod = currentCollection.GetType()
                                    .GetMethod("Add", BindingFlags.Instance | BindingFlags.Public);

                                addMethod.Invoke(currentCollection, new object[] { subTargetInstance });
                            }
                        }
                    }
                }

                if (instance == null)
                {

                    throw new FormParseException($"Could not parse values to object of type '{targetType}'");
                }

                Console.WriteLine(instance);

                return instance;
            }
            catch (Exception ex) when (ex.ShouldCatch())
            {
                throw new FormParseException(
                    string.Format("Could not deserialize type {0} from JSON '{1}'", targetType, json),
                    ex);

            }
        }
    }
}