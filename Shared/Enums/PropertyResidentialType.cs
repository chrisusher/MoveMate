using Newtonsoft.Json.Converters;

namespace ChrisUsher.MoveMate.Shared.Enums;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum PropertyResidentialType
{
    Residential = 0,

    NonResidential = 1
}