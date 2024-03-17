namespace AnalysisSales.Models;

public record SaleAnalyticRequest(
    SaleAnalyticCommand SaleAnalytic,
    long ProductId,
    long Days = 0
);