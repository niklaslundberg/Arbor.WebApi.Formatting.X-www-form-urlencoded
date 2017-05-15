using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using Arbor.ModelBinding.Core;
using Microsoft.Extensions.Primitives;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    public static class WebApiFormsExtensions
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

            IEnumerable<KeyValuePair<string, StringValues>> values = formDataCollection
                .GroupBy(item => item.Key)
                .Select(grouping => new KeyValuePair<string, StringValues>(grouping.Key,
                    new StringValues(grouping.Select(item => item.Value).ToArray())));

            return FormsExtensions.ParseFromPairs(values, targetType);
        }
    }
}