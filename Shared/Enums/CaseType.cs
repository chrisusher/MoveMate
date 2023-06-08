using Newtonsoft.Json.Converters;

namespace ChrisUsher.MoveMate.Shared.Enums;

[Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
public enum CaseType
{
    MiddleCase = 0,
    WorstCase = 1,
    BestCase = 2
}