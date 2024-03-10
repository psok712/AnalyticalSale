using System;
using System.Text.Json.Serialization;

namespace Domain.Models;

public record SaleRecord(    
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("date")] DateTime Date,
    [property: JsonPropertyName("sales")] long Sales,
    [property: JsonPropertyName("stock")] long Stock
);