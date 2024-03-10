using DataAccess.Exceptions;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;

namespace Service;

public class AnalysisSalesService : IAnalysisSalesService
{                                               
    private readonly ISaleHistoryRepository _saleHistory;
    private readonly ISeasonalityProductsRepository _seasonalityProducts;

    public AnalysisSalesService()
    {
        _saleHistory = new SaleHistoryRepository();
        _seasonalityProducts = new SeasonalityProductsRepository();
    }
    
    public AnalysisSalesService(ISaleHistoryRepository saleHistory, ISeasonalityProductsRepository seasonalityProducts)
    {
        _saleHistory = saleHistory;
        _seasonalityProducts = seasonalityProducts;
    }
    
    public double GetAverageDaySales(long productId)
    {
        var allSales = _saleHistory.GetAllSales();
        var countDay = 0;
        long countSales = 0;

        foreach (var sale in allSales)
        {
            if (sale.Id == productId)
            {
                if (sale.Stock > 0)
                {
                    ++countDay;
                }

                countSales += sale.Sales;
            }
        }

        return countDay != 0 
            ? (double)countSales / countDay 
            : throw new ProductNotFoundException("Sorry, no such product found");
    }

    public double GetSalesPrediction(long productId, long days)
    {
        CheckDayRange(days);
        
        DateTime currentDate = DateTime.Today;   
        DateTime lastDate = currentDate.AddDays(days);
        double result = 0;
        double ads = GetAverageDaySales(productId);

        while (lastDate != currentDate)
        {
            double seasonalityFactor = _seasonalityProducts.GetCoefficient(productId, lastDate.Month);
            long deltaDays = days > lastDate.Day ? lastDate.Day : days;
            days -= deltaDays;
            lastDate = lastDate.AddDays(-deltaDays);
            result += seasonalityFactor * deltaDays * ads;
        }

        return result;
    }

    public double GetSalesDemand(long productId, long days)
    {
        CheckDayRange(days);
        
        var allSales = _saleHistory.GetAllSales();
        SaleRecord? lastDayProduct = null;
        var maxDateTime = DateTime.MinValue;
        var prediction = GetSalesPrediction(productId, days);
        
        foreach (var sale in allSales)
        {
            if (productId == sale.Id && sale.Date > maxDateTime)
            {
                lastDayProduct = sale;
            }
        }

        if (lastDayProduct == null)
        {
            throw new ProductNotFoundException("Sorry, no such product found");
        }

        return prediction - lastDayProduct.Stock < 0 
            ? 0 
            : prediction - lastDayProduct.Stock;
    }

    private static void CheckDayRange(long days)
    {
        var maxDayCount = (DateTime.MaxValue - DateTime.Today).Days;
        
        if (days < 1 || days > maxDayCount)
        {
            throw new DayOutOfRangeException("It is impossible to calculate for such a number of days.");
        }
    }
}