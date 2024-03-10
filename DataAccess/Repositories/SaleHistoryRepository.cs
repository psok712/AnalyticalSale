using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;
            
public class SaleHistoryRepository : ISaleHistoryRepository
{
    private static IReadOnlyList<SaleRecord> _saleHistory = new List<SaleRecord>();
    
    public SaleHistoryRepository(String pathFileData = @"..\TestsFileJson\HistorySales.json")
    {
        var separator = Path.DirectorySeparatorChar.ToString();
        pathFileData = pathFileData.Replace(@"\", separator);
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        _saleHistory = JsonSerializer.Deserialize<List<SaleRecord>>(
            File.ReadAllText(pathFileData), serializeOptions)!;
    }
    
    public IReadOnlyList<SaleRecord> GetSalesById(long idProduct)
    {
        var salesProduct = new List<SaleRecord>();

        foreach (var el in _saleHistory)
        {
            if (el.Id == idProduct)
            {
                salesProduct.Add(el);
            }
        }

        return salesProduct;
    }

    public IReadOnlyList<SaleRecord> GetAllSales()
    {
        return _saleHistory;
    }
}