namespace Domain.Interfaces;

public interface IAnalyticalSaleService
{
    double GetAverageDaySales(long productId);
    double GetSalesPrediction(long productId, long days);
    double GetSalesDemand(long productId, long days);
}