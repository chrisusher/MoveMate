namespace ChrisUsher.MoveMate.Shared.DTOs.Properties;

public class PropertyMarketDetails
{
    private int _numberOfBedrooms;
    private long _id;

    [JsonPropertyName("id")]
    public long Id
    {
        get
        {
            if (_id > 0)
            {
                return _id;
            }

            if (string.IsNullOrEmpty(Url))
            {
                return _id;
            }

            return long.Parse(Url.Split('/').Last(x => !string.IsNullOrEmpty(x)));
        }
        set
        {
            _id = value;
        }
    }

    [JsonPropertyName("heading")]
    public string Heading { get; set; } = string.Empty;

    [JsonPropertyName("subHeading")]
    public string SubHeading { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("numberOfBedrooms")]
    public int NumberOfBedrooms
    {
        get
        {
            if (_numberOfBedrooms > 0)
            {
                return _numberOfBedrooms;
            }
            if (string.IsNullOrEmpty(Heading))
            {
                return _numberOfBedrooms;
            }

            var headingWords = Heading.Split(' ');

            if (int.TryParse(headingWords[0], out _numberOfBedrooms))
            {
                return _numberOfBedrooms;
            }

            return _numberOfBedrooms;
        }
    }

    [JsonPropertyName("listPrice")]
    public int ListPrice { get; set; }

    [JsonPropertyName("floorSpaceSqFt")]
    public int? FloorSpaceSqFt { get; set; }

    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();
}