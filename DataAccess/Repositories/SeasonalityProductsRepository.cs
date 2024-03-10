using System.Text.Json;
using DataAccess.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;

public class SeasonalityProductsRepository : ISeasonalityProductsRepository
{
    private static IReadOnlyList<SeasonalityRecord> _seasonalityProducts = new List<SeasonalityRecord>();
    
    public SeasonalityProductsRepository(String pathFileData = @"..\TestsFileJson\SeasonalityProducts.json")
    {
        var separator = Path.DirectorySeparatorChar.ToString();
        pathFileData = pathFileData.Replace(@"\", separator);
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        _seasonalityProducts = JsonSerializer.Deserialize<List<SeasonalityRecord>>(
            File.ReadAllText(pathFileData), serializeOptions)!;
    }
    
    public double GetCoefficient(long idProduct, int month)
    {
        var result = _seasonalityProducts
            .Where(el => el.Id == idProduct && el.Month == month)
            .ToList();

        return result.Count == 0
            ? throw new ProductNotFoundException(
                "Sorry, no such product with a seasonality factor was found in this month..")
            : result[0].Coef;
    }
}