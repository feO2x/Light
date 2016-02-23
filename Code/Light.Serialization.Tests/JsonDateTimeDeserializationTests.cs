﻿using System;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.Serialization.Tests
{
    public sealed class JsonDateTimeDeserializationTests : BaseDefaultJsonDeserializationTest
    {
        [Theory(DisplayName = "JSON strings that represent date times are correctly parsed according to ISO 8601.")]
        [MemberData(nameof(ValidDateTimesData))]
        public void ValidDateTimes(string json, DateTime expected)
        {
            CompareDeserializedJsonToExpected(json, expected);
        }

        public static readonly TestData ValidDateTimesData =
            new[]
            {
                new object[] { "\"2016-02\"", new DateTime(2016, 2, 1, 0, 0, 0, DateTimeKind.Utc) },
                new object[] { "\"2016-02-12\"", new DateTime(2016, 2, 12, 0, 0, 0, DateTimeKind.Utc) },
                new object[] { "\"0011-04-29\"", new DateTime(11, 4, 29, 0, 0, 0, DateTimeKind.Utc) },
                new object[] { "\"2016-02-22T15:54+01\"", new DateTime(2016, 2, 22, 15, 54, 0, DateTimeKind.Local) },
                new object[] { "\"2016-02-22T14:55:47Z\"", new DateTime(2016, 2, 22, 14, 55, 47, DateTimeKind.Utc) },
                new object[] { "\"1990-09-30T03:08:00.125Z\"", new DateTime(1990, 09, 30, 3, 8, 0, 125, DateTimeKind.Utc) },
                new object[] { "\"1311-12-24T05:14:30.388-04\"", new DateTime(1311, 12, 24, 5, 14, 30, 388, DateTimeKind.Local) }
            };

        [Theory(DisplayName = "JSON strings that contain malformed date times result in an exception.")]
        [MemberData(nameof(InvalidDateTimesData))]
        public void InvalidDateTimes(string json)
        {
            CheckDeserializerThrowsExceptionWithMessage<DateTime>(json, $"The specified token {json} does not represent a valid date time.");
        }

        public static readonly TestData InvalidDateTimesData =
            new[]
            {
                new object[] { "\"201-01\"" },
                new object[] { "\"2000-13\"" },
                new object[] { "\"2014-01-32\"" },
                new object[] { "\"2011-02-29\"" },
                new object[] { "\"1987-04-31\"" },
                new object[] { "\"1955-01-00\"" },
                new object[] { "\"88-01-01\"" },
                new object[] { "\"1995-00\"" },
                new object[] { "\"1997-01-0114:15\"" },
                new object[] { "\"2001-04-17T3:15\"" },
                new object[] { "\"2001-04-17T03/15\"" },
                new object[] { "\"2001-04/17T03:15\"" },
                new object[] { "\"2001/04-17T03:15\"" },
                new object[] { "\"2001-04-17T03:15+1\"" },
                new object[] { "\"2001-04-17T03:15-1\"" },
                new object[] { "\"2010-09-20T05:14+01:1\"" },
                new object[] { "\"2010-09-20T05:14-01:7\"" }
            };
    }
}