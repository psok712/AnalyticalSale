using DataAccess.Exceptions;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess;

public class AnalysisSalesService : IAnalysisSalesService
{
    private readonly ISaleHistoryRepository _saleHistory = new SaleHistoryRepository();
    private readonly ISeasonalityProductsRepository _seasonalityProducts = new SeasonalityProductsRepository();
    
    public double GetAverageDaySales(long idProduct)
    {
        var allSales = _saleHistory.GetAllSales();
        long countDay = 0;
        long countSales = 0;

        foreach (var sale in allSales)
        {
            if (sale.Id == idProduct)
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

    public double GetSalesPrediction(long idProduct, long days)
    {
        CheckDayRange(days);
        
        DateTime currentDate = DateTime.Today;   
        DateTime lastDate = currentDate.AddDays(days);
        double result = 0;
        double ads = GetAverageDaySales(idProduct);

        while (lastDate != currentDate)
        {
            double seasonalityFactor = _seasonalityProducts.GetCoefficient(idProduct, lastDate.Month);
            long deltaDays = days > lastDate.Day ? lastDate.Day : days;
            days -= deltaDays;
            lastDate = lastDate.AddDays(-deltaDays);
            result += seasonalityFactor * deltaDays * ads;
        }

        return result;
    }

    public double GetSalesDemand(long idProduct, long days)
    {
        CheckDayRange(days);
        
        var allSales = _saleHistory.GetAllSales();
        SaleRecord? lastDayProduct = null;
        DateTime maxDateTime = DateTime.MinValue;
        
        foreach (var sale in allSales)
        {
            if (idProduct == sale.Id && sale.Date > maxDateTime)
            {
                lastDayProduct = sale;
            }
        }

        if (lastDayProduct == null)
        {
            throw new ProductNotFoundException("Sorry, no such product found");
        }

        return GetSalesPrediction(idProduct, days) - lastDayProduct.Stock;
    }

    private static void CheckDayRange(long day)
    {
        if (day < 1 || day > (DateTime.MaxValue - DateTime.Today).Days)
        {
            throw new DayOutOfRangeException("It is impossible to calculate for such a number of days.");
        }
    }
}