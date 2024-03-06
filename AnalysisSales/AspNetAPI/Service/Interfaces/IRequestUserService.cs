using Microsoft.AspNetCore.Http.HttpResults;

namespace AspNetAPI.Service.Interfaces;

public interface IRequestUserService
{
    Results<NotFound, Ok<double>> GetSalesDemand(long idProduct, long days);
    Results<NotFound, Ok<double>> GetSalesPrediction(long idProduct, long days);
    Results<NotFound, Ok<double>> GetAverageDaySales(long idProduct);
}