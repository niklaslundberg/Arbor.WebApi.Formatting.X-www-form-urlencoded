using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    public class XWwwFormUrlEncodedFormatter : FormUrlEncodedMediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            var task = ReadObjectFromStreamAsync(type, readStream, content, formatterLogger);

            return task;
        }

        async Task<object> ReadObjectFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            var targetType = typeof (FormDataCollection);

            object obj = await base.ReadFromStreamAsync(targetType, readStream, content, formatterLogger);

            var formDataCollection = (FormDataCollection) obj;

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

            var singleValuePairs = pairGroups.Where(x => x.Values.Count() == 1);

            foreach (var keyValuePair in singleValuePairs)
            {
                dynamicObjectDictionary[keyValuePair.Key] = keyValuePair.Values.Single();
            }

            var multipleValuesPairs = pairGroups.Where(x => x.Values.Count() >= 2);

            foreach (var keyValuePair in multipleValuesPairs)
            {
                string[] values = keyValuePair.Values;

                dynamicObjectDictionary[keyValuePair.Key] = values;
            }

            var json = JsonConvert.SerializeObject(dynamicObject);

            try
            {
                var instance = JsonConvert.DeserializeObject(json, type);

                return instance;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}