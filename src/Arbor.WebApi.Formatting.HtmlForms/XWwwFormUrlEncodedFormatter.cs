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
            var convertedObject = ReadObjectFromStream(readStream, type);

            return Task.FromResult(convertedObject);
        }

        object ReadObjectFromStream(Stream stream, Type type)
        {
            var pairs = stream.ReadFormUrlEncoded(ReadBufferSize).ToArray();

            var dynamicObject = new ExpandoObject();

            var pairGroups =
                pairs.GroupBy(pair => pair.Key)
                    .Select(x => new {x.Key, Values = x.Select(item => item.Value).ToList()})
                    .ToArray();

            foreach (var keyValuePair in pairGroups.Where(x => x.Values.Count == 1))
            {
                ((IDictionary<string, object>) dynamicObject)[keyValuePair.Key] = keyValuePair.Values.Single();
            }

            foreach (var keyValuePair in pairGroups.Where(x => x.Values.Count >= 2))
            {
                string[] values = keyValuePair.Values.ToArray();

                ((IDictionary<string, object>) dynamicObject)[keyValuePair.Key] = values;
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