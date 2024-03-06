using Domain.Models;

namespace Domain.Interfaces;

public interface ISaleHistoryRepository
{
    IReadOnlyList<SaleRecord> GetSalesById(long idProduct);
    IReadOnlyList<SaleRecord> GetAllSales();
}