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
        if (File.Exists(@"..\..\..\..\..\TestsFileJson\SeasonalityProducts.json"))
            _seasonalityProducts = JsonSerializer.Deserialize<List<SeasonalityRecord>>(
                File.ReadAllText(@"..\..\..\..\..\TestsFileJson\SeasonalityProducts.json"))!;
        else
            _seasonalityProducts = JsonSerializer.Deserialize<List<SeasonalityRecord>>(
                File.ReadAllText(@"..\..\TestsFileJson\SeasonalityProducts.json"))!;
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