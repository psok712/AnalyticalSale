using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;

public class SaleHistoryRepository : ISaleHistoryRepository
{
    private static readonly IReadOnlyList<SaleRecord> SaleHistory = 
        JsonSerializer.Deserialize<List<SaleRecord>>(
            File.ReadAllText(@"TestsFileJson\HistorySales.json"))!;
    
    public IReadOnlyList<SaleRecord> GetSalesById(long idProduct)
    {
        List<SaleRecord> salesProduct = new List<SaleRecord>();

        foreach (var el in SaleHistory)
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
        return SaleHistory;
    }
}