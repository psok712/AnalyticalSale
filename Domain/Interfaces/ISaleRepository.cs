using Domain.Models;

namespace Domain.Interfaces;

public interface ISaleRepository
{
    IReadOnlyList<Sale> GetAllSales();
}