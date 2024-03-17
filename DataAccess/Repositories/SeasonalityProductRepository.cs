using System.Text.Json;
using DataAccess.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;

public class SeasonalityProductRepository : ISeasonalityProductRepository
{
    private static IReadOnlyList<SeasonalityProduct> _seasonalityProducts = new List<SeasonalityProduct>();
    
    public SeasonalityProductRepository(string pathFileData)
    {
        pathFileData = pathFileData.SettingSeparatorOs();
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        _seasonalityProducts = JsonSerializer.Deserialize<List<SeasonalityProduct>>(
            File.ReadAllText(pathFileData), serializeOptions)!;
    }
    
    public double GetCoefficient(long productId, int month)
    {
        var result = _seasonalityProducts
            .Where(el => el.Id == productId && el.Month == month)
            .ToList();

        return result.Count == 0
            ? throw new ProductNotFoundException(
                "Sorry, no such product with a seasonality factor was found in this month..")
            : result[0].Coef;
    }
}