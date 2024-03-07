using System.Text.Json.Serialization;

namespace Domain.Models;

public record SeasonalityRecord(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("month")] int Month,
    [property: JsonPropertyName("coef")] double Coef
);