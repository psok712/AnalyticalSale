namespace Domain.Interfaces;

public interface ISeasonalityProductRepository
{
    double GetCoefficient(long productId, int month);
}