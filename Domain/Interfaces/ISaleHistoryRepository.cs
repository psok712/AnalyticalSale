using Domain.Models;

namespace Domain.Interfaces;

public interface ISaleHistoryRepository
{
    IReadOnlyList<SaleRecord> GetAllSales();
}