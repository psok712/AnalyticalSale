namespace Domain.Interfaces;

public interface ISeasonalityProductsRepository
{
    double GetCoefficient(long idProduct, int month);
}