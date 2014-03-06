// Copyright (c) Microsoft Open Technologies, Inc.  All rights reserved.
// Microsoft Open Technologies would like to thank its contributors, a list
// of whom are at http://aspnetwebstack.codeplex.com/wikipage?title=Contributors.

// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License. You may
// obtain a copy of the License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied. See the License for the specific language governing permissions
// and limitations under the License.

// Modifications by Niklas Lundberg

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

namespace Arbor.WebApi.Formatting.HtmlForms
{
    internal static class StreamExtensions
    {
        const int MinBufferSize = 256;

        /// <summary>
        ///     Reads all name-value pairs encoded as HTML Form URL encoded data and add them to
        ///     a collection as UNescaped URI strings.
        /// </summary>
        /// <param name="input">Stream to read from.</param>
        /// <param name="bufferSize">Size of the buffer used to read the contents.</param>
        /// <returns>Collection of name-value pairs.</returns>
        public static IEnumerable<KeyValuePair<string, string>> ReadFormUrlEncoded(this Stream input, int bufferSize)
        {
            Contract.Assert(input != null, "input stream cannot be null");
            Contract.Assert(bufferSize >= MinBufferSize, "buffer size cannot be less than MinBufferSize");

            byte[] data = new byte[bufferSize];

            int bytesRead;
            bool isFinal = false;
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            FormUrlEncodedParser parser = new FormUrlEncodedParser(result, Int64.MaxValue);
            ParserState state;

            while (true)
            {
                bytesRead = input.Read(data, 0, data.Length);
                if (bytesRead == 0)
                {
                    isFinal = true;
                }


                int bytesConsumed = 0;
                state = parser.ParseBuffer(data, bytesRead, ref bytesConsumed, isFinal);
                if (state != ParserState.NeedMoreData && state != ParserState.Done)
                {
                    throw new InvalidOperationException("bytesConsumed " + bytesConsumed);
                }

                if (isFinal)
                {
                    return result;
                }
            }
        }
    }
}