namespace Domain.Interfaces;

public interface IAnalysisSalesService
{
    double GetAverageDaySales(long productId);
    double GetSalesPrediction(long productId, long days);
    double GetSalesDemand(long productId, long days);
}