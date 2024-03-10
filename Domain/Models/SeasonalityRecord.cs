namespace Domain.Models;

public record SeasonalityRecord(
    long Id,
    int Month,
    double Coef
);