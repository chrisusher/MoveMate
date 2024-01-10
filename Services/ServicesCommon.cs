using System.Text.Json.Serialization;

namespace ChrisUsher.MoveMate.API.Services
{
    public class ServicesCommon
    {
        private static JsonSerializerOptions _jsonOptions;

        public static JsonSerializerOptions JsonOptions
        {
            get
            {
                if (_jsonOptions is null)
                {
                    _jsonOptions = new JsonSerializerOptions
                    {
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                        WriteIndented = true
                    };

                    _jsonOptions.Converters.Add(new JsonStringEnumConverter());
                }

                return _jsonOptions;
            }
        }

    }
}