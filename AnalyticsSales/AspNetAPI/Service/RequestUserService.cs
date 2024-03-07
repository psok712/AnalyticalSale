using AnalysisSales.Models;
using AspNetAPI.Service.Interfaces;
using DataAccess;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AspNetAPI.Service;

public class RequestUserService : IRequestUserService
{
    public Results<NotFound, Ok<double>> GetAverageDaySales(long idProduct)
    {
        var statusCode 
            = CountRequestUser(CommandRequestEnum.Ads, idProduct, out double result);
        
        return statusCode == StatusCodes.Status200OK
            ? TypedResults.Ok(result)
            : TypedResults.NotFound();
    }
    
    public Results<NotFound, Ok<double>> GetSalesDemand(long idProduct, long days)
    {
        var statusCode 
            = CountRequestUser(CommandRequestEnum.Demand, idProduct, out double result, days);

        return statusCode == StatusCodes.Status200OK
            ? TypedResults.Ok(result)
            : TypedResults.NotFound();
    }

    public Results<NotFound, Ok<double>> GetSalesPrediction(long idProduct, long days)
    {
        var statusCode 
            = CountRequestUser(CommandRequestEnum.Prediction, idProduct, out double result, days);
        
        return statusCode == StatusCodes.Status200OK
            ? TypedResults.Ok(result)
            : TypedResults.NotFound();
    }
    
    private int CountRequestUser(CommandRequestEnum command, long idProduct, out double res, long days = 1)
    {
        res = 0;
        
        if (idProduct < 0 || days < 1 || days > (DateTime.MaxValue - DateTime.Now).Days)
            return StatusCodes.Status404NotFound;
        
        IAnalysisSalesService service = new AnalysisSalesService();
        try
        {
            switch (command)
            {
                case CommandRequestEnum.Ads:
                    res = service.GetAverageDaySales(idProduct);
                    break;
                case CommandRequestEnum.Demand:
                    res = service.GetSalesDemand(idProduct, days);
                    break;
                case CommandRequestEnum.Prediction:
                    res = service.GetSalesPrediction(idProduct, days);
                    break;
                default:
                    return StatusCodes.Status404NotFound;
            }
        }
        catch (Exception)
        {
            return StatusCodes.Status404NotFound;
        }

        return StatusCodes.Status200OK;
    }
}