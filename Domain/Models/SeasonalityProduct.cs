namespace Domain.Models;

public record SeasonalityProduct(
    long Id,
    int Month,
    double Coef
);