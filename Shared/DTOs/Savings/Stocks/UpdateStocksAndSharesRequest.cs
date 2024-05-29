namespace ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

public class UpdateStocksAndSharesRequest : CreateStocksAndSharesRequest
{
    [JsonPropertyName("balances")]
    public List<StockBalance> Balances { get; set; }
}
