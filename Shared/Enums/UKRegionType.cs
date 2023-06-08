using Newtonsoft.Json.Converters;

namespace ChrisUsher.MoveMate.Shared.Enums;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum UKRegionType
{
    England = 0,

    Wales = 1,

    Scotland = 2
}