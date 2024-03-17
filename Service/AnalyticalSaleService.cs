using System.Text.Json.Nodes;
using DataAccess.Exceptions;
using DataAccess.Repositories;
using Domain.Interfaces;
namespace Service;

public class AnalyticalSaleService : IAnalyticalSaleService
{                                               
    private readonly ISaleRepository _sale;
    private readonly ISeasonalityProductRepository _seasonalityProduct;

    public AnalyticalSaleService()
    {
        _sale = 
            new SaleRepository(GetPathFileDirectory("SaleHistoryRepository"));
        _seasonalityProduct = 
            new SeasonalityProductRepository(GetPathFileDirectory("SeasonalityProductsRepository"));
    }
    
    public double GetAverageDaySales(long productId)
    {
        var allSales = _sale.GetAllSales();
        var countDay = allSales.Count(el => el.Id == productId && el.Stock > 0);
        var countSales = allSales
            .Where(el => el.Id == productId)
            .Sum(el => el.Sales);
        
        return countDay == 0
            ? throw new ProductNotFoundException("Sorry, no such product found")
            : (double)countSales / countDay;
    }

    public double GetSalesPrediction(long productId, long days)
    {
        CheckDayRange(days);
        
        var prediction = 0d;
        var ads = GetAverageDaySales(productId);
        var currentDate = DateTime.Today;   
        var lastDate = currentDate.AddDays(days);

        while (lastDate != currentDate)
        {
            var seasonalityFactor = _seasonalityProduct.GetCoefficient(productId, lastDate.Month);
            var deltaDays = days > lastDate.Day ? lastDate.Day : days;
            days -= deltaDays;
            lastDate = lastDate.AddDays(-deltaDays);
            prediction += seasonalityFactor * deltaDays * ads;
        }

        return prediction;
    }

    public double GetSalesDemand(long productId, long days)
    {
        CheckDayRange(days);
        
        var prediction = GetSalesPrediction(productId, days);
        var lastDayProduct = _sale
            .GetAllSales()
            .Where(el => el.Id == productId)
            .MaxBy(el => el.Date);
        
        if (lastDayProduct is null)
            throw new ProductNotFoundException("Sorry, no such product found");

        return prediction - lastDayProduct.Stock > 0 
            ? prediction - lastDayProduct.Stock 
            : 0;
    }

    private static void CheckDayRange(long days)
    {
        var maxDayCount = (DateTime.MaxValue - DateTime.Today).Days;
        
        if (days < 1 || days > maxDayCount)
        {
            throw new DayOutOfRangeException("It is impossible to calculate for such a number of days.");
        }
    }

    private static string GetPathFileDirectory(string nameDirectory)
    {
        var pathConfigFile = "appsettings.json";
        
        while (!File.Exists(pathConfigFile) && pathConfigFile.Length < 100)
        {
            pathConfigFile = "../" + pathConfigFile;
        }

        if (!File.Exists(pathConfigFile))
            throw new FileNotFoundException("This file does not exist in your directory!");
        
        var allSettingsText = File.ReadAllText(pathConfigFile);
        var jsonNode = JsonNode.Parse(allSettingsText)!;
        
        return (string)jsonNode[nameDirectory]!;
    }
}