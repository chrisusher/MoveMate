using Newtonsoft.Json.Converters;

namespace ChrisUsher.MoveMate.Shared.Enums;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum PropertyType
{
    ToPurchase = 0,
    Current = 1
}