

namespace ChrisUsher.MoveMate.Shared.DTOs.Savings.Stocks;

public class StocksAndSharesDetails : UpdateStocksAndSharesRequest
{
    [JsonPropertyName("stockId")]
    public Guid StockId { get; set; }
}
