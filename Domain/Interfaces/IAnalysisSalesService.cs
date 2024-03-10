namespace Domain.Interfaces;

public interface IAnalysisSalesService
{
    double GetAverageDaySales(long idProduct);
    double GetSalesPrediction(long idProduct, long days);
    double GetSalesDemand(long idProduct, long days);
}