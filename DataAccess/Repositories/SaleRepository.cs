using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;
            
public class SaleRepository : ISaleRepository
{
    private static IReadOnlyList<Sale> _saleHistory = new List<Sale>();
    
    public SaleRepository(string pathFileData)
    {
        pathFileData = pathFileData.SettingSeparatorOs();
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        _saleHistory = JsonSerializer.Deserialize<List<Sale>>(
            File.ReadAllText(pathFileData), serializeOptions)!;
    }

    public IReadOnlyList<Sale> GetAllSales()
    {
        return _saleHistory;
    }
}