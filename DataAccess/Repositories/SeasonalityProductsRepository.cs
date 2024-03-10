using System.Text.Json;
using DataAccess.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;

public class SeasonalityProductsRepository : ISeasonalityProductsRepository
{
    private static IReadOnlyList<SeasonalityRecord> _seasonalityProducts = new List<SeasonalityRecord>();
    public SeasonalityProductsRepository()
    {
        String pathTestFileConsole = @"..\..\..\..\TestsFileJson\SeasonalityProducts.json";
        String pathTestFileAsp = @"..\TestsFileJson\SeasonalityProducts.json";

        if (OperatingSystem.IsIOS() || OperatingSystem.IsLinux())
        {
            pathTestFileConsole = pathTestFileConsole.Replace(@"\", "/");
            pathTestFileAsp = pathTestFileAsp.Replace(@"\", "/");
        }
        
        if (File.Exists(pathTestFileConsole))
            _seasonalityProducts = JsonSerializer.Deserialize<List<SeasonalityRecord>>(
                File.ReadAllText(pathTestFileConsole))!;
        else
            _seasonalityProducts = JsonSerializer.Deserialize<List<SeasonalityRecord>>(
                File.ReadAllText(pathTestFileAsp))!;
    }
    
    public double GetCoefficient(long idProduct, int month)
    {
        foreach (var product in _seasonalityProducts)
        {
            if (product.Id == idProduct && product.Month == month)
            {
                return product.Coef;
            }
        }

        throw new ProductNotFoundException(
            "Sorry, no such product with a seasonality factor was found in this month..");
    }
}