using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;

public class SaleHistoryRepository : ISaleHistoryRepository
{
    private static IReadOnlyList<SaleRecord> _saleHistory = new List<SaleRecord>();

    public SaleHistoryRepository()
    {
        String pathTestFileConsole = @"..\..\..\..\TestsFileJson\HistorySales.json";
        String pathTestFileAsp =@"..\TestsFileJson\HistorySales.json";

        if (OperatingSystem.IsIOS() || OperatingSystem.IsLinux())
        {
            pathTestFileConsole = pathTestFileConsole.Replace(@"\", "/");
            pathTestFileAsp = pathTestFileAsp.Replace(@"\", "/");
        }
        
        if (File.Exists(pathTestFileConsole))
            _saleHistory = JsonSerializer.Deserialize<List<SaleRecord>>(
                File.ReadAllText(pathTestFileConsole))!;
        else
            _saleHistory = JsonSerializer.Deserialize<List<SaleRecord>>(
                File.ReadAllText(pathTestFileAsp))!;
    }
    
    public IReadOnlyList<SaleRecord> GetSalesById(long idProduct)
    {
        List<SaleRecord> salesProduct = new List<SaleRecord>();

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