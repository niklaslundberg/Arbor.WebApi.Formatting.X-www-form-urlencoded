using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    public static class FormsExtensions
    {
        public static object ParseFromCollection<T>(this FormDataCollection formDataCollection)
        {
            return ParseFromCollection(formDataCollection, typeof (T));
        }

        public static object ParseFromCollection(this FormDataCollection formDataCollection, Type targetType)
        {
            if (formDataCollection == null)
            {
                throw new ArgumentNullException("formDataCollection");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            var dynamicObject = new ExpandoObject();

            var pairGroups = formDataCollection
                .GroupBy(pair => pair.Key)
                .Select(grouping => new
                                    {
                                        grouping.Key,
                                        Values = grouping
                                            .Select(item => item.Value)
                                            .ToArray()
                                    })
                .ToArray();

            IDictionary<string, object> dynamicObjectDictionary = dynamicObject;

            var singleValuePairs = pairGroups.Where(pairGroup => pairGroup.Values.Count() == 1);

            foreach (var keyValuePair in singleValuePairs)
            {
                dynamicObjectDictionary[keyValuePair.Key] = keyValuePair.Values.Single();
            }

            var multipleValuesPairs = pairGroups.Where(pairGroup => pairGroup.Values.Count() >= 2);

            foreach (var keyValuePair in multipleValuesPairs)
            {
                string[] values = keyValuePair.Values;

                dynamicObjectDictionary[keyValuePair.Key] = values;
            }

            string json = JsonConvert.SerializeObject(dynamicObject);

            try
            {
                object instance = JsonConvert.DeserializeObject(json, targetType);

                return instance;
            }
            catch (JsonSerializationException ex)
            {
                throw new InvalidOperationException(
                    string.Format("Could not deserialize type {0} from JSON '{1}'", targetType, json), ex);
            }
        }
    }
}