using System;
using System.IO.Compression;
using Xunit;

namespace Light.Serialization.Tests
{
    public sealed class JsonEnumerationDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory]
        [InlineData("\"black\"", ConsoleColor.Black)]
        [InlineData("\"insertLineBreaks\"", Base64FormattingOptions.InsertLineBreaks)]
        [InlineData("\"noCompression\"", CompressionLevel.NoCompression)]
        public void EnumValues<T>(string json, T expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }
    }
}