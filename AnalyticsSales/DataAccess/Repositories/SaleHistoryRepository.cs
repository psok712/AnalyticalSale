using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Domain.Interfaces;
using Domain.Models;

namespace DataAccess.Repositories;

public class SaleHistoryRepository : ISaleHistoryRepository
{
    private static IReadOnlyList<SaleRecord> _saleHistory = new List<SaleRecord>();

    public SaleHistoryRepository()
    {
        if (File.Exists(@"..\..\..\..\..\TestsFileJson\HistorySales.json"))
            _saleHistory = JsonSerializer.Deserialize<List<SaleRecord>>(
                File.ReadAllText(@"..\..\..\..\..\TestsFileJson\HistorySales.json"))!;
        else
            _saleHistory = JsonSerializer.Deserialize<List<SaleRecord>>(
                File.ReadAllText(@"..\..\TestsFileJson\HistorySales.json"))!;
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