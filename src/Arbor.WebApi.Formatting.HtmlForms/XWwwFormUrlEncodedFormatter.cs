using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    public class XWwwFormUrlEncodedFormatter : FormUrlEncodedMediaTypeFormatter
    {
        public override bool CanReadType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            bool isConcreteClass = type.IsClass && !type.IsAbstract;

            return isConcreteClass;
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (readStream == null)
            {
                throw new ArgumentNullException(nameof(readStream));
            }

            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (!CanReadType(type))
            {
                throw new NotSupportedException($"Cannot deserialize type {type.FullName}");
            }

            Task<object> task = ReadObjectFromStreamAsync(type, readStream, content, formatterLogger);

            return task;
        }

        async Task<object> ReadObjectFromStreamAsync(Type type, Stream readStream, HttpContent content,
            IFormatterLogger formatterLogger)
        {
            Type formDataType = typeof (FormDataCollection);

            object deserializedObject = await base.ReadFromStreamAsync(formDataType, readStream, content, formatterLogger);

            var formDataCollection = (FormDataCollection) deserializedObject;

            object instance = formDataCollection.ParseFromCollection(type);
            return instance;
        }
    }
}