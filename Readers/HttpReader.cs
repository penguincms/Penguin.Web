using Loxifi;
using Penguin.Extensions.String;
using Penguin.SystemExtensions.Abstractions.Interfaces;
using Penguin.Web.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Penguin.Web.Readers
{
    public class HttpReader
    {
        private readonly byte[] DecodedBody;

        private readonly List<HttpHeader> headers = new();

        public IEnumerable<byte> Body => DecodedBody ?? RawBody;

        public string BodyText => Encoding.UTF8.GetString(Body.ToArray());

        public int HeaderBreak { get; protected set; }

        public string HeaderLine { get; protected set; }

        public IReadOnlyList<HttpHeader> Headers => headers;

        public bool IsChunked => string.Equals(GetHeader("transfer-encoding"), "chunked", StringComparison.OrdinalIgnoreCase);

        public IReadOnlyList<byte> Raw { get; protected set; }

        public IEnumerable<byte> RawBody => Raw.Skip(HeaderBreak + 4);

        public HttpReader(byte[] raw)
        {
            Raw = raw;

            SetHeaders();

            if (IsChunked)
            {
                DecodedBody = DecodeBodyBytes(RawBody);
            }

            if (string.Equals(GetHeader("Content-Encoding"), "gzip", StringComparison.OrdinalIgnoreCase))
            {
                DecodedBody = Decompress(Body);
            }
        }

        public static byte[] DecodeBodyBytes(IEnumerable<byte> toDecode)
        {
            byte[] chunkedBodyBytes = toDecode.ToArray();

            int curPos = 0;
            int chunkSize = 0;
            List<byte> bodyBytesL = new();

            do
            {
                string hexChunkSize = string.Empty;

                for (; curPos < chunkedBodyBytes.Length; curPos++)
                {
                    hexChunkSize += (char)chunkedBodyBytes[curPos];

                    if (chunkedBodyBytes[curPos + 1] == '\r' && chunkedBodyBytes[curPos + 2] == '\n')
                    {
                        curPos += 3;
                        break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(hexChunkSize))
                {
                    chunkSize = int.Parse(hexChunkSize, NumberStyles.HexNumber);

                    bodyBytesL.AddRange(chunkedBodyBytes.Skip(curPos).Take(chunkSize));

                    curPos += chunkSize + 2;
                }
            } while (chunkSize > 0);

            return bodyBytesL.ToArray();
        }

        public string GetHeader(string key)
        {
            return Headers.SingleOrDefault(h => string.Equals(h.Key, key, StringComparison.OrdinalIgnoreCase))?.Value;
        }

        public void SetHeaders()
        {
            SetHeaderBreak();

            string headerString = Encoding.ASCII.GetString(Raw.Take(HeaderBreak).ToArray());

            HeaderLine = headerString.To("\r\n");

            foreach (string header in headerString.Split("\r\n").Skip(1))
            {
                HttpHeader httpHeader = new();

                (httpHeader as IConvertible<string>).Convert(header);

                headers.Add(httpHeader);
            }
        }

        private static byte[] Decompress(IEnumerable<byte> gzip)
        {
            // Create a GZIP stream with decompression mode.
            // ... Then create a buffer and write into while reading from the GZIP stream.
            using GZipStream stream = new(new MemoryStream(gzip.ToArray()), CompressionMode.Decompress);
            const int size = 4096;
            byte[] buffer = new byte[size];
            using MemoryStream memory = new();
            int count = 0;
            do
            {
                count = stream.Read(buffer, 0, size);
                if (count > 0)
                {
                    memory.Write(buffer, 0, count);
                }
            }
            while (count > 0);
            return memory.ToArray();
        }

        private void SetHeaderBreak()
        {
            for (int bi = 0; bi < Raw.Count - 3; bi++)
            {
                if (Raw[bi] == '\r' && Raw[bi + 1] == '\n' && Raw[bi + 2] == '\r' && Raw[bi + 3] == '\n')
                {
                    HeaderBreak = bi;
                    return;
                }
            }

            throw new Exception("Post header break not found");
        }
    }
}