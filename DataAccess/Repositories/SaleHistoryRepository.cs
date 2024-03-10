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

    public IReadOnlyList<SaleRecord> GetAllSales()
    {
        return _saleHistory;
    }
}