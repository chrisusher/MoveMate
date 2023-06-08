using System.Text.Json.Serialization;

namespace ChrisUsher.MoveMate.API.Functions
{
    public class HttpFunction
    {
        private JsonSerializerOptions _options;

        protected JsonSerializerOptions JsonOptions 
        {
            get
            {
                if(_options == null)
                {
                    _options = new(JsonSerializerDefaults.Web)
                    {
                        WriteIndented = true
                    };
                    _options.Converters.Add(new JsonStringEnumConverter());
                    _options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                }

                return _options;
            }
        }
    }
}